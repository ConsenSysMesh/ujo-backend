using Nethereum.Web3;

namespace Ujo.Work.Service
{
    public class WorkServiceBase
    {
        protected readonly Web3 web3;

        public const string ABI =
            @"[{'constant':false,'inputs':[{'name':'_key','type':'bytes32'}],'name':'sha3OfValueAtKey','outputs':[{'name':'','type':'bytes32'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'}],'name':'registerWorkWithRegistry','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_keys','type':'bytes32[]'},{'name':'vals','type':'string'},{'name':'_standard','type':'bool'}],'name':'bulkSetValue','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_newController','type':'address'}],'name':'changeController','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'},{'name':'_license','type':'address'}],'name':'registerLicenseAndAttachToThisWork','outputs':[],'payable':false,'type':'function'},{'constant':true,'inputs':[{'name':'','type':'bytes32'}],'name':'store','outputs':[{'name':'','type':'string'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'data','type':'bytes32'}],'name':'bytes32ToString','outputs':[{'name':'','type':'string'}],'payable':false,'type':'function'},{'constant':true,'inputs':[],'name':'schema_addr','outputs':[{'name':'','type':'address'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'}],'name':'unregisterWorkWithRegistry','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'x','type':'address'}],'name':'addressToBytes','outputs':[{'name':'b','type':'bytes'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'},{'name':'_license','type':'address'}],'name':'unregisterLicense','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_key','type':'bytes32'},{'name':'_value','type':'string'},{'name':'_standard','type':'bool'}],'name':'setValue','outputs':[],'payable':false,'type':'function'},{'inputs':[{'name':'_schema_addr','type':'address'}],'type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'key','type':'bytes32'},{'indexed':false,'name':'value','type':'string'}],'name':'StandardDataChanged','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'key','type':'bytes32'},{'indexed':false,'name':'value','type':'string'}],'name':'NonStandardDataChanged','type':'event'}]";

        protected Contract contract;

        public WorkServiceBase(Web3 web3)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(ABI, null);
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

        public Function GetSchemaAddressFunction()
        {
            return contract.GetFunction("schema_addr");
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