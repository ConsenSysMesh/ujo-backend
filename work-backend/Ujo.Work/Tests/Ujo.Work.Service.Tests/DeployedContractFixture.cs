using System.Threading.Tasks;
using Nethereum.IntegrationTesting;

namespace Ujo.Work.Service.Tests
{
    public class DeployedContractFixture : GethServerFixture
    {
        public string ContractAddress { get; set; }
        private TransactionHelpers txHelper = new TransactionHelpers();


        public DeployedContractFixture()
        {

        }

        public override void Init()
        {
            base.Init();
            this.ContractAddress = DeployContract().Result;
        }

        public async Task<string> DeployContract()
        {
            //TODO Deploy contract Standard first
            var standardContract = await txHelper.DeployContract(this.GetWeb3(),
                DefaultSettings.AddressFrom, DefaultSettings.Password, DefaultSettings.StandardSchemaContractByteCode);


            return await txHelper.DeployContract(WorkService.ABI, this.GetWeb3(),
                DefaultSettings.AddressFrom, DefaultSettings.Password, DefaultSettings.ContractByteCode, new []{(string)standardContract});
        }
    }
}