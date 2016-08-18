using Nethereum.Web3;

namespace Ujo.Work.Service
{
    public class WorkServiceBase
    {
        protected readonly Web3 web3;

        protected string abi =
            @"[{""constant"":false,""inputs"":[{""name"":""_key"",""type"":""bytes32""}],""name"":""sha3OfValueAtKey"",""outputs"":[{""name"":"""",""type"":""bytes32""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_registry"",""type"":""address""}],""name"":""registerWorkWithRegistry"",""outputs"":[],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_keys"",""type"":""bytes32[]""},{""name"":""vals"",""type"":""string""},{""name"":""_standard"",""type"":""bool""}],""name"":""bulkSetValue"",""outputs"":[],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_newController"",""type"":""address""}],""name"":""changeController"",""outputs"":[],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_registry"",""type"":""address""},{""name"":""_license"",""type"":""address""}],""name"":""registerLicenseAndAttachToThisWork"",""outputs"":[],""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""bytes32""}],""name"":""store"",""outputs"":[{""name"":"""",""type"":""string""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_registry"",""type"":""address""}],""name"":""unregisterWorkWithRegistry"",""outputs"":[],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_registry"",""type"":""address""},{""name"":""_license"",""type"":""address""}],""name"":""unregisterLicense"",""outputs"":[],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_key"",""type"":""bytes32""},{""name"":""_value"",""type"":""string""},{""name"":""_standard"",""type"":""bool""}],""name"":""setValue"",""outputs"":[],""type"":""function""},{""inputs"":[{""name"":""_schema_addr"",""type"":""address""}],""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""key"",""type"":""bytes32""},{""indexed"":false,""name"":""value"",""type"":""string""}],""name"":""StandardDataChanged"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""key"",""type"":""bytes32""},{""indexed"":false,""name"":""value"",""type"":""string""}],""name"":""NonStandardDataChanged"",""type"":""event""}]";

        protected Contract contract;

        public WorkServiceBase(Web3 web3)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(abi, null);
        }

        public Event GetStandardDataChangedEvent()
        {
            return contract.GetEvent("StandardDataChanged");
        }
        public Event GetNonStandardDataChangedEvent()
        {
            return contract.GetEvent("NonStandardDataChanged");
        }

        public Function GetRegisterLicenseAndAttachToThisWorkFunction()
        {
            return contract.GetFunction("registerLicenseAndAttachToThisWork");
        }

        public Function GetStoreFunction()
        {
            return contract.GetFunction("store");
        }

        public Function GetSetValueFunction()
        {
            return contract.GetFunction("setValue");
        }

        public Function GetBulkSetValueFunction()
        {
            return contract.GetFunction("bulkSetValue");
        }

        public Function GetRegisterWorkWithRegistryFunction()
        {
            return contract.GetFunction("registerWorkWithRegistry");
        }

        public Function GetChangeControllerFunction()
        {
            return contract.GetFunction("changeController");
        }

        public Function GetUnregisterWorkWithRegistryFunction()
        {
            return contract.GetFunction("unregisterWorkWithRegistry");
        }

        public Function GetUnregisterLicenseFunction()
        {
            return contract.GetFunction("unregisterLicense");
        }
        public Function GetSha3OfValueAtKeyFunction()
        {
            return contract.GetFunction("sha3OfValueAtKey");
        }
    }

}