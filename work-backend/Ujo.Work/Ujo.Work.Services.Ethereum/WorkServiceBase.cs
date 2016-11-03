using Nethereum.Web3;

namespace Ujo.Work.Services.Ethereum
{
    public class WorkServiceBase
    {
        public const string Abi =
            @"[{'constant':false,'inputs':[{'name':'_key','type':'bytes32'}],'name':'sha3OfValueAtKey','outputs':[{'name':'','type':'bytes32'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'}],'name':'registerWorkWithRegistry','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_keys','type':'bytes32[]'},{'name':'vals','type':'string'},{'name':'_standard','type':'bool'}],'name':'bulkSetValue','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_newController','type':'address'}],'name':'changeController','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'},{'name':'_license','type':'address'}],'name':'registerLicenseAndAttachToThisWork','outputs':[],'payable':false,'type':'function'},{'constant':true,'inputs':[{'name':'','type':'bytes32'}],'name':'store','outputs':[{'name':'','type':'string'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'data','type':'bytes32'}],'name':'bytes32ToString','outputs':[{'name':'','type':'string'}],'payable':false,'type':'function'},{'constant':true,'inputs':[],'name':'schema_addr','outputs':[{'name':'','type':'address'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'}],'name':'unregisterWorkWithRegistry','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'x','type':'address'}],'name':'addressToBytes','outputs':[{'name':'b','type':'bytes'}],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_registry','type':'address'},{'name':'_license','type':'address'}],'name':'unregisterLicense','outputs':[],'payable':false,'type':'function'},{'constant':false,'inputs':[{'name':'_key','type':'bytes32'},{'name':'_value','type':'string'},{'name':'_standard','type':'bool'}],'name':'setValue','outputs':[],'payable':false,'type':'function'},{'inputs':[{'name':'_schema_addr','type':'address'}],'type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'key','type':'bytes32'},{'indexed':false,'name':'value','type':'string'}],'name':'StandardDataChanged','type':'event'},{'anonymous':false,'inputs':[{'indexed':true,'name':'key','type':'bytes32'},{'indexed':false,'name':'value','type':'string'}],'name':'NonStandardDataChanged','type':'event'}]";

        protected readonly Web3 Web3;

        protected Contract Contract;

        public WorkServiceBase(Web3 web3)
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

        public Function GetRegisterLicenseAndAttachToThisWorkFunction()
        {
            return Contract.GetFunction("registerLicenseAndAttachToThisWork");
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

        public Function GetRegisterWorkWithRegistryFunction()
        {
            return Contract.GetFunction("registerWorkWithRegistry");
        }

        public Function GetChangeControllerFunction()
        {
            return Contract.GetFunction("changeController");
        }

        public Function GetUnregisterWorkWithRegistryFunction()
        {
            return Contract.GetFunction("unregisterWorkWithRegistry");
        }

        public Function GetUnregisterLicenseFunction()
        {
            return Contract.GetFunction("unregisterLicense");
        }

        public Function GetSha3OfValueAtKeyFunction()
        {
            return Contract.GetFunction("sha3OfValueAtKey");
        }
    }
}