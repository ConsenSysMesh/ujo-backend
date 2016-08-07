
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Ujo.ContractRegistry;
using Ujo.WorkRegistry.Service;
using Ujo.WorkRegistry.Storage;
using Wintellect.Azure.Storage.Table;

namespace Ujo.WorkRegistry.WebJob
{
    public class Functions
    {

        [Singleton]
        public static async Task ProcessWorks([TimerTrigger("00:01:00")] TimerInfo timer,
            [Table("WorkRegistry")] CloudTable tableBinding, TextWriter log,
            [Queue("WorkRegisteredQueue")] ICollector<string> workRegisteredQueue)
        {
            log.WriteLine("Start job");
            var web3 = new Web3(ConfigurationSettings.GetEthereumRPCUrl());
            var service = new WorkRegistryService(web3, ConfigurationSettings.GetWorkRegistryContractAddress());
            var workRegistryTable = new AzureTable(tableBinding);

            log.WriteLine("Getting current block number to process from");

            var blockNumber = await GetBlockNumberToProcessFrom(workRegistryTable);

            //Getting current blockNumber to set the progress later on
            var currentBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            log.WriteLine("Getting all events of registerd and unregistered from" + blockNumber);
            var eventLogs = await service.GetRegisteredUnregisteredFromBlockNumber(blockNumber);
            foreach (var eventLog in eventLogs)
            {
                //TODO: what happens if something is registered and then unregistered and then registered again
                //do we a) check for for each one if is still registered or unregistered if opposite ignore it?
                //changes might have happened in later stages so is not an option better to process sequentially
                //do we b) Get the oldest if duplicated <<< TODO
                // c continue as it stands 

                if (eventLog is EventLog<RegisteredEvent>)
                {
                    await ProcessRegisteredWork(workRegisteredQueue, eventLog, workRegistryTable);
                }

                if (eventLog is EventLog<UnregisteredEvent>)
                {
                    await ProcessUnregistedWork(workRegisteredQueue, eventLog, workRegistryTable);
                }
            }
            log.WriteLine("Updating current process progres to:" + currentBlockNumber.Value);
            await UpsertBlockNumberProcessedTo(workRegistryTable, (long)currentBlockNumber.Value);
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

        private static async Task ProcessUnregistedWork(ICollector<string> workRegisteredQueue, object eventLog, AzureTable workRegistryTable)
        {
            var unregisteredEvent = eventLog as EventLog<UnregisteredEvent>;
            var workRegistryRecord = await WorkRegistryRecord.FindAsync(workRegistryTable,
                unregisteredEvent.Event.RegisteredAddress);
            if (workRegistryRecord != null)
            {
                await workRegistryRecord.DeleteAsync();
            }
            workRegisteredQueue.Add("Unreg:" + unregisteredEvent.Event.RegisteredAddress);
        }

        private static async Task ProcessRegisteredWork(ICollector<string> workRegisteredQueue, object eventLog, AzureTable workRegistryTable)
        {
            var registeredEvent = eventLog as EventLog<RegisteredEvent>;
            var workRegistryRecord = WorkRegistryRecord.Create(workRegistryTable,
                registeredEvent.Event.RegisteredAddress,
                registeredEvent.Event.Owner,
                registeredEvent.Event.Time,
                registeredEvent.Event.Id);
            await workRegistryRecord.InsertOrReplaceAsync();
            workRegisteredQueue.Add("Reg:" + registeredEvent.Event.RegisteredAddress);
        }


        /// <summary>
        /// Counts the frequency of characters in a word (triggered by messages created by "CountAndSplitInWords")
        /// </summary>
        public static void CharFrequency([QueueTrigger("words")] string word, TextWriter log)
        {
            // Create a dictionary of character frequencies
            //      Key = the character
            //      Value = number of times that character appears in a word
            IDictionary<char, int> frequency = word
                .GroupBy(c => c)
                .ToDictionary(group => group.Key, group => group.Count());

            log.WriteLine("The frequency of letters in the word \"{0}\" is: ", word);
            foreach (var character in frequency)
            {
                log.WriteLine("{0}: {1}", character.Key, character.Value);
            }
        }
    }
}
