using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Newtonsoft.Json;
using Ujo.Work.Service.Tests.xUnitPriority;
using Xunit;

namespace Ujo.Work.Service.Tests
{
    [TestCaseOrderer("Ujo.Work.Service.Tests.xUnitPriority.PriorityOrderer", "Ujo.Work.Service.Tests")]
    public class WorkServiceTests : IClassFixture<DeployedContractFixture>
    {
        private readonly DeployedContractFixture deployedContractFixture;
        private static TransactionHelpers txHelper = new TransactionHelpers();
        private static HexBigInteger defaultGas = new HexBigInteger(900000);

        public WorkServiceTests(DeployedContractFixture deployedContractFixture)
        {
            this.deployedContractFixture = deployedContractFixture;
        }


        [Fact, TestPriority(1)]
        public async Task Should_1_SetAttributes()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var dataChangedEvent = workService.GetStandardDataChangedEvent();
            var filter = await dataChangedEvent.CreateFilterAsync();

            var receipt = await txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.name, "Hello", true, defaultGas));

            var eventLogs = await dataChangedEvent.GetFilterChanges<DataChangedEvent>(filter);
            Assert.Equal(WorkSchema.name.ToString(), eventLogs[0].Event.Key);
            Assert.Equal("Hello", eventLogs[0].Event.Value);

            var value = await workService.GetWorkAttributeAsyncCall(WorkSchema.name);
            Assert.Equal("Hello", value);
        }

        [Fact]
        public async Task Should_CheckIfLogIsDataChanged()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var worksService = new WorksService(web3);
            var dataChangedEvent = workService.GetStandardDataChangedEvent();
             var receipt = await txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
              () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.name, "Hello", true, defaultGas));

            Assert.True(worksService.IsStandardDataChangeLog(receipt.Logs[0]));
       
            var filterLog = JsonConvert.DeserializeObject<FilterLog>(receipt.Logs[0].ToString());
            var dataChanged = Event.DecodeAllEvents<DataChangedEvent>(new[] {filterLog});

            Assert.Equal(WorkSchema.name.ToString(), dataChanged[0].Event.Key);
            Assert.Equal("Hello", dataChanged[0].Event.Value);

        }

        [Fact]
        public async Task Should_GetDataChangedLogsForBlockNumberRange()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var worksService = new WorksService(web3);
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            var receipt = await txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
              () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.name, "Hello", true, defaultGas));

            var logs = await worksService.GetDataChangedEventsAsync((ulong) blockNumber.Value);
            Assert.True(logs.Count == 0);
            var newBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            logs = await worksService.GetDataChangedEventsAsync((ulong) blockNumber.Value, (ulong) newBlockNumber.Value);
            Assert.True(logs.Count == 1);
        }

        [Fact]
        public async Task Should_GetWorkObjectModel()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);

            await txHelper.SendAndMineTransactionsAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                  () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.name, "Hello", true, defaultGas),
                  () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.audio, "WORKHASH", true, defaultGas)
                   );

            var work = await workService.GetWorkAsync();
            Assert.Equal("Hello", work.Name);
            Assert.Equal("WORKHASH", work.WorkFileIpfsHash);
            Assert.Equal(string.Empty, work.CoverImageIpfsHash);

        }

        [Fact]
        public async Task Should_SetInBulk()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var keys = new[] { WorkSchema.name, WorkSchema.audio};
            var values = "Hello|WORKHASH";

            await txHelper.SendAndMineTransactionsAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                  () => workService.BulkSetValueAsync(DefaultSettings.AddressFrom, keys ,  values, true, defaultGas)
                   );

            var work = await workService.GetWorkAsync();
            Assert.Equal("Hello", work.Name);
            Assert.Equal("WORKHASH", work.WorkFileIpfsHash);
            Assert.Equal(string.Empty, work.CoverImageIpfsHash);

        }

        private WorkService GetWorkService(Web3 web3)
        {
            var contractAddress = deployedContractFixture.ContractAddress;
            //Given a contract is deployed
            Assert.False(string.IsNullOrEmpty(contractAddress));
            return new WorkService(web3, contractAddress);
        }
        
    }
}