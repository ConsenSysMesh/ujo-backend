using System.IO;
using CCC.BlockchainProcessing;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Web3;
using Ujo.Repository;
using Ujo.Repository.Infrastructure;
using Ujo.Work.Search.Service;
using Ujo.Work.Services;
using Ujo.Work.Storage;
using Ujo.WorkRegistry.Storage;
using ProcessInfoRepository = Ujo.Work.Storage.ProcessInfoRepository;

namespace Ujo.Work.WebJob
{
    public class Bootstrap
    {
        public static BlokchainBatchProcessorService InitialiseBlockchainBatchProcessorService(CloudTable workProcesssTable,
            CloudTable workProcessRegistryTable,
            WorkDataLogProcessor workProcessorService, TextWriter logWriter, Web3 web3)
        {
            var workProcessRepository = new ProcessInfoRepository(workProcesssTable);
            var workRegistryProcessInfoRepository = new ProcessInfoRepository(workProcessRegistryTable);
            var blockchainLogProcessor = new BlockchainLogProcessor(new [] {workProcessorService}, web3);
            var logger = new TextWriterLogger(logWriter);
            var childBlockchainProcessorService = new ChildBlockBlockchainProcessProgressService(workRegistryProcessInfoRepository,
                ConfigurationSettings.StartProcessFromBlockNumber(), workProcessRepository
            );
            return new BlokchainBatchProcessorService(blockchainLogProcessor, logger, childBlockchainProcessorService,
                50);
        }

        public static WorkDataLogProcessor InitialiseWorkDataLogProcessor(CloudTable workTable, CloudTable workRegistryTable,
            ICollector<string> ipfsImageProcesssinQueue)
        {
            var web3 = new Web3(ConfigurationSettings.GetEthereumRpcUrl());
            var workSearchService = InitialiseWorkSearchService();
            var workRepository = new WorkRepository(workTable);
            var workRegistryRepository = new WorkRegistryRepository(workRegistryTable);
            var ipfsQueue = new IpfsImageQueue(ipfsImageProcesssinQueue);
            Ujo.Repository.MappingBootstrapper.Initialise();
            var musicRecordingService = new MusicRecordingService(new UnitOfWork(new UjoContext(ConfigurationSettings.GetRepositoryConnectionString())));

            return WorkDataLogProcessor.Create(web3, workRegistryRepository, ipfsQueue, workRepository, workSearchService, musicRecordingService);
        }

        public static WorkSearchService InitialiseWorkSearchService()
        {
            return new WorkSearchService(ConfigurationSettings.GetSearchApiServiceName(),
                ConfigurationSettings.GetSearchApiWorkIndexName(), ConfigurationSettings.GetSearchApiSearchAdminKey(),
                ConfigurationSettings.GetSearchApiWorkIndexName());
        }
    }
}