using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;

namespace CCC.BlockchainProcessing
{
    public class BlockchainLogProcessor : IBlockchainLogProcessor
    {
        private Web3 _web3;
        private IEnumerable<ILogProcessor> _logProcessors;

        public BlockchainLogProcessor(IEnumerable<ILogProcessor> logProcessors, Web3 web3)
        {
            this._logProcessors = logProcessors;
            this._web3 = web3;
        }

        public async Task ProcessLogsAsync(ulong fromBlockNumber, ulong toBlockNumber)
        {
            var logs = await _web3.Eth.Filters.GetLogs.SendRequestAsync(new NewFilterInput
            {
                FromBlock = new BlockParameter(fromBlockNumber),
                ToBlock = new BlockParameter(toBlockNumber)
            });

            if (logs == null) return;

            var processingCollection = new List<LogsMatchedForProcessing>();

            foreach (var logProcessor in _logProcessors)
            {
                processingCollection.Add(new LogsMatchedForProcessing(logProcessor));
            }

            foreach (var log in logs)
            {
                foreach (var matchedForProcessing in processingCollection)
                {
                    matchedForProcessing.AddIfMatched(log);
                }
            }

            foreach (var matchedForProcessing in processingCollection)
            {
                await matchedForProcessing.ProcessLogsAsync();
            }
        }
    }
}