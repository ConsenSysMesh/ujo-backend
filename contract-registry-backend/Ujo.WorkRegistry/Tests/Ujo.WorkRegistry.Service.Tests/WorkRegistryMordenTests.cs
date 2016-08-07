using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.WorkRegistry.Service;
using Ujo.WorkRegistry.Service.Tests;
using Xunit;

namespace Ujo.ContractRegistry.Tests
{
    public class WorkRegistryMordenTests
    {
        string contractAddress = "0x95bc9c2b7078a2e5f0d6fbff4aab18c307b54f04";
        private string blockNumber = "1435835";
        //http://testnet.etherscan.io/tx/0xabda3cecd24250b35c71945103ba670fc2146d7fd8159a78bd3721d30dec2701
        string userName = "0xbb7e97e5670d7475437943a1b314e661d7a9fa2a";
        string password = "password";
        private static HexBigInteger defaultGas = new HexBigInteger(900000);

        //[Fact]
        public async Task DeployContractToMordenAsync()
        {
            var transactionHelper = new TransactionHelpers();
            //var address = System.Configuration.ConfigurationManager.
           string contract =  await
                transactionHelper.DeployContract(new Web3(), userName, password,
                    DefaultSettings.ContractByteCode);

        }

        [Fact]
        public async Task ShouldRegisterAddresses()
        {
            var address1 = "0xb794f5ea0ba39494ce839613fffba74279579268";
            var address2 = "0xe853c56864a2ebe4576a807d26fdc4a0ada51919";
            var address3 = "0xab7c74abc0c4d48d1bdad5dcb26153fc8780f83e";
            var address4 = "0xde0b295669a9fd93d5f28d9ec85e40f4cb697bae";

            var web3 = new Web3();
            var contractRegistryService = new WorkRegistryService(web3, contractAddress);
            await web3.Personal.UnlockAccount.SendRequestAsync(userName, password, new HexBigInteger(60000));
            await contractRegistryService.RegisterAsync(userName,
                address1,
                defaultGas);
            await contractRegistryService.RegisterAsync(userName,
                address2,
                defaultGas);
            await contractRegistryService.RegisterAsync(userName,
                address3, defaultGas 
                );
            await contractRegistryService.RegisterAsync(userName,
                address4,
                defaultGas);


        }
    }
}