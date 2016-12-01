using System.Threading.Tasks;
using CCC.Contracts;
using CCC.Contracts.Registry.Services;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.Work.Services.Ethereum;
using Ujo.WorkRegistry.Service.Tests;
using Xunit;

namespace Ujo.ContractRegistry.Tests
{
    public class WorkRegistryPublicNodeIntegration
    {
        string contractAddress = "0xc824f28B9a59F301AaB0C2b2037E8488992225c7";
        string workContractAddress = "0x742b08076d0c21b7d7fc65a4cdbafecd30f815ff";
        string account = "0xe84a30306b0BEEfD34a13A8d20d333078Ba7792e";
        string privateKey = "070bf7ac9a7e9f26b7776f1bb806898df457bb1a1951d28486b9aaf38bc763b0";
        string publicNode = "http://localhost:8545";
       // string publicNode = "https://ropsten.infura.io";
        string byteCode = DefaultSettings.ContractByteCode;
        HexBigInteger defaultGas = new HexBigInteger(2400000);

        [Fact]
        public async Task<string> DeployContractAsync()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = new Web3(publicNode);
            return await transactionHelper.DeployContract(privateKey, web3, account,
                byteCode);        
        }

        [Fact]
        public async Task<string> RegisterDeployedContract(string address)
        {
            var web3 = CreateNewWeb3Instance();
            var contractRegistryService = new RegistryService(web3, contractAddress);
            
            var tx  = await contractRegistryService.RegisterAsync(account,
                address,
                defaultGas);
            var transactionHelper = new TransactionHelpers();
            var receipt = await transactionHelper.GetTransactionReceipt(web3, tx);
            return tx;
        }

        protected Web3 CreateNewWeb3Instance()
        {
            var web3 = new Web3(publicNode);
            web3.Client.OverridingRequestInterceptor = new Nethereum.Web3.Interceptors.TransactionRequestToOfflineSignedTransactionInterceptor(account, privateKey, web3);
            return web3;
        }

        [Fact]
        public async Task ShouldMatchCode()
        {
            var web3 = new Web3(publicNode);
            var contractRegistryService = new ByteCodeMatcher(web3, new WorkContractDefinition());
            Assert.True(await contractRegistryService.IsMatchAsync(workContractAddress));
        }

        public async Task UnRegisterContract(string address)
        {
            var web3 = CreateNewWeb3Instance();
            var contractRegistryService = new RegistryService(web3, contractAddress);
            await contractRegistryService.UnregisterAsync(account,
                address,
                defaultGas);
        }
    }
}