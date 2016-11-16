using System.Threading.Tasks;

namespace CCC.BlockchainProcessing
{
    public class ChildBlockBlockchainProcessProgressService : BlockhainProcessProgressService
    {
        private readonly IBlockProcessProgressRepository _parentBlockProcessProgressRepository;

        public ChildBlockBlockchainProcessProgressService(IBlockProcessProgressRepository parentBlockProcessProgressRepository, ulong defaultBlockNumber, IBlockProcessProgressRepository blockProcessProgressRepository) : base(defaultBlockNumber, blockProcessProgressRepository)
        {
            _parentBlockProcessProgressRepository = parentBlockProcessProgressRepository;
        }

        public override async Task<ulong> GetBlockNumberToProcessTo()
        {
            var block = await _parentBlockProcessProgressRepository.GetLatestAsync();
            if (block == null) return _defaultBlockNumber;
            return block.Value;
        }
    }
}