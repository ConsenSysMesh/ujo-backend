using System.Threading.Tasks;

namespace Ujo.Work.Services
{
    public interface IStandardDataRegistry
    {
        Task<bool> ExistsAsync(string contractAddress);
    }
}