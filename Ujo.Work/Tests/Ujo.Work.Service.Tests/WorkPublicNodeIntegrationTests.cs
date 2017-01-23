using System.Diagnostics;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Ujo.Work.Services.Ethereum;
using Xunit;

namespace Ujo.Work.Service.Tests
{
    public class WorkPublicNodeIntegrationTests: WorkIntegration
    {
        public WorkPublicNodeIntegrationTests()
        {
            this.Account = "0x00511a703b28D366239cE5224F8DC7E6301882E8";
            this.WorkStandardSchemaAddress = "0x8254F49E6DfFEeb7A591E752d9EADc0aBbA02F3e";
            //this.PublicNode = "http://localhost:8545";
            this.PublicNode = "https://ropsten.infura.io";
        }

        protected string PrivateKey { get; set; } = "db24be0966a1afff47fc7ad32cae8c76d3229b854a856c0452335d02083bdb8c";

        [Fact]
        public async Task ShouldGetLogs()
        {
            var web3 = await CreateNewWeb3Instance();
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var transactionReceipt =
                await
                    web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(
                        "0x1aa9d4772f48c4cdc5f0fc54971d87571ed71c950e1b9f3b07315512298ddd77");

            var logs = await web3.Eth.Filters.GetLogs.SendRequestAsync(new NewFilterInput()
            {
                FromBlock = new BlockParameter(new HexBigInteger(126995)),
                ToBlock = new BlockParameter(new HexBigInteger(126996))
            });
        }

        [Fact]
        public override async Task<string> DeployContractAsync()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            var contract = await
                transactionHelper.DeployContract(PrivateKey, WorkContractDefinition.ABI, web3, Account,
                    ByteCode, new[] { WorkStandardSchemaAddress });
            return contract;
        }

        [Fact]
        public virtual async Task<string> ShouldDeployStandardSchema()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            var contract = await
                transactionHelper.DeployContract(PrivateKey, web3, Account,
                    WorkStandardSchemaByteCode);
            return contract;
        }

        [Fact]
        public virtual async Task<string> ShouldDeployRegistryFactory()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            var contract = await
                transactionHelper.DeployContract(PrivateKey, web3, Account,
                    WorkFactoryService.BYTE_CODE);
            return contract;
        }


        public override async Task<Web3> CreateNewWeb3Instance()
        {
            var web3 = new Web3(PublicNode);
            web3.Client.OverridingRequestInterceptor = new Nethereum.Web3.Interceptors.TransactionRequestToOfflineSignedTransactionInterceptor(Account, PrivateKey, web3);
            return web3;
        }
    }
}