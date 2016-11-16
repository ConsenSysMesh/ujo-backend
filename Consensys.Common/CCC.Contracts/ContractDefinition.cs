using Nethereum.ABI;
using Nethereum.ABI.FunctionEncoding;

namespace CCC.Contracts
{
    public class ContractDefinition : IContractDefinition
    {
        public string Abi { get; protected set; }
        public string RuntimeByteCode { get; protected set; }
        public string ByteCode { get; protected set; }
        public ContractABI ContractAbi { get; protected set; }

        public ContractDefinition(string abi, string runtimeByteCode, string byteCode)
        {
            Abi = abi;
            RuntimeByteCode = runtimeByteCode;
            ByteCode = byteCode;
            ContractAbi = new ABIDeserialiser().DeserialiseContract(abi);
        }
       
    }
}