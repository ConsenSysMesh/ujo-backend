using System.Threading.Tasks;

namespace CCC.BlockchainProcessing
{
    public interface IBlockProcessProgressRepository
    {
        Task UpsertProgressAsync(ulong blockNumber);
        Task<ulong?> GetLatestAsync();
    }
}