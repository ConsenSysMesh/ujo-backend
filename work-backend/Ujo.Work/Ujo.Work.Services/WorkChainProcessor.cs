using System.IO;
using System.Threading.Tasks;
using Ujo.Work.Storage;
using Ujo.Work.WebJob;

namespace Ujo.Work.Services
{
    public class WorkChainProcessor
    {
        private readonly WorkProcessorService _workProcessorService;
        private TextWriter _logWriter;
        private readonly long _defaultBlockNumber;
        private readonly WorkProcessInfoRepository _workProcessInfoRepository;
        private readonly WorkRegistryProcessInfoRepository _workRegistryProcessInfoRepository;

        public WorkChainProcessor(WorkProcessorService workProcessorService, TextWriter logWriter, long defaultBlockNumber,
            WorkProcessInfoRepository workProcessInfoRepository, WorkRegistryProcessInfoRepository workRegistryProcessInfoRepository)
        {
            _workProcessorService = workProcessorService;
            _logWriter = logWriter;
            _defaultBlockNumber = defaultBlockNumber;
            _workProcessInfoRepository = workProcessInfoRepository;
            _workRegistryProcessInfoRepository = workRegistryProcessInfoRepository;
        }

        public async Task ProcessLatestBlocks()
        {
            _logWriter.WriteLine("Getting current block number to process from");

            var blockNumberFrom = await GetBlockNumberToProcessFrom();
            var blockNumberTo = await GetBlockNumberToProcessTo();

            if (blockNumberTo < blockNumberFrom) return;
            //process max 100 at a time?
            if (blockNumberTo - blockNumberFrom > 100)
                blockNumberTo = blockNumberFrom + 100;


            _logWriter.WriteLine("Getting all data changes events from: " + blockNumberFrom + " to " + blockNumberTo);
            await _workProcessorService.ProcessWorksAsync(blockNumberFrom, blockNumberTo);
            _logWriter.WriteLine("Updating current process progres to:" + blockNumberTo);
            await UpsertBlockNumberProcessedTo(blockNumberTo);
        }
    

        private async Task UpsertBlockNumberProcessedTo(long blockNumber)
        {
            var processInfo = _workProcessInfoRepository.NewProcessInfo(blockNumber);
            await processInfo.InsertOrReplaceAsync();
        }

        public async Task<long> GetBlockNumberToProcessFrom()
        {
            var processInfo = await _workProcessInfoRepository.FindAsync();

            var blockNumber = _defaultBlockNumber;

            if (processInfo != null)
            {
                blockNumber = processInfo.Number;
            }
            return blockNumber;
        }

        public async Task<long> GetBlockNumberToProcessTo()
        {
            var processInfo = await _workRegistryProcessInfoRepository.FindAsync();
            //if there is no setting we process to our current default setting
            var blockNumber = _defaultBlockNumber;

            if (processInfo != null)
            {
                blockNumber = processInfo.Number;
            }
            return blockNumber;
        }
    }
}