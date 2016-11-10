using System.Threading.Tasks;
using CCC.StandardDataProcessing;
using Nethereum.Web3;

namespace Ujo.Work.Services
{
    public interface IStandardDataChangedService<in T>
    {
        Task UpsertAsync(T work);
        Task DataChangedAsync(T model, EventLog<DataChangedEvent> dataEventLog);
        Task RemovedAsync(string contractAddress);
    }
}