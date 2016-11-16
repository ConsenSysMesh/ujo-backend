using System.Threading.Tasks;

namespace CCC.BlockchainProcessing
{
    public interface IBlockhainProcessProgressService
    {
        Task UpsertBlockNumberProcessedTo(ulong blockNumber);
        Task<ulong> GetBlockNumberToProcessFrom();
        Task<ulong> GetBlockNumberToProcessTo();
    }
}