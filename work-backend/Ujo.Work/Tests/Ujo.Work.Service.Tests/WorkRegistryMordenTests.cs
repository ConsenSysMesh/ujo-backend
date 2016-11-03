using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.Work.Model;
using Ujo.Work.Service;
using Ujo.Work.Service.Tests;
using Ujo.Work.Services.Ethereum;
using Xunit;

namespace Ujo.Work.Service.Tests
{
    public class WorkMordenTests:WorkIntegration
    {
        public WorkMordenTests()
        {
            this.Account = "0xdc4f716883423facd4e13763391ea2d9bcb28022";
            this.WorkStandardSchemaAddress = "0xe168aa45fb2c84c0486305db6c2d6fdf01a66754";
        }
        
        string _password = "password";

        public override async Task<Web3> CreateNewWeb3Instance()
        {
            var web3 = new Web3();
            await web3.Personal.UnlockAccount.SendRequestAsync(Account, _password, new HexBigInteger(6000));
            return web3;
        }

        [Fact]
        public async Task ShouldDeployContractToMordenAsync()
        {
            var workService = new WorkService(new Web3(), "0xdF597079182391EaFB478412F2352CfAfc7E29A3");
            await workService.SetAttributeAsync(Account, WorkSchema.Name, "Hello", true, DefaultGas);
            await workService.SetAttributeAsync(Account, WorkSchema.Audio, "MP3HASH", true, DefaultGas);
            await workService.SetAttributeAsync(Account, WorkSchema.Image, "QmZTEAwF3f1oidQtZNKqM2VVK79xDdMNDnSDjC632AzZnU", true, DefaultGas);
        }

        [Fact]
        public async Task ShouldChangeDataInContract()
        {
            var web3 = await CreateNewWeb3Instance();
            var workService = new WorkService(web3, "0xdF597079182391EaFB478412F2352CfAfc7E29A3");
            await workService.SetAttributeAsync(Account, WorkSchema.Image, "QmY6CYcPbpmvz2R2v5Jfv5DLgTjyyN6HmWPKsBTnG2Ajv7", true, DefaultGas);

        }

        [Fact]
        public virtual async Task ShouldDeployStandardSchema()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            string contract = await
                 transactionHelper.DeployContract(web3, Account, _password,
                     WorkStandardSchemaByteCode, false);
        }


        public override async Task<string> DeployContractAsync()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            var contract = await
                 transactionHelper.DeployContract(WorkService.Abi, web3, Account, _password,
                     ByteCode, new[] { WorkStandardSchemaAddress });
            Debug.WriteLine("Contract created: " + contract);
            return contract;
        }
    }
}