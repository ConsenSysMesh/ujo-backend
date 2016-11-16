using System.Threading.Tasks;

namespace CCC.Contracts.StandardData.Processing
{
    public interface IStandardDataRegistry
    {
        Task<bool> ExistsAsync(string contractAddress);
    }
}