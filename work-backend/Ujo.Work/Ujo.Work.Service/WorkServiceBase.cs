using Nethereum.Web3;

namespace Ujo.Work.Service
{
    public class WorkServiceBase
    {
        protected readonly Web3 web3;

        protected string abi =
            @"[{""constant"":true,""inputs"":[{""name"":"""",""type"":""uint256""}],""name"":""store"",""outputs"":[{""name"":"""",""type"":""string""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""key"",""type"":""uint256""},{""name"":""value"",""type"":""string""}],""name"":""setAttribute"",""outputs"":[{""name"":""success"",""type"":""bool""}],""type"":""function""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""key"",""type"":""uint256""},{""indexed"":false,""name"":""value"",""type"":""string""}],""name"":""DataChanged"",""type"":""event""}]";

        protected Contract contract;

        public WorkServiceBase(Web3 web3)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(abi, null);
        }

        public Event GetDataChangedEvent()
        {
            return contract.GetEvent("DataChanged");
        }

        public Function GetSetAttributeFunction()
        {
            return contract.GetFunction("setAttribute");
        }

        public Function GetGetAttributeFunction()
        {
            return contract.GetFunction("store");
        }
    }
}