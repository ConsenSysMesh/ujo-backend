using System.Threading.Tasks;

namespace CCC.BlockchainProcessing
{
    public abstract class BlockhainProcessProgressService: IBlockhainProcessProgressService
    {
        protected ulong _defaultBlockNumber;
        private readonly IBlockProcessProgressRepository _blockProcessProgressRepository;

        public BlockhainProcessProgressService(ulong defaultBlockNumber, IBlockProcessProgressRepository blockProcessProgressRepository)
        {
            _defaultBlockNumber = defaultBlockNumber;
            _blockProcessProgressRepository = blockProcessProgressRepository;
        }

        public async Task UpsertBlockNumberProcessedTo(ulong blockNumber)
        {
            await _blockProcessProgressRepository.UpsertProgressAsync(blockNumber);
        }

        public async Task<ulong> GetBlockNumberToProcessFrom()
        {
            var processInfo = await _blockProcessProgressRepository.GetLatestAsync();

            var blockNumber = _defaultBlockNumber;

            if (processInfo != null)
            {
                blockNumber = processInfo.Value;
            }
            return blockNumber;
        }

        public abstract Task<ulong> GetBlockNumberToProcessTo();

    }
}