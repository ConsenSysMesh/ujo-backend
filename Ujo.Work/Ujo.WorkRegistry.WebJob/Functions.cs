
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CCC.Contracts.Registry.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Ujo.WorkRegistry.Service;
using Ujo.WorkRegistry.Storage;
using Wintellect.Azure.Storage.Table;

namespace Ujo.WorkRegistry.WebJob
{
    public class Functions
    {
        
        [Singleton]
        public static async Task ProcessWorkRegistry([TimerTrigger("00:00:05")] TimerInfo timer,
            [Table("WorkRegistry")] CloudTable tableBinding, TextWriter log,
            [Queue("WorkRegisteredQueue")] ICollector<string> workRegisteredUnregisteredQueue)
        {
            log.WriteLine("Start job");
            var web3 = new Web3(ConfigurationSettings.GetEthereumRPCUrl());
            var service = new RegistryService(web3, ConfigurationSettings.GetWorkRegistryContractAddress());
            var byteCodeMatcher = new WorkRegistryWorkByteCodeMatcher(web3);
            var workRegistryTable = new AzureTable(tableBinding);

            log.WriteLine("Getting current block number to process from");

            var blockNumberFrom = await GetBlockNumberToProcessFrom(workRegistryTable) - 1;

            //Getting current blockNumber to set the progress later on
            var currentBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            var processTo = currentBlockNumber.Value;
            if ((long) processTo - blockNumberFrom > 50)
            {
                processTo = blockNumberFrom + 50;
            }

            for (long i = blockNumberFrom; i <= processTo; i++)
            {
                try
                {
                    log.WriteLine("Getting all events of registered and unregistered from: " + i + "to: " +
                                  i);
                    var eventLogs = await service.GetRegisteredUnregistered(i, i);

                    log.WriteLine("Found total of registered and unregistered logs: " + eventLogs.Count);

                    foreach (var eventLog in eventLogs)
                    {
                        if (eventLog is EventLog<RegisteredEvent>)
                        {
                           if(await byteCodeMatcher.IsMatchAsync((eventLog as EventLog<RegisteredEvent>).Event.RegisteredAddress))
                           {
                                await
                                    ProcessRegisteredWork(workRegisteredUnregisteredQueue, eventLog, workRegistryTable,
                                        log);
                           }
                        }
                        else if (eventLog is EventLog<UnregisteredEvent>)
                        {
                            await
                                ProcessUnregistedWork(workRegisteredUnregisteredQueue, eventLog, workRegistryTable, log);
                        }
                        else
                        {
                            log.WriteLine("Unknown event type, should not reach here");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Put in error storage to log and process later on
                    // add a generic job to process just block numbers
                    System.Diagnostics.Trace.TraceError("Work Registry error, BlockNumber " + i + " Error:" +
                        ex.StackTrace.ToString());

                    log.WriteLine("Error:" + ex.Message);
                }
            }
            log.WriteLine("Updating current process progres to:" + processTo);
            await UpsertBlockNumberProcessedTo(workRegistryTable, (long)processTo);
        }

        private static async Task UpsertBlockNumberProcessedTo(AzureTable table, long blockNumber)
        {
            var processInfo =  ProcessInfo.Create(table, blockNumber);
            await processInfo.InsertOrReplaceAsync();
        }

        private static async Task<long> GetBlockNumberToProcessFrom(AzureTable workRegistryTable)
        {
            var processInfo = await ProcessInfo.FindAsync(workRegistryTable);

            var blockNumber = ConfigurationSettings.StartProcessFromBlockNumber();

            if (processInfo != null)
            {
                blockNumber = processInfo.Number;
            }
            return blockNumber;
        }

        private static async Task ProcessUnregistedWork(ICollector<string> workRegisteredQueue, object eventLog, AzureTable workRegistryTable, TextWriter log)
        {
            var unregisteredEvent = eventLog as EventLog<UnregisteredEvent>;
            log.WriteLine("Unregistering " + unregisteredEvent.Event.RegisteredAddress);
            var workRegistryRecord = await WorkRegistryRecord.FindAsync(workRegistryTable,
                unregisteredEvent.Event.RegisteredAddress);
            if (workRegistryRecord != null)
            {
                await workRegistryRecord.DeleteAsync();
            }
            workRegisteredQueue.Add("Unreg:" + unregisteredEvent.Event.RegisteredAddress);
        }

        private static async Task ProcessRegisteredWork(ICollector<string> workRegisteredQueue, object eventLog, AzureTable workRegistryTable, TextWriter log)
        {
            //TODO: get code verify bytecode matches, simple hard coding to start with
            var registeredEvent = eventLog as EventLog<RegisteredEvent>;
            log.WriteLine("Registering " + registeredEvent.Event.RegisteredAddress);
            var workRegistryRecord = WorkRegistryRecord.Create(workRegistryTable,
                registeredEvent.Event.RegisteredAddress,
                registeredEvent.Event.Owner,
                registeredEvent.Event.Time,
                registeredEvent.Event.Id);
            await workRegistryRecord.InsertOrReplaceAsync();
            workRegisteredQueue.Add("Reg:" + registeredEvent.Event.RegisteredAddress);
        }
    }
}
