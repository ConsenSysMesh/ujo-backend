using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Newtonsoft.Json;
using Ujo.Work.Model;
using Ujo.Work.Service.Tests.xUnitPriority;
using Ujo.Work.Services.Ethereum;
using Xunit;

namespace Ujo.Work.Service.Tests
{
    [TestCaseOrderer("Ujo.Work.Service.Tests.xUnitPriority.PriorityOrderer", "Ujo.Work.Service.Tests")]
    public class WorkServiceTests : IClassFixture<DeployedContractFixture>
    {
        private readonly DeployedContractFixture _deployedContractFixture;
        private static TransactionHelpers _txHelper = new TransactionHelpers();
        private static HexBigInteger _defaultGas = new HexBigInteger(4000000);

        public WorkServiceTests(DeployedContractFixture deployedContractFixture)
        {
            this._deployedContractFixture = deployedContractFixture;
        }


        [Fact, TestPriority(1)]
        public async Task Should_1_SetAttributes()
        {
            var web3 = _deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var dataChangedEvent = workService.GetStandardDataChangedEvent();
            var filter = await dataChangedEvent.CreateFilterAsync();
            var schemaAddress = await workService.GetSchemaAddress();
            var receipt = await _txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.Name, "Hello", true, _defaultGas));

            var eventLogs = await dataChangedEvent.GetFilterChanges<DataChangedEvent>(filter);
            Assert.Equal(WorkSchema.Name.ToString(), eventLogs[0].Event.Key);
            Assert.Equal("Hello", eventLogs[0].Event.Value);

            var value = await workService.GetWorkAttributeAsyncCall(WorkSchema.Name);
            Assert.Equal("Hello", value);
        }

        [Fact, TestPriority(2)]
        public async Task Should_GetALongKeyAttributes()
        {
            var web3 = _deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var dataChangedEvent = workService.GetStandardDataChangedEvent();
            var filter = await dataChangedEvent.CreateFilterAsync();
            var schemaAddress = await workService.GetSchemaAddress();
            var receipt = await _txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.ContributingArtistRole10, "Vocals", true, _defaultGas));

            var eventLogs = await dataChangedEvent.GetFilterChanges<DataChangedEvent>(filter);
            Assert.Equal(WorkSchema.ContributingArtistRole10.ToString(), eventLogs[0].Event.Key);
            Assert.Equal("Vocals", eventLogs[0].Event.Value);

            var value = await workService.GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole10);
            Assert.Equal("Vocals", value);
        }

        [Fact]
        public async Task Should_CheckIfLogIsDataChanged()
        {
            var web3 = _deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var worksService = new WorksService(web3);
            var dataChangedEvent = workService.GetStandardDataChangedEvent();
             var receipt = await _txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
              () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.Name, "Hello", true, _defaultGas));

            Assert.True(worksService.IsStandardDataChangeLog(receipt.Logs[0]));
       
            var filterLog = JsonConvert.DeserializeObject<FilterLog>(receipt.Logs[0].ToString());
            var dataChanged = Event.DecodeAllEvents<DataChangedEvent>(new[] {filterLog});

            Assert.Equal(WorkSchema.Name.ToString(), dataChanged[0].Event.Key);
            Assert.Equal("Hello", dataChanged[0].Event.Value);

        }

        [Fact]
        public async Task Should_GetDataChangedLogsForBlockNumberRange()
        {
            var web3 = _deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var worksService = new WorksService(web3);
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

            var receipt = await _txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
              () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.Name, "Hello", true, _defaultGas));

            var logs = await worksService.GetDataChangedEventsAsync((ulong) blockNumber.Value);
            Assert.True(logs.Count == 0);
            var newBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            logs = await worksService.GetDataChangedEventsAsync((ulong) blockNumber.Value, (ulong) newBlockNumber.Value);
            Assert.True(logs.Count == 1);
        }

        [Fact]
        public async Task Should_GetWorkObjectModel()
        {
            var web3 = _deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);

            await _txHelper.SendAndMineTransactionsAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                  () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.Name, "Hello", true, _defaultGas),
                  () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.Audio, "WORKHASH", true, _defaultGas)
                   );

            var work = await workService.GetWorkAsync();
            Assert.Equal("Hello", work.Name);
            Assert.Equal("WORKHASH", work.WorkFileIpfsHash);
            Assert.Equal(string.Empty, work.CoverImageIpfsHash);
        }

        [Fact]
        public async Task Should_SetInBulk()
        {
            var web3 = _deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var keys = new[] { WorkSchema.Name, WorkSchema.Audio};
            var values = "Hello|WORKHASH";

            var receipts = await _txHelper.SendAndMineTransactionsAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                  () => workService.BulkSetValueAsync(DefaultSettings.AddressFrom, keys ,  values, true, _defaultGas)
                   );

            var work = await workService.GetWorkAsync();
            Assert.Equal("Hello", work.Name);
            Assert.Equal("WORKHASH", work.WorkFileIpfsHash);
            Assert.Equal(string.Empty, work.CoverImageIpfsHash);

        }

        [Fact]
        public async Task Should_SetInBulkAllFields()
        {
            var web3 = _deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);

            var keys = new[] {  WorkSchema.Name,
                                WorkSchema.Image,
                                WorkSchema.Audio,
                                WorkSchema.Genre,
                                WorkSchema.Keywords,
                                WorkSchema.ByArtist,
                                WorkSchema.FeaturedArtist1,
                                WorkSchema.FeaturedArtist2,
                                WorkSchema.FeaturedArtist3,
                                WorkSchema.FeaturedArtist4,
                                WorkSchema.FeaturedArtist5,
                                WorkSchema.FeaturedArtist6,
                                WorkSchema.FeaturedArtist7,
                                WorkSchema.FeaturedArtist8,
                                WorkSchema.FeaturedArtist9,
                                WorkSchema.FeaturedArtist10,
                                WorkSchema.FeaturedArtistRole1,
                                WorkSchema.FeaturedArtistRole2,
                                WorkSchema.FeaturedArtistRole3,
                                WorkSchema.FeaturedArtistRole4,
                                WorkSchema.FeaturedArtistRole5,
                                WorkSchema.FeaturedArtistRole6,
                                WorkSchema.FeaturedArtistRole7,
                                WorkSchema.FeaturedArtistRole8,
                                WorkSchema.FeaturedArtistRole9,
                                WorkSchema.FeaturedArtistRole10,
                                WorkSchema.ContributingArtist1,
                                WorkSchema.ContributingArtist2,
                                WorkSchema.ContributingArtist3,
                                WorkSchema.ContributingArtist4,
                                WorkSchema.ContributingArtist5,
                                WorkSchema.ContributingArtist6,
                                WorkSchema.ContributingArtist7,
                                WorkSchema.ContributingArtist8,
                                WorkSchema.ContributingArtist9,
                                WorkSchema.ContributingArtist10,
                                WorkSchema.ContributingArtistRole1,
                                WorkSchema.ContributingArtistRole2,
                                WorkSchema.ContributingArtistRole3,
                                WorkSchema.ContributingArtistRole4,
                                WorkSchema.ContributingArtistRole5,
                                WorkSchema.ContributingArtistRole6,
                                WorkSchema.ContributingArtistRole7,
                                WorkSchema.ContributingArtistRole8,
                                WorkSchema.ContributingArtistRole9,
                                WorkSchema.ContributingArtistRole10,
                                WorkSchema.PerformingArtist1,
                                WorkSchema.PerformingArtist2,
                                WorkSchema.PerformingArtist3,
                                WorkSchema.PerformingArtist4,
                                WorkSchema.PerformingArtist5,
                                WorkSchema.PerformingArtist6,
                                WorkSchema.PerformingArtist7,
                                WorkSchema.PerformingArtist8,
                                WorkSchema.PerformingArtist9,
                                WorkSchema.PerformingArtist10,
                                WorkSchema.PerformingArtistRole1,
                                WorkSchema.PerformingArtistRole2,
                                WorkSchema.PerformingArtistRole3,
                                WorkSchema.PerformingArtistRole4,
                                WorkSchema.PerformingArtistRole5,
                                WorkSchema.PerformingArtistRole6,
                                WorkSchema.PerformingArtistRole7,
                                WorkSchema.PerformingArtistRole8,
                                WorkSchema.PerformingArtistRole9,
                                WorkSchema.PerformingArtistRole10,
                                WorkSchema.Label,
                                WorkSchema.Description,
                                WorkSchema.Publisher,
                                WorkSchema.HasPartOf,
                                WorkSchema.IsPartOf,
                                WorkSchema.IsFamilyFriendly,
                                WorkSchema.License,
                                WorkSchema.IswcCode
            };
                var values = "";
                foreach (var key in keys)
                {
                    values = values + key.ToString() + "|";
                }
           

                var receipts = await _txHelper.SendAndMineTransactionsAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                      () => workService.BulkSetValueAsync(DefaultSettings.AddressFrom, keys, values, true, _defaultGas)
                       );

                var work = await workService.GetWorkAsync();
                Assert.Equal("name", work.Name);
                Assert.Equal("image", work.Image);
                Assert.Equal("audio", work.Audio);
                Assert.Equal("genre", work.Genre);
                Assert.Equal("keywords", work.Keywords);
                Assert.Equal("byArtist", work.ByArtistName);
                Assert.Equal("featuredArtist1", work.FeaturedArtists[0].Name);
                Assert.Equal("featuredArtist2", work.FeaturedArtists[1].Name);
                Assert.Equal("featuredArtist3", work.FeaturedArtists[2].Name);
                Assert.Equal("featuredArtist4", work.FeaturedArtists[3].Name);
                Assert.Equal("featuredArtist5", work.FeaturedArtists[4].Name);
                Assert.Equal("featuredArtist6", work.FeaturedArtists[5].Name);
                Assert.Equal("featuredArtist7", work.FeaturedArtists[6].Name);
                Assert.Equal("featuredArtist8", work.FeaturedArtists[7].Name);
                Assert.Equal("featuredArtist9", work.FeaturedArtists[8].Name);
                Assert.Equal("featuredArtist10",work.FeaturedArtists[9].Name);
                Assert.Equal("featuredArtistRole1", work.FeaturedArtists[0].Role);
                Assert.Equal("featuredArtistRole2", work.FeaturedArtists[1].Role);
                Assert.Equal("featuredArtistRole3", work.FeaturedArtists[2].Role);
                Assert.Equal("featuredArtistRole4", work.FeaturedArtists[3].Role);
                Assert.Equal("featuredArtistRole5", work.FeaturedArtists[4].Role);
                Assert.Equal("featuredArtistRole6", work.FeaturedArtists[5].Role);
                Assert.Equal("featuredArtistRole7", work.FeaturedArtists[6].Role);
                Assert.Equal("featuredArtistRole8", work.FeaturedArtists[7].Role);
                Assert.Equal("featuredArtistRole9", work.FeaturedArtists[8].Role);
                Assert.Equal("featuredArtistRole10",work.FeaturedArtists[9].Role);
                Assert.Equal("contributingArtist1", work.ContributingArtists[0].Name);
                Assert.Equal("contributingArtist2", work.ContributingArtists[1].Name);
                Assert.Equal("contributingArtist3", work.ContributingArtists[2].Name);
                Assert.Equal("contributingArtist4", work.ContributingArtists[3].Name);
                Assert.Equal("contributingArtist5", work.ContributingArtists[4].Name);
                Assert.Equal("contributingArtist6", work.ContributingArtists[5].Name);
                Assert.Equal("contributingArtist7", work.ContributingArtists[6].Name);
                Assert.Equal("contributingArtist8", work.ContributingArtists[7].Name);
                Assert.Equal("contributingArtist9", work.ContributingArtists[8].Name);
                Assert.Equal("contributingArtist10",work.ContributingArtists[9].Name);
                Assert.Equal("contributingArtistRole1", work.ContributingArtists[0].Role);
                Assert.Equal("contributingArtistRole2", work.ContributingArtists[1].Role);
                Assert.Equal("contributingArtistRole3", work.ContributingArtists[2].Role);
                Assert.Equal("contributingArtistRole4", work.ContributingArtists[3].Role);
                Assert.Equal("contributingArtistRole5", work.ContributingArtists[4].Role);
                Assert.Equal("contributingArtistRole6", work.ContributingArtists[5].Role);
                Assert.Equal("contributingArtistRole7", work.ContributingArtists[6].Role);
                Assert.Equal("contributingArtistRole8", work.ContributingArtists[7].Role);
                Assert.Equal("contributingArtistRole9", work.ContributingArtists[8].Role);
                Assert.Equal("contributingArtistRole10",work.ContributingArtists[9].Role);
                Assert.Equal("performingArtist1", work.PerformingArtists[0].Name);
                Assert.Equal("performingArtist2", work.PerformingArtists[1].Name);
                Assert.Equal("performingArtist3", work.PerformingArtists[2].Name);
                Assert.Equal("performingArtist4", work.PerformingArtists[3].Name);
                Assert.Equal("performingArtist5", work.PerformingArtists[4].Name);
                Assert.Equal("performingArtist6", work.PerformingArtists[5].Name);
                Assert.Equal("performingArtist7", work.PerformingArtists[6].Name);
                Assert.Equal("performingArtist8", work.PerformingArtists[7].Name);
                Assert.Equal("performingArtist9", work.PerformingArtists[8].Name);
                Assert.Equal("performingArtist10",work.PerformingArtists[9].Name);
                Assert.Equal("performingArtistRole1", work.PerformingArtists[0].Role);
                Assert.Equal("performingArtistRole2", work.PerformingArtists[1].Role);
                Assert.Equal("performingArtistRole3", work.PerformingArtists[2].Role);
                Assert.Equal("performingArtistRole4", work.PerformingArtists[3].Role);
                Assert.Equal("performingArtistRole5", work.PerformingArtists[4].Role);
                Assert.Equal("performingArtistRole6", work.PerformingArtists[5].Role);
                Assert.Equal("performingArtistRole7", work.PerformingArtists[6].Role);
                Assert.Equal("performingArtistRole8", work.PerformingArtists[7].Role);
                Assert.Equal("performingArtistRole9", work.PerformingArtists[8].Role);
                Assert.Equal("performingArtistRole10",work.PerformingArtists[9].Role);
                Assert.Equal("label", work.Label);
                Assert.Equal("description", work.Description);
                Assert.Equal("publisher", work.Publisher);
                Assert.Equal(false, work.HasPartOf);
                Assert.Equal(false, work.IsPartOf);
                Assert.Equal("isFamilyFriendly", work.IsFamilyFriendly);
                Assert.Equal("license", work.License);
                Assert.Equal("iswcCode", work.IswcCode);

        }

        private WorkService GetWorkService(Web3 web3)
        {
            var contractAddress = _deployedContractFixture.ContractAddress;
            //Given a contract is deployed
            Assert.False(string.IsNullOrEmpty(contractAddress));
            return new WorkService(web3, contractAddress);
        }
        
    }
}