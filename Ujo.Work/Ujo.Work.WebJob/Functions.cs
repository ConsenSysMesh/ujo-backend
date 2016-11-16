using System.IO;
using System.Threading.Tasks;
using CCC.BlockchainProcessing;
using CCC.Contracts.Registry.Processing;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Web3;
using Ujo.Work.Search.Service;
using Ujo.Work.Services;
using Ujo.Work.Storage;
using Ujo.WorkRegistry.Storage;

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
            var web3 = new Web3(ConfigurationSettings.GetEthereumRpcUrl());
            var workProcessor = Bootstrap.InitialiseWorkDataLogProcessor(workTable, workRegistryTable, ipfsImageProcesssinQueue);
            var workBlockProcessor = Bootstrap.InitialiseBlockchainBatchProcessorService(workTable, workRegistryTable, workProcessor, log, web3);

            await workBlockProcessor.ProcessLatestBlocks();

            log.WriteLine("Finished job");
        }

        public static async Task ProcessRegistrationsAndUnregistrations(
            [QueueTrigger("WorkRegisteredQueue")] RegistrationMessage registrationMessage, [Table("Work")] CloudTable workTable,
            [Table("WorkRegistry")] CloudTable workRegistryTable, TextWriter log,
            [Queue("IpfsCoverImageProcessingQueue")] ICollector<string> ipfsImageProcesssinQueue)
        {
            var workProcessor = Bootstrap.InitialiseWorkDataLogProcessor(workTable, workRegistryTable, ipfsImageProcesssinQueue);
            
            if (registrationMessage.Registered)
                await workProcessor.ProcessFullUpsertAsync(registrationMessage.Address);

            if (!registrationMessage.Registered)
                await workProcessor.RemoveUnregisteredAsync(registrationMessage.Address);
        }
    }
}