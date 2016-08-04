using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ujo.ContractRegistry
{
    public class ContractRegistryService
    {
        private readonly Web3 web3;

        private string abi =
            @"[{""constant"":false,""inputs"":[{""name"":""registeredAddress"",""type"":""address""}],""name"":""unregister"",""outputs"":[],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""registeredAddress"",""type"":""address""}],""name"":""register"",""outputs"":[],""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""address""}],""name"":""records"",""outputs"":[{""name"":""registeredAddress"",""type"":""address""},{""name"":""owner"",""type"":""address""},{""name"":""time"",""type"":""uint256""},{""name"":""Id"",""type"":""uint256""}],""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""uint256""}],""name"":""workRegistered"",""outputs"":[{""name"":"""",""type"":""address""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""numRecords"",""outputs"":[{""name"":"""",""type"":""uint256""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""maxId"",""outputs"":[{""name"":"""",""type"":""uint256""}],""type"":""function""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""registeredAddress"",""type"":""address""},{""indexed"":true,""name"":""id"",""type"":""uint256""},{""indexed"":true,""name"":""owner"",""type"":""address""},{""indexed"":false,""name"":""time"",""type"":""uint256""}],""name"":""Registered"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""registeredAddress"",""type"":""address""},{""indexed"":true,""name"":""id"",""type"":""uint256""}],""name"":""Unregistered"",""type"":""event""}]";

        private Contract contract;

        public ContractRegistryService(Web3 web3, string address)
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

        public Task<string> GetWorkRegisteredAsyncCall(Int64 id)
        {
            var function = GetWorkRegisteredFunction();
            return function.CallAsync<string>();
        }


        public Function GetNumRecordsFunction()
        {
            return contract.GetFunction("numRecords");
        }

        public Task<Int64> NumRecordsAsyncCall()
        {
            var function = GetNumRecordsFunction();
            return function.CallAsync<Int64>();
        }

        public Function GetMaxIdFunction()
        {
            return contract.GetFunction("maxId");
        }

        public Task<Int64> MaxIdAsyncCall()
        {
            var function = GetMaxIdFunction();
            return function.CallAsync<Int64>();
        }
    }
}
