using System.Diagnostics;
using System.Threading.Tasks;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.Work.Services.Ethereum;
using Xunit;

namespace Ujo.Work.Service.Tests
{
    public class WorkPublicNodeIntegrationTests: WorkIntegration
    {
        public WorkPublicNodeIntegrationTests()
        {
            this.Account = "0x471c1C9cDFFAaDcaC29Fe4F5c50a556106E23dbe";
            this.WorkStandardSchemaAddress = "0xd36f5f247482c99fc604f5feb70d0e1e13f696ba";
            this.PublicNode = "https://consensysnet.infura.io:8545";
        }

        protected string PrivateKey { get; set; } = "6994c19e4712d5a3b236a798ca78b681831ce70b8a0bb54d75483446a0842a52";
    

        [Fact]
        public override async Task<string> DeployContractAsync()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            var contract = await
                transactionHelper.DeployContract(PrivateKey, WorkContractDefinition.ABI, web3, Account,
                    ByteCode, new[] { WorkStandardSchemaAddress });
            Debug.WriteLine("Contract created: " + contract);
            return contract;
        }

        [Fact]
        public virtual async Task ShouldDeployStandardSchema()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            string contract = await
                transactionHelper.DeployContract(PrivateKey, web3, Account,
                    WorkStandardSchemaByteCode);
        }


        public override async Task<Web3> CreateNewWeb3Instance()
        {
            var web3 = new Web3(PublicNode);
            web3.Client.OverridingRequestInterceptor = new Nethereum.Web3.Interceptors.TransactionRequestToOfflineSignedTransactionInterceptor(Account, PrivateKey, web3);
            return web3;
        }
    }
}