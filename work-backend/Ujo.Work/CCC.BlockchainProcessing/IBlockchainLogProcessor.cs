using System.Threading.Tasks;

namespace CCC.BlockchainProcessing
{
    public interface IBlockchainLogProcessor
    {
        Task ProcessLogsAsync(ulong fromBlockNumber, ulong toBlockNumber);
    }
}