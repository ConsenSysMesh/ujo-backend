using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexTypes;

namespace Ujo.ContractRegistry.Tests
{
    [TestClass]
    public class ContractRegistryTest
    {
        private static string contractAddress;
        private static ContractRegistryGethTestRunner gethRunner;
        private static string contractByteCode = "0x60606040526106f2806100126000396000f360606040523615610074576000357c0100000000000000000000000000000000000000000000000000000000900480632ec2c246146100765780634420e4861461008e578063469e9067146100a6578063788cadf914610113578063b5d1990d14610155578063c5ea3c651461017857610074565b005b61008c6004808035906020019091905050610440565b005b6100a4600480803590602001909190505061019b565b005b6100bc6004808035906020019091905050610641565b604051808573ffffffffffffffffffffffffffffffffffffffff1681526020018473ffffffffffffffffffffffffffffffffffffffff16815260200183815260200182815260200194505050505060405180910390f35b61012960048080359060200190919050506106ba565b604051808273ffffffffffffffffffffffffffffffffffffffff16815260200191505060405180910390f35b610162600480505061062f565b6040518082815260200191505060405180910390f35b6101856004805050610638565b6040518082815260200191505060405180910390f35b6000600260005060008373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060020160005054141561043c5742600260005060008373ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000506002016000508190555033600260005060008373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060010160006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908302179055506001600081815054809291906001019190505550600160005054600260005060008373ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050600301600050819055508060036000506000600160005054815260200190815260200160002060006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908302179055506000600081815054809291906001019190505550600260005060008273ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060010160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16600260005060008373ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050600301600050548273ffffffffffffffffffffffffffffffffffffffff167fa1559e2788bce2e1be76d17f52a99fe897d4f30c22381249c19f197b567365e9600260005060008673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050600201600050546040518082815260200191505060405180910390a45b5b50565b60003373ffffffffffffffffffffffffffffffffffffffff16600260005060008473ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060005060010160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16141561062a57600260005060008373ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600050600301600050549050600260005060008373ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060006000820160006101000a81549073ffffffffffffffffffffffffffffffffffffffff02191690556001820160006101000a81549073ffffffffffffffffffffffffffffffffffffffff02191690556002820160005060009055600382016000506000905550506000600081815054809291906001900391905055506003600050600082815260200190815260200160002060006101000a81549073ffffffffffffffffffffffffffffffffffffffff0219169055808273ffffffffffffffffffffffffffffffffffffffff167f15f7469572dc44ee54c08cc4adf4e1031d7b6254626e8894015195a560039d0960405180905060405180910390a35b5b5050565b60006000505481565b60016000505481565b60026000506020528060005260406000206000915090508060000160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16908060010160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16908060020160005054908060030160005054905084565b600360005060205280600052604060002060009150909054906101000a900473ffffffffffffffffffffffffffffffffffffffff168156";
        private static string addressFrom = "0x12890d2cce102216644c59dae5baed380d84830c";
        private static string password = "password";
        private static TransactionHelpers txHelper = new TransactionHelpers();
        private static HexBigInteger defaultGas = new HexBigInteger(900000);

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            gethRunner = new ContractRegistryGethTestRunner();
            gethRunner.CleanUp();
            gethRunner.StartGeth();
            contractAddress = DeployContract().Result;
        }

        public static async Task<string> DeployContract()
        {
            return await txHelper.DeployContract(new Web3.Web3(), addressFrom, password, contractByteCode);
        }

        [TestMethod]
        public async Task ShouldRegiterContract()
        {
            var web3 = new Web3.Web3();
            //Given a contract is deployed
            Assert.IsFalse(string.IsNullOrEmpty(contractAddress));

            var contractRegistryService = new ContractRegistryService(web3, contractAddress);
            var registeredEvent =  contractRegistryService.GetRegisteredEvent();
            var filter = await registeredEvent.CreateFilterAsync();

            await txHelper.SendAndMineTransactionAsync(web3, addressFrom, password,
                () => contractRegistryService.RegisterAsync(addressFrom, addressFrom, defaultGas));

            var eventLogs = await registeredEvent.GetFilterChanges<RegisteredEvent>(filter);
        }


        [TestMethod]
        public async Task TestMethod2()
        {
            var web3 = new Web3.Web3();
            //Given a contract is deployed
            Assert.IsFalse(string.IsNullOrEmpty(contractAddress));

            var contractRegistryService = new ContractRegistryService(web3, contractAddress);
        }

        [ClassCleanup]
        public static void TestCleanup()
        {
            gethRunner.StopGeth();
        }
    }
}