using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;

namespace CCC.Contracts
{
    public class ByteCodeMatcher : IByteCodeMatcher
    {
        private readonly Web3 web3;
        private readonly ContractDefinition _contractDefinition;

        public ByteCodeMatcher(Web3 web3, ContractDefinition contractDefinition)
        {
            this.web3 = web3;
            _contractDefinition = contractDefinition;
        }

        public async Task<bool> IsMatchAsync(string address)
        {
            var code = await web3.Eth.GetCode.SendRequestAsync(web3.ToValid20ByteAddress(address));
            return code.IsTheSameHex(_contractDefinition.RuntimeByteCode);
        }
    }
}