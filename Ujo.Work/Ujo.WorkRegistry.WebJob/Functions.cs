
using System;
using System.IO;
using System.Threading.Tasks;
using CCC.BlockchainProcessing;
using CCC.Contracts;
using CCC.Contracts.Registry.Processing;
using CCC.Contracts.Registry.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Web3;
using Ujo.WorkRegistry.Storage;
using Wintellect.Azure.Storage.Table;
using Ujo.Work.Services.Ethereum;

namespace Ujo.WorkRegistry.WebJob
{

    public class Functions
    {
        
        [Singleton]
        public static async Task ProcessWorkRegistry([TimerTrigger("00:00:05")] TimerInfo timer,
            [Table("WorkRegistry")] CloudTable tableBinding, TextWriter log,
            [Queue("WorkRegisteredQueue")] ICollector<RegistrationMessage> workRegisteredUnregisteredQueue)
        {
            log.WriteLine("Start job");

            var workRegistryTable = new AzureTable(tableBinding);

            var web3 = new Web3(ConfigurationSettings.GetEthereumRPCUrl());

            var blockchainRegistryProcessor = Bootstrap.InitialiseBlockchainRegistryProcessor(log, workRegisteredUnregisteredQueue, web3, workRegistryTable);
            var batchProcessor = Bootstrap.InitialiseBatchProcessorService(blockchainRegistryProcessor,
                workRegistryTable, log, web3);

            await batchProcessor.ProcessLatestBlocks();
        }
    }
}
