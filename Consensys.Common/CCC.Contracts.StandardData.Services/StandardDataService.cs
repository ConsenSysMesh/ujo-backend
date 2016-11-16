using System.Collections.Generic;
using System.Threading.Tasks;
using CCC.Contracts.StandardData.Services.Model;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;

namespace CCC.Contracts.StandardData.Services
{
    public class StandardDataService
    {
        public string Abi { get; protected set; } =
            @"[{'constant':false,'inputs':[{'name':'_key','type':'bytes32'}],'name':'sha3OfValueAtKey','outputs':[{'name':'','type':'bytes32'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'}],'name':'registerWorkWithRegistry','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_keys','type':'bytes32[]'},{'name':'vals','type':'string'},{'name':'_standard','type':'bool'}],'name':'bulkSetValue','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_newController','type':'address'}],'name':'changeController','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'},{'name':'_license','type':'address'}],'name':'registerLicenseAndAttachToThisWork','outputs':[],'payable':false,'type':'function'},{'constant':true,'inputs':[{'name':'','type':'bytes32'}],'name':'store','outputs':[{'name':'','type':'string'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'data','type':'bytes32'}],'name':'bytes32ToString','outputs':[{'name':'','type':'string'}],'payable':false,'type':'function'},{'constant':true,'inputs':[],'name':'schema_addr','outputs':[{'name':'','type':'address'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'}],'name':'unregisterWorkWithRegistry','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'x','type':'address'}],'name':'addressToBytes','outputs':[{'name':'b','type':'bytes'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'},{'name':'_license','type':'address'}],'name':'unregisterLicense','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_key','type':'bytes32'},{'name':'_value','type':'string'},{'name':'_standard','type':'bool'}],'name':'setValue','outputs':[],'payable':false,'type':'function'},{'inputs':[{'name':'_schema_addr','type':'address'}],'type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'key','type':'bytes32'},{'indexed':false,'name':'value','type':'string'}],'name':'StandardDataChanged','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'key','type':'bytes32'},{'indexed':false,'name':'value','type':'string'}],'name':'NonStandardDataChanged','type':'event'}]";


        protected readonly Web3 Web3;

        protected Contract Contract;

        public StandardDataService(Web3 web3)
        {
            Web3 = web3;
            Contract = web3.Eth.GetContract(Abi, null);
        }

        public Event GetStandardDataChangedEvent()
        {
            return Contract.GetEvent("StandardDataChanged");
        }

        public Event GetNonStandardDataChangedEvent()
        {
            return Contract.GetEvent("NonStandardDataChanged");
        }

        public Function GetStoreFunction()
        {
            return Contract.GetFunction("store");
        }

        public Function GetSchemaAddressFunction()
        {
            return Contract.GetFunction("schema_addr");
        }

        public Function GetSetValueFunction()
        {
            return Contract.GetFunction("setValue");
        }

        public Function GetBulkSetValueFunction()
        {
            return Contract.GetFunction("bulkSetValue");
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
                var eventLog = Event.DecodeAllEvents<DataChangedEvent>(new[] { log })[0];
                results.Add(eventLog);
            }
            return results;
        }
    }
}