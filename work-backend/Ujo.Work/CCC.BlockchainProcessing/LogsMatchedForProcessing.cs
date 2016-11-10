using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.Filters;

namespace CCC.BlockchainProcessing
{
    public class LogsMatchedForProcessing
    {
        public LogsMatchedForProcessing(ILogProcessor logProcessor)
        {
            LogProcessor = logProcessor;
            MatchedLogs = new List<FilterLog>();
        }

        public ILogProcessor LogProcessor { get; }
        public List<FilterLog> MatchedLogs { get; }

        public void AddIfMatched(FilterLog log)
        {
            if (LogProcessor.IsLogForEvent(log))
            {
                MatchedLogs.Add(log);
            }
        }

        public async Task ProcessLogsAsync()
        {
            await LogProcessor.ProcessLogsAsync(this.MatchedLogs.ToArray());
        }
    }
}