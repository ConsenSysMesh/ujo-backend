using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.WorkRegistry.Service;
using Ujo.WorkRegistry.Service.Tests;
using Xunit;

namespace Ujo.ContractRegistry.Tests
{
   

    public class WorkRegistryPublicNodeIntegration
    {
        string contractAddress = "0x56118ee1692bfd21fa70c2edefc6c122246be410";
        string workContractAddress = "0x742b08076d0c21b7d7fc65a4cdbafecd30f815ff";
        string account = "0x471c1C9cDFFAaDcaC29Fe4F5c50a556106E23dbe";
        string privateKey = "6994c19e4712d5a3b236a798ca78b681831ce70b8a0bb54d75483446a0842a52";
        string publicNode = "https://consensysnet.infura.io:8545";
        string byteCode = DefaultSettings.ContractByteCode;
        HexBigInteger defaultGas = new HexBigInteger(2400000);

        [Fact]
        public async Task DeployContractAsync()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = new Web3(publicNode);
            string contract = await
                 transactionHelper.DeployContract(privateKey, web3, account,
                     byteCode);
        }

        [Fact]
        public async Task<string> RegisterDeployedContract(string address)
        {
            var web3 = CreateNewWeb3Instance();
            var contractRegistryService = new WorkRegistryService(web3, contractAddress);
            
            var tx  = await contractRegistryService.RegisterAsync(account,
                address,
                defaultGas);
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
            var contractRegistryService = new WorkRegistryWorkByteCodeMatcher(web3);
            Assert.True(await contractRegistryService.IsMatchAsync(workContractAddress));
        }

        public async Task UnRegisterContract(string address)
        {
            var web3 = CreateNewWeb3Instance();
            var contractRegistryService = new WorkRegistryService(web3, contractAddress);
             await contractRegistryService.UnregisterAsync(account,
                address,
                defaultGas);
        }
    }



    public class WorkRegistryMordenTests
    {
        string contractAddress = "0x95bc9c2b7078a2e5f0d6fbff4aab18c307b54f04";
        private string blockNumber = "1435835";
        //http://testnet.etherscan.io/tx/0xabda3cecd24250b35c71945103ba670fc2146d7fd8159a78bd3721d30dec2701
        string userName = "0xdc4f716883423facd4e13763391ea2d9bcb28022";
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
        public async Task ShouldRegisterDeployedContract()
        {
            var address = "0xdF597079182391EaFB478412F2352CfAfc7E29A3";
            await RegisterDeployedContract(address);
        }

        [Fact]
        public async Task ShouldGetRegisteredUnregisteredFromMordern()
        {
            var web3 = new Web3("https://morden.infura.io:8545");
            var contractRegistryService = new WorkRegistryService(web3, contractAddress);
            var logs = await contractRegistryService.GetRegisteredUnregistered(1488231, 1488279);
        }

        [Fact]
        public async Task ShouldMatchCodeFromMordern()
        {
            var web3 = new Web3("https://morden.infura.io:8545");
            var contractRegistryService = new WorkRegistryWorkByteCodeMatcher(web3);
            Assert.True(await contractRegistryService.IsMatchAsync("0xa0BA11FF2F5608D75c7E0ba01f7fac496cfb7677"));
        }

        public async Task RegisterDeployedContract(string address)
        {
            var web3 = new Web3();
            var contractRegistryService = new WorkRegistryService(web3, contractAddress);
            await web3.Personal.UnlockAccount.SendRequestAsync(userName, password, new HexBigInteger(60000));
            await contractRegistryService.RegisterAsync(userName,
                address,
                defaultGas);
        }

        public async Task UnRegisterContract(string address)
        {
            var web3 = new Web3();
            var contractRegistryService = new WorkRegistryService(web3, contractAddress);
            await web3.Personal.UnlockAccount.SendRequestAsync(userName, password, new HexBigInteger(60000));
            await contractRegistryService.UnregisterAsync(userName,
                address,
                defaultGas);
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