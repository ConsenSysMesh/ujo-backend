using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;

namespace Ujo.Work.Services.Ethereum
{
    public class WorksService : WorkServiceBase
    {
        public WorksService(Web3 web3) : base(web3)
        {
        }

        public async Task<List<EventLog<DataChangedEvent>>> GetDataChangedEventsAsync(ulong fromBlockNumber,
            ulong toBlockNumber)
        {
            //we do one block at a time to avoid huge json rpc responses, this can be run multithreaded..
            var results = new List<EventLog<DataChangedEvent>>();
            for (var i = fromBlockNumber; i <= toBlockNumber; i++)
            {
                var result = await GetDataChangedEventsAsync(i);
                results.AddRange(result);
            }
            return results;
        }

        public bool IsStandardDataChangeLog(FilterLog log)
        {
            var dataChangeEvent = GetStandardDataChangedEvent();
            return dataChangeEvent.IsLogForEvent(log);
        }

        public bool IsStandardDataChangeLog(JToken log)
        {
            var dataChangeEvent = GetStandardDataChangedEvent();
            return dataChangeEvent.IsLogForEvent(log);
        }

        public async Task<List<EventLog<DataChangedEvent>>> GetDataChangedEventsAsync(ulong blockNumber)
        {
            var logs = await Web3.Eth.Filters.GetLogs.SendRequestAsync(new NewFilterInput
            {
                FromBlock = new BlockParameter(blockNumber),
                ToBlock = new BlockParameter(blockNumber)
            });

            var results = new List<EventLog<DataChangedEvent>>();

            if (logs == null) return results;

            foreach (var log in logs)
            {
                if (!IsStandardDataChangeLog(log)) continue;
                var eventLog = Event.DecodeAllEvents<DataChangedEvent>(new[] {log})[0];
                results.Add(eventLog);
            }
            return results;
        }
    }
}