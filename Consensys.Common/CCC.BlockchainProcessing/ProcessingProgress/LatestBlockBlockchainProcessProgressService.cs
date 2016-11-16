using System.Threading.Tasks;
using Nethereum.Web3;

namespace CCC.BlockchainProcessing
{
    public class LatestBlockBlockchainProcessProgressService : BlockhainProcessProgressService
    {
        private readonly Web3 _web3;

        public LatestBlockBlockchainProcessProgressService(Web3 web3, ulong defaultBlockNumber, IBlockProcessProgressRepository blockProcessProgressRepository) : base(defaultBlockNumber, blockProcessProgressRepository)
        {
            _web3 = web3;
        }

        public override async Task<ulong> GetBlockNumberToProcessTo()
        {
            var block = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            return (ulong) block.Value;
        }
    }
}