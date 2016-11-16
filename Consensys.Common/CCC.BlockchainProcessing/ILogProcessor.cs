using System.Threading.Tasks;
using Nethereum.RPC.Eth.Filters;

namespace CCC.BlockchainProcessing
{
    public interface ILogProcessor
    {
        bool IsLogForEvent(FilterLog log);

        Task ProcessLogsAsync(params FilterLog[] eventLogs);
    }
}