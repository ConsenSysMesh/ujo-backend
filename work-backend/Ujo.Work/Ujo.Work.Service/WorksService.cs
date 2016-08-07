using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ujo.Work.Service
{
    public class WorksService:WorkServiceBase
    {
        public async Task<List<EventLog<DataChangedEvent>>> GetDataChangedEventsAsync(ulong fromBlockNumber, ulong toBlockNumber)
        {
            //we do one block at a time to avoid huge json rpc responses, this can be run multithreaded..
            var results = new List<EventLog<DataChangedEvent>>();
            for (ulong i = fromBlockNumber; i <= toBlockNumber; i++)
            {
                var result = await GetDataChangedEventsAsync(i);
                results.AddRange(result);
            }
            return results;
        }
        

        public async Task<List<EventLog<DataChangedEvent>>> GetDataChangedEventsAsync(ulong blockNumber)
        {
            var filterId = await web3.Eth.Filters.NewFilter.SendRequestAsync(new NewFilterInput()
            {
                FromBlock = new BlockParameter(blockNumber),
                ToBlock = new BlockParameter(blockNumber)
            });

            var logs = await web3.Eth.Filters.GetFilterLogsForEthNewFilter.SendRequestAsync(filterId);

            var results = new List<EventLog<DataChangedEvent>>();
            var dataChangeEvent = GetDataChangedEvent();

            if (logs == null) return results;

            foreach (var log in logs)
            {
                if (!IsDataChangedLog(log)) continue;

                var eventLog = dataChangeEvent.DecodeAllEvents<DataChangedEvent>(new[] {log})[0];
                results.Add(eventLog);
            }

            return results;
        }

        public bool IsDataChangedLog(JToken log)
        {
            return IsDataChangedLog(JsonConvert.DeserializeObject<FilterLog>(log.ToString()));
        }

        public bool IsDataChangedLog(FilterLog log)
        {
            //todo move to Nethereum or just make it public
            var eventAbi = this.contract.ContractABI.Events.First(x => x.Name == "DataChanged");
            //todo move check to events
            if (log.Topics != null && log.Topics.Length > 0)
            {
                var eventtopic = log.Topics[0].ToString();
                if (SafeHexComparison(eventAbi.Sha33Signature) == SafeHexComparison(eventtopic))
                    return true;
            }
            return false;
        }

        private string SafeHexComparison(string hex)
        {
            if (hex.StartsWith("0x")) return hex.ToLower();
            return "0x" + hex.ToLower();
        }

        public WorksService(Web3 web3) : base(web3)
        {
        }
    }
}