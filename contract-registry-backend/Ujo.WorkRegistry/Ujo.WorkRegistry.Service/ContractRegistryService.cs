using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Nethereum.Web3.Contracts.Comparers;
using Ujo.ContractRegistry;

namespace Ujo.WorkRegistry.Service
{
    public class WorkRegistryService
    {
        private readonly Web3 web3;

        private string abi =
            @"[{""constant"":false,""inputs"":[{""name"":""registeredAddress"",""type"":""address""}],""name"":""unregister"",""outputs"":[],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""registeredAddress"",""type"":""address""}],""name"":""register"",""outputs"":[],""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""address""}],""name"":""records"",""outputs"":[{""name"":""registeredAddress"",""type"":""address""},{""name"":""owner"",""type"":""address""},{""name"":""time"",""type"":""uint256""},{""name"":""Id"",""type"":""uint256""}],""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""uint256""}],""name"":""workRegistered"",""outputs"":[{""name"":"""",""type"":""address""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""numRecords"",""outputs"":[{""name"":"""",""type"":""uint256""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""maxId"",""outputs"":[{""name"":"""",""type"":""uint256""}],""type"":""function""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""registeredAddress"",""type"":""address""},{""indexed"":true,""name"":""id"",""type"":""uint256""},{""indexed"":true,""name"":""owner"",""type"":""address""},{""indexed"":false,""name"":""time"",""type"":""uint256""}],""name"":""Registered"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""registeredAddress"",""type"":""address""},{""indexed"":true,""name"":""id"",""type"":""uint256""}],""name"":""Unregistered"",""type"":""event""}]";

        private Contract contract;

        public WorkRegistryService(Web3 web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(abi, address);
        }

        public Event GetRegisteredEvent()
        {
            return contract.GetEvent("Registered");
        }

        public Event GetUnregisteredEvent()
        {
            return contract.GetEvent("Unregistered");
        }

        public Function GetUnregisterFunction()
        {
            return contract.GetFunction("unregister");
        }

     
        public async Task<List<EventLog<RegisteredEvent>>> GetRegistered(BigInteger blockNumberFrom, BigInteger blockNumberTo)
        {
            var registeredEvent = GetRegisteredEvent();
            var filterInput = registeredEvent.CreateFilterInput(new BlockParameter(new HexBigInteger(blockNumberFrom)), new BlockParameter(new HexBigInteger(blockNumberTo)));
            return await registeredEvent.GetAllChanges<RegisteredEvent>(filterInput);
          
        }

        public async Task<List<EventLog<UnregisteredEvent>>> GetUnregistered(BigInteger blockNumberFrom,
           BigInteger blockNumberTo)
        {
            var unregisteredEvent = GetUnregisteredEvent();
            var filterInput = unregisteredEvent.CreateFilterInput(new BlockParameter(new HexBigInteger(blockNumberFrom)), new BlockParameter(new HexBigInteger(blockNumberTo)));
            return await unregisteredEvent.GetAllChanges<UnregisteredEvent>(filterInput);
        }


        public async Task<List<object>> GetRegisteredUnregistered(BigInteger blockNumberFrom, BigInteger blockNumberTo)
        {
            var registered = await GetRegistered(blockNumberFrom, blockNumberTo);
            var unregistered = await GetUnregistered(blockNumberFrom, blockNumberTo);
            var list = new List<object>();
            list.AddRange(registered);
            list.AddRange(unregistered);
            list.Sort(new EventLogBlockNumberTransactionIndexComparer());
            return list;
        }

        public async Task<List<EventLog<RegisteredEvent>>> GetRegisteredFromBlockNumber(BigInteger blockNumber)
        {
            var registeredEvent = GetRegisteredEvent();
            var filterInput = registeredEvent.CreateFilterInput(new BlockParameter(new HexBigInteger(blockNumber)));
            return await registeredEvent.GetAllChanges<RegisteredEvent>(filterInput);
        }

        public async Task<List<EventLog<UnregisteredEvent>>> GetUnregisteredFromBlockNumber(BigInteger blockNumber)
        {
            var unregisteredEvent = GetUnregisteredEvent();
            var filterInput = unregisteredEvent.CreateFilterInput(new BlockParameter(new HexBigInteger(blockNumber)));
            return await unregisteredEvent.GetAllChanges<UnregisteredEvent>(filterInput).ConfigureAwait(false);
        }

        public async Task<List<object>> GetRegisteredUnregisteredFromBlockNumber(BigInteger blockNumber)
        {
            var registered = await GetRegisteredFromBlockNumber(blockNumber);
            var unregistered = await GetUnregisteredFromBlockNumber(blockNumber);
            var list = new List<object>();
            list.AddRange(registered);
            list.AddRange(unregistered);
            list.Sort(new EventLogBlockNumberTransactionIndexComparer());
            return list;
        } 

        public Task<string> UnregisterAsync(string addressFrom, string registeredAddress,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetUnregisterFunction();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, registeredAddress);
        }

        public Function GetRegisterFunction()
        {
            return contract.GetFunction("register");
        }

        public Task<string> RegisterAsync(string addressFrom, string registeredAddress,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetRegisterFunction();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, registeredAddress);
        }

        public Function GetRecordsFunction()
        {
            return contract.GetFunction("records");
        }

        public Task<RegistryRecord> GetRecordAsyncCall(string address)
        {
            var function = GetRecordsFunction();
            return function.CallDeserializingToObjectAsync<RegistryRecord>(address);
        }

        public Function GetWorkRegisteredFunction()
        {
            return contract.GetFunction("workRegistered");
        }

        public Task<string> GetWorkRegisteredAsyncCall(long id)
        {
            var function = GetWorkRegisteredFunction();
            return function.CallAsync<string>(id);
        }


        public Function GetNumRecordsFunction()
        {
            return contract.GetFunction("numRecords");
        }

        public Task<long> NumRecordsAsyncCall()
        {
            var function = GetNumRecordsFunction();
            return function.CallAsync<long>();
        }

        public Function GetMaxIdFunction()
        {
            return contract.GetFunction("maxId");
        }

        public Task<long> MaxIdAsyncCall()
        {
            var function = GetMaxIdFunction();
            return function.CallAsync<long>();
        }
    }
}
