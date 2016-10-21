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
        private static HexBigInteger defaultGas = new HexBigInteger(4000000);

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
            var schemaAddress = await workService.GetSchemaAddress();
            var receipt = await txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.name, "Hello", true, defaultGas));

            var eventLogs = await dataChangedEvent.GetFilterChanges<DataChangedEvent>(filter);
            Assert.Equal(WorkSchema.name.ToString(), eventLogs[0].Event.Key);
            Assert.Equal("Hello", eventLogs[0].Event.Value);

            var value = await workService.GetWorkAttributeAsyncCall(WorkSchema.name);
            Assert.Equal("Hello", value);
        }

        [Fact, TestPriority(2)]
        public async Task Should_GetALongKeyAttributes()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);
            var dataChangedEvent = workService.GetStandardDataChangedEvent();
            var filter = await dataChangedEvent.CreateFilterAsync();
            var schemaAddress = await workService.GetSchemaAddress();
            var receipt = await txHelper.SendAndMineTransactionAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                () => workService.SetAttributeAsync(DefaultSettings.AddressFrom, WorkSchema.contributingArtistRole10, "Vocals", true, defaultGas));

            var eventLogs = await dataChangedEvent.GetFilterChanges<DataChangedEvent>(filter);
            Assert.Equal(WorkSchema.contributingArtistRole10.ToString(), eventLogs[0].Event.Key);
            Assert.Equal("Vocals", eventLogs[0].Event.Value);

            var value = await workService.GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole10);
            Assert.Equal("Vocals", value);
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

            var receipts = await txHelper.SendAndMineTransactionsAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                  () => workService.BulkSetValueAsync(DefaultSettings.AddressFrom, keys ,  values, true, defaultGas)
                   );

            var work = await workService.GetWorkAsync();
            Assert.Equal("Hello", work.Name);
            Assert.Equal("WORKHASH", work.WorkFileIpfsHash);
            Assert.Equal(string.Empty, work.CoverImageIpfsHash);

        }

        [Fact]
        public async Task Should_SetInBulkAllFields()
        {
            var web3 = deployedContractFixture.GetWeb3();
            var workService = GetWorkService(web3);

            var keys = new[] {  WorkSchema.name,
                                WorkSchema.image,
                                WorkSchema.audio,
                                WorkSchema.genre,
                                WorkSchema.keywords,
                                WorkSchema.byArtist,
                                WorkSchema.featuredArtist1,
                                WorkSchema.featuredArtist2,
                                WorkSchema.featuredArtist3,
                                WorkSchema.featuredArtist4,
                                WorkSchema.featuredArtist5,
                                WorkSchema.featuredArtist6,
                                WorkSchema.featuredArtist7,
                                WorkSchema.featuredArtist8,
                                WorkSchema.featuredArtist9,
                                WorkSchema.featuredArtist10,
                                WorkSchema.featuredArtistRole1,
                                WorkSchema.featuredArtistRole2,
                                WorkSchema.featuredArtistRole3,
                                WorkSchema.featuredArtistRole4,
                                WorkSchema.featuredArtistRole5,
                                WorkSchema.featuredArtistRole6,
                                WorkSchema.featuredArtistRole7,
                                WorkSchema.featuredArtistRole8,
                                WorkSchema.featuredArtistRole9,
                                WorkSchema.featuredArtistRole10,
                                WorkSchema.contributingArtist1,
                                WorkSchema.contributingArtist2,
                                WorkSchema.contributingArtist3,
                                WorkSchema.contributingArtist4,
                                WorkSchema.contributingArtist5,
                                WorkSchema.contributingArtist6,
                                WorkSchema.contributingArtist7,
                                WorkSchema.contributingArtist8,
                                WorkSchema.contributingArtist9,
                                WorkSchema.contributingArtist10,
                                WorkSchema.contributingArtistRole1,
                                WorkSchema.contributingArtistRole2,
                                WorkSchema.contributingArtistRole3,
                                WorkSchema.contributingArtistRole4,
                                WorkSchema.contributingArtistRole5,
                                WorkSchema.contributingArtistRole6,
                                WorkSchema.contributingArtistRole7,
                                WorkSchema.contributingArtistRole8,
                                WorkSchema.contributingArtistRole9,
                                WorkSchema.contributingArtistRole10,
                                WorkSchema.performingArtist1,
                                WorkSchema.performingArtist2,
                                WorkSchema.performingArtist3,
                                WorkSchema.performingArtist4,
                                WorkSchema.performingArtist5,
                                WorkSchema.performingArtist6,
                                WorkSchema.performingArtist7,
                                WorkSchema.performingArtist8,
                                WorkSchema.performingArtist9,
                                WorkSchema.performingArtist10,
                                WorkSchema.performingArtistRole1,
                                WorkSchema.performingArtistRole2,
                                WorkSchema.performingArtistRole3,
                                WorkSchema.performingArtistRole4,
                                WorkSchema.performingArtistRole5,
                                WorkSchema.performingArtistRole6,
                                WorkSchema.performingArtistRole7,
                                WorkSchema.performingArtistRole8,
                                WorkSchema.performingArtistRole9,
                                WorkSchema.performingArtistRole10,
                                WorkSchema.label,
                                WorkSchema.description,
                                WorkSchema.publisher,
                                WorkSchema.hasPartOf,
                                WorkSchema.isPartOf,
                                WorkSchema.isFamilyFriendly,
                                WorkSchema.license,
                                WorkSchema.iswcCode
            };
                var values = "";
                foreach (var key in keys)
                {
                    values = values + key.ToString() + "|";
                }
           

                var receipts = await txHelper.SendAndMineTransactionsAsync(web3, DefaultSettings.AddressFrom, DefaultSettings.Password,
                      () => workService.BulkSetValueAsync(DefaultSettings.AddressFrom, keys, values, true, defaultGas)
                       );

                var work = await workService.GetWorkAsync();
                Assert.Equal("name", work.Name);
                Assert.Equal("image", work.Image);
                Assert.Equal("audio", work.Audio);
                Assert.Equal("genre", work.Genre);
                Assert.Equal("keywords", work.Keywords);
                Assert.Equal("byArtist", work.ByArtist);
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
            var contractAddress = deployedContractFixture.ContractAddress;
            //Given a contract is deployed
            Assert.False(string.IsNullOrEmpty(contractAddress));
            return new WorkService(web3, contractAddress);
        }
        
    }
}