using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.Work.Service;
using Ujo.Work.Service.Tests;
using Xunit;

namespace Ujo.Work.Service.Tests
{
    public class WorkMordenTests
    {
        
        string userName = "0xbb7e97e5670d7475437943a1b314e661d7a9fa2a";
        string password = "password";
        private static HexBigInteger defaultGas = new HexBigInteger(900000);

        [Fact]
        public async Task ShouldDeployContractToMordenAsync()
        {
            var transactionHelper = new TransactionHelpers();
            //var address = System.Configuration.ConfigurationManager.
            //string contract = await
            //     transactionHelper.DeployContract(new Web3(), userName, password,
            //         DefaultSettings.ContractByteCode);
            var workService = new WorkService(new Web3(), "0xdF597079182391EaFB478412F2352CfAfc7E29A3");
            await workService.SetAttributeAsync(userName, StandardSchema.name, "Hello", true, defaultGas);
            await workService.SetAttributeAsync(userName, StandardSchema.audio, "MP3HASH", true, defaultGas);
            await workService.SetAttributeAsync(userName, StandardSchema.image, "QmZTEAwF3f1oidQtZNKqM2VVK79xDdMNDnSDjC632AzZnU", true, defaultGas);

        }


        public async Task<string> DeployContractToModernAsync(string workHash, string workTitle, string coverImageHash,
            string artistName)
        {
            var transactionHelper = new TransactionHelpers();
            string contract = await
                 transactionHelper.DeployContract(new Web3(), userName, password,
                     DefaultSettings.ContractByteCode, false);
            var workService = new WorkService(new Web3(), contract);
            await workService.SetAttributeAsync(userName, StandardSchema.name, workTitle, true, defaultGas);
            await workService.SetAttributeAsync(userName, StandardSchema.audio, workHash, true, defaultGas);
            await workService.SetAttributeAsync(userName, StandardSchema.image, coverImageHash, true, defaultGas);
            return contract;
        }


        [Fact]
        public async Task ShouldChangeDataInContract()
        {
            var web3 = new Web3();
            var workService = new WorkService(web3, "0xdF597079182391EaFB478412F2352CfAfc7E29A3");
            await web3.Personal.UnlockAccount.SendRequestAsync(userName, password, new HexBigInteger(6000));
            await workService.SetAttributeAsync(userName, StandardSchema.image, "QmY6CYcPbpmvz2R2v5Jfv5DLgTjyyN6HmWPKsBTnG2Ajv7", true, defaultGas);
            
        }
    }
}