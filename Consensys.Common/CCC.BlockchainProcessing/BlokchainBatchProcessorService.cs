using System.Threading.Tasks;

namespace CCC.BlockchainProcessing
{
    public class BlokchainBatchProcessorService
    {
        private readonly IBlockchainProcessor _blockchainProcessor;
        private readonly ILogger _logger;
        private readonly IBlockhainProcessProgressService _blockhainProcessProgressService;
        private readonly ulong _maxNumberOfBlocksPerBatch = 100;

        public BlokchainBatchProcessorService(IBlockchainProcessor blockchainProcessor, 
            ILogger logger,
            IBlockhainProcessProgressService  blockhainProcessProgressService, ulong? maxNumberOfBlocksPerBatch = null)
        {
            _blockchainProcessor = blockchainProcessor;
            _logger = logger;
            _blockhainProcessProgressService = blockhainProcessProgressService;
            if(maxNumberOfBlocksPerBatch != null)
                _maxNumberOfBlocksPerBatch = maxNumberOfBlocksPerBatch.Value;
        }

        public async Task ProcessLatestBlocks()
        {
            _logger.WriteLine("Getting current block number to process from");

            var blockNumberFrom = await _blockhainProcessProgressService.GetBlockNumberToProcessFrom();
            var blockNumberTo = await _blockhainProcessProgressService.GetBlockNumberToProcessTo();

            if (blockNumberTo < blockNumberFrom) return;
            //process max 100 at a time?
            if (blockNumberTo - blockNumberFrom > _maxNumberOfBlocksPerBatch)
                blockNumberTo = blockNumberFrom + _maxNumberOfBlocksPerBatch;

            _logger.WriteLine("Getting all data changes events from: " + blockNumberFrom + " to " + blockNumberTo);
            await _blockchainProcessor.ProcessAsync(blockNumberFrom, blockNumberTo);
            _logger.WriteLine("Updating current process progres to:" + blockNumberTo);
            await _blockhainProcessProgressService.UpsertBlockNumberProcessedTo(blockNumberTo);
        }

    }
}