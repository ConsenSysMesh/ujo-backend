using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Web3;
using Ujo.Search.Service;
using Ujo.Work.Services;
using Ujo.Work.Storage;

namespace Ujo.Work.WebJob
{
    public class Functions
    {
        //TODO: Add Gridmail notification of errors
        //TODO: Queue blocks if a specific block has an error we can continue processing other blocks.
        //TODO: Queue block processing should be fast and allowed to be in parallel, we should only use change notifications to retrieve value of the contract
        // we should validate if a contract is registered

        [Singleton]
        public static async Task ProcessWorks([TimerTrigger("00:00:05")] TimerInfo timer,
            [Table("Work")] CloudTable workTable, [Table("WorkRegistry")] CloudTable workRegistryTable, TextWriter log,
            [Queue("IpfsCoverImageProcessingQueue")] ICollector<string> ipfsImageProcesssinQueue
        )
        {
            log.WriteLine("Start job");
            var workProcessor = GetWorkProcessorService(workTable, workRegistryTable, ipfsImageProcesssinQueue);
            var workBlockProcessor = GetWorkChainProcessorService(workTable, workRegistryTable, workProcessor, log);

            await workBlockProcessor.ProcessLatestBlocks();

            log.WriteLine("Finished job");
        }

        public static async Task ProcessRegistrationsAndUnregistrations(
            [QueueTrigger("WorkRegisteredQueue")] string regunregaddress, [Table("Work")] CloudTable workTable,
            [Table("WorkRegistry")] CloudTable workRegistryTable, TextWriter log,
            [Queue("IpfsCoverImageProcessingQueue")] ICollector<string> ipfsImageProcesssinQueue)
        {
            var workProcessor = GetWorkProcessorService(workTable, workRegistryTable, ipfsImageProcesssinQueue);
            //TODO: Remove format use type objects
            //format:
            //Reg:Address
            //Unreg:Adddress
            var info = regunregaddress.Split(':');
            var operation = info[0];
            var address = info[1];

            if (operation.ToLower() == "reg")
                await workProcessor.ProcessWorkAsync(address);

            if (operation.ToLower() == "unreg")
                await workProcessor.RemoveUnregisteredAsync(address);
        }

        private static WorkChainProcessor GetWorkChainProcessorService(CloudTable workProcesssTable,
            CloudTable workProcessRegistryTable,
            WorkProcessorService workProcessorService, TextWriter logWriter)
        {
            var workProcessRepository = new WorkProcessInfoRepository(workProcesssTable);
            var workRegistryProcessInfoRepository = new WorkRegistryProcessInfoRepository(workProcessRegistryTable);
            return new WorkChainProcessor(workProcessorService, logWriter,
                ConfigurationSettings.StartProcessFromBlockNumber(), workProcessRepository,
                workRegistryProcessInfoRepository);
        }


        private static WorkProcessorService GetWorkProcessorService(CloudTable workTable, CloudTable workRegistryTable,
            ICollector<string> ipfsImageProcesssinQueue)
        {
            var web3 = new Web3(ConfigurationSettings.GetEthereumRpcUrl());
            var workSearchService = GetWorkSearchService();
            var workRepository = new WorkRepository(workTable);
            var workRegistryRepository = new WorkRegistryRepository(workRegistryTable);
            var ipfsQueue = new IpfsImageQueue(ipfsImageProcesssinQueue);

            return new WorkProcessorService(workSearchService, ipfsQueue, workRepository, web3,
                workRegistryRepository);
        }

        private static WorkSearchService GetWorkSearchService()
        {
            return new WorkSearchService(ConfigurationSettings.GetSearchApiServiceName(),
                ConfigurationSettings.GetSearchApiWorkIndexName(), ConfigurationSettings.GetSearchApiSearchAdminKey(),
                ConfigurationSettings.GetSearchApiWorkIndexName());
        }
    }
}