using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Ujo.Work.Service;
using Ujo.WorkRegistry.Storage;
using Wintellect.Azure.Storage.Table;
using ProcessInfo = Ujo.Work.Storage.ProcessInfo;
using Ujo.Search.Service;

namespace Ujo.Work.WebJob
{
    public class Functions
    {
        //TODO: Add Gridmail notification of errors
        //TODO: Queue blocks if a specific block has an error we can continue processing other blocks.
        //TODO: Queue block processing should be fast and allowed to be in parallel, we should only use change notifications to retrieve value of the contract
        // we should validate if a contract is registered


        [Singleton]
        public static async Task ProcessWorks([TimerTrigger("00:00:30")] TimerInfo timer,
            [Table("Work")] CloudTable tableBinding, [Table("WorkRegistry")] CloudTable workRegistryCloudTable, TextWriter log,
            [Queue("IpfsCoverImageProcessingQueue")] ICollector<string> ipfsImageProcesssinQueue
            )
        {
            log.WriteLine("Start job");
            var web3 = new Web3(ConfigurationSettings.GetEthereumRPCUrl());
            

            var service = new WorksService(web3);
            var worksTable = new AzureTable(tableBinding);
            var workRegistryTable = new AzureTable(workRegistryCloudTable);

            log.WriteLine("Getting current block number to process from");

            var blockNumber = await GetBlockNumberToProcessFrom(worksTable);
            var blockNumberToProcessTo = await GetBlockNumberToProcessTo(workRegistryTable);

            if (blockNumberToProcessTo < blockNumber) return;
            //process max 100 at a time?
            if (blockNumberToProcessTo - blockNumber > 100)
                blockNumberToProcessTo = blockNumber + 100;
            

            log.WriteLine("Getting all data changes events from: " + blockNumber + " to " + blockNumberToProcessTo);
            var dataEventLogs = await service.GetDataChangedEventsAsync(Convert.ToUInt64(blockNumber), Convert.ToUInt64(blockNumberToProcessTo));

            //TODO: ensure sorted
            foreach (var dataEventLog in dataEventLogs)
            {
                var address = dataEventLog.Log.Address;
                if (await WorkRegistryRecord.ExistsAsync(workRegistryTable, address))
                {
                    var work = await Storage.WorkEntity.FindAsync(worksTable, address);
                    if (work == null)
                    {
                       await ProcessNewWork(web3, address, worksTable, ipfsImageProcesssinQueue);
                    }
                    else
                    {
                       await ProcessDataChangeUpdate(work, dataEventLog, ipfsImageProcesssinQueue);
                    }
                }

            }
            log.WriteLine("Updating current process progres to:" + blockNumberToProcessTo);
            await UpsertBlockNumberProcessedTo(worksTable, blockNumberToProcessTo);

        }
    
        private static async Task ProcessDataChangeUpdate(Storage.WorkEntity workEntity, EventLog<DataChangedEvent> dataEventLog, ICollector<string> ipfsImageProcesssinQueue)
        {

            var address = dataEventLog.Log.Address;
            var web3 = new Web3(ConfigurationSettings.GetEthereumRPCUrl());

            var workService = new WorkService(web3, address);
            var workSearchService = GetWorkSearchService();

            Model.Work work = null;
            work = await workService.GetWorkAsync();
            

            var key = dataEventLog.Event.Key;
            var val = dataEventLog.Event.Value;

            WorkSchema schemaField;

            if (Enum.TryParse(key, out schemaField))
            {
                workEntity.Initialise(work);
            }
            else
            {
                workEntity.SetUnknownKey(val, key);
            }

            if (key == WorkSchema.image.ToString())
            {
                workEntity.CoverFileIpfsHash = work.CoverImageIpfsHash;
                ipfsImageProcesssinQueue.Add(work.CoverImageIpfsHash);
            }
            else if (key == WorkSchema.audio.ToString())
            {
                workEntity.WorkFileIpfsHash = work.WorkFileIpfsHash;
            }

            await workSearchService.UploadOrMergeAsync(work);
            await workEntity.InsertOrMergeAsync();
        }
        
        public static async Task ProcessRegistrationsAndUnregistrations([QueueTrigger("WorkRegisteredQueue")] string regunregaddress, [Table("Work")] CloudTable tableBinding, TextWriter log, [Queue("IpfsCoverImageProcessingQueue")]
        ICollector<string> ipfsImageProcesssinQueue)
        {
            //TODO: Remove format use type objects
            //format:
            //Reg:Address
            //Unreg:Adddress
            var info = regunregaddress.Split(':');
            var operation = info[0];
            var address = info[1];
            var web3 = new Web3(ConfigurationSettings.GetEthereumRPCUrl());
            var worksTable = new AzureTable(tableBinding);

            if (operation.ToLower()  == "reg")
            {
                await ProcessNewWork(web3, address, worksTable, ipfsImageProcesssinQueue);
            }

            if(operation.ToLower() == "unreg")
            {
                await RemoveUnregistered(address, worksTable);
            }
        }

        private static async Task RemoveUnregistered(string address, AzureTable worksTable)
        {
            var workStore = await Storage.WorkEntity.FindAsync(worksTable, address);
            var workSearchService = GetWorkSearchService();
            if (workStore != null)
            {
                await workStore.DeleteAsync();
                await workSearchService.DeleteAsync(address);
            }
        }

        private static async Task ProcessNewWork(Web3 web3, string address, AzureTable worksTable, ICollector<string> ipfsImageProcesssinQueue)
        {
            var workService = new WorkService(web3, address);
            Model.Work work = null;
            work = await workService.GetWorkAsync();
            
            if (work != null)
            {
                var workStore = Storage.WorkEntity.Create(worksTable, work);
                var workSearchService = GetWorkSearchService();
                await workSearchService.UploadOrMergeAsync(work);
                var result = await workStore.InsertOrReplaceAsync();
                if (!string.IsNullOrEmpty(work.CoverImageIpfsHash))
                {
                    ipfsImageProcesssinQueue.Add(work.CoverImageIpfsHash);
                }
            }
        }

        private static async Task UpsertBlockNumberProcessedTo(AzureTable table, long blockNumber)
        {
            var processInfo = ProcessInfo.Create(table, blockNumber);
            await processInfo.InsertOrReplaceAsync();
        }

        private static async Task<long> GetBlockNumberToProcessFrom(AzureTable workTable)
        {
            var processInfo = await ProcessInfo.FindAsync(workTable);

            var blockNumber = ConfigurationSettings.StartProcessFromBlockNumber();

            if (processInfo != null)
            {
                blockNumber = processInfo.Number;
            }
            return blockNumber;
        }

        private static async Task<long> GetBlockNumberToProcessTo(AzureTable workTable)
        {
            var processInfo = await WorkRegistry.Storage.ProcessInfo.FindAsync(workTable);
            //if there is no setting we process to our current default setting
            var blockNumber = ConfigurationSettings.StartProcessFromBlockNumber();

            if (processInfo != null)
            {
                blockNumber = processInfo.Number;
            }
            return blockNumber;
        }

        private static WorkSearchService GetWorkSearchService()
        {
            return new WorkSearchService(ConfigurationSettings.GetSearchApiServiceName(), ConfigurationSettings.GetSearchApiWorkIndexName(), ConfigurationSettings.GetSearchApiSearchAdminKey(), ConfigurationSettings.GetSearchApiWorkIndexName());
        }
    }
}
