using System.Threading.Tasks;
using CCC.Contracts.StandardData.Services.Model;
using Nethereum.Web3;

namespace CCC.Contracts.StandardData.Processing
{
    public interface IStandardDataProcessingService<in T>
    {
        Task UpsertAsync(T work);
        Task DataChangedAsync(T model, EventLog<DataChangedEvent> dataEventLog);
        Task RemovedAsync(string contractAddress);
    }
}