using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.IntegrationTesting;
using Nethereum.Web3;
using Ujo.Work.Model;
using Ujo.Work.Services.Ethereum;

namespace Ujo.Work.Service.Tests
{
    public abstract class WorkIntegration
    {
        protected string Account { get; set; } 
        protected string PublicNode { get; set; } 

        protected string ByteCode { get; set; } = DefaultSettings.ContractByteCode;

        protected string WorkStandardSchemaByteCode { get; set; } = DefaultSettings.StandardSchemaContractByteCode;

        protected string WorkStandardSchemaAddress { get; set; }

        protected HexBigInteger DefaultGas = new HexBigInteger(4000000);

        public abstract Task<string> DeployContractAsync();
        public abstract Task<Web3> CreateNewWeb3Instance();

        public async Task ShouldUpdateWorkFileInContract(string address, string fileHash)
        {
            var web3 = await CreateNewWeb3Instance();
            var workService = new WorkService(web3, address);
            await workService.SetAttributeAsync(Account, WorkSchema.audio, fileHash, true, DefaultGas);

        }

        public async Task<string> CreateAndRegisterWorkWithMockUpFields(string registryAddress, string workHash, string workTitle,
            string coverImageHash,
            string artistNameAddress, string genre)
        {
            string factoryContract = "0xb018cf9b7d5c5ea940d30671cc8257327f8160f2";
            var web3 = await CreateNewWeb3Instance();
            var workFactoryService = new WorkFactoryService(web3, factoryContract);

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
                //WorkSchema.featuredArtist6,
                //WorkSchema.FeaturedArtist7,
                //WorkSchema.FeaturedArtist8,
                //WorkSchema.FeaturedArtist9,
                //WorkSchema.FeaturedArtist10,
                //WorkSchema.FeaturedArtistRole1,
                WorkSchema.featuredArtistRole2,
                WorkSchema.featuredArtistRole3,
                WorkSchema.featuredArtistRole4,
                //WorkSchema.FeaturedArtistRole5,
                //WorkSchema.FeaturedArtistRole6,
                //WorkSchema.FeaturedArtistRole7,
                //WorkSchema.FeaturedArtistRole8,
                //WorkSchema.FeaturedArtistRole9,
                //WorkSchema.FeaturedArtistRole10,
                WorkSchema.contributingArtist1,
                WorkSchema.contributingArtist2,
                WorkSchema.contributingArtist3,
                WorkSchema.contributingArtist4,
                WorkSchema.contributingArtist5,
                //WorkSchema.ContributingArtist6,
                //WorkSchema.ContributingArtist7,
                //WorkSchema.ContributingArtist8,
                //WorkSchema.ContributingArtist9,
                //WorkSchema.ContributingArtist10,
        
                WorkSchema.contributingArtistRole1,
                WorkSchema.contributingArtistRole2,
                WorkSchema.contributingArtistRole3,
                WorkSchema.contributingArtistRole4,
                WorkSchema.contributingArtistRole5,
                //WorkSchema.ContributingArtistRole6,
                //WorkSchema.ContributingArtistRole7,
                //WorkSchema.ContributingArtistRole8,
                //WorkSchema.ContributingArtistRole9,
                //WorkSchema.ContributingArtistRole10,
                //WorkSchema.PerformingArtist1,
                WorkSchema.performingArtist2,
                WorkSchema.performingArtist3,
                WorkSchema.performingArtist4,
                WorkSchema.performingArtist5,
                //WorkSchema.PerformingArtist6,
                //WorkSchema.PerformingArtist7,
                //WorkSchema.PerformingArtist8,
                //WorkSchema.PerformingArtist9,
                //WorkSchema.PerformingArtist10,
                WorkSchema.performingArtistRole1,
                WorkSchema.performingArtistRole2,
                WorkSchema.performingArtistRole3,
                WorkSchema.performingArtistRole4,
                WorkSchema.performingArtistRole5,
                //WorkSchema.PerformingArtistRole6,
                //WorkSchema.PerformingArtistRole7,
                //WorkSchema.PerformingArtistRole8,
                //WorkSchema.PerformingArtistRole9,
                //WorkSchema.PerformingArtistRole10,
                //WorkSchema.Label,
                WorkSchema.description,
                WorkSchema.publisher,
                WorkSchema.hasPartOf,
                WorkSchema.isPartOf,
                WorkSchema.isFamilyFriendly,
                WorkSchema.license,
                WorkSchema.iswcCode
            };
            string values = GetValues(workHash, workTitle, coverImageHash, artistNameAddress, genre, keys);

            var tx1 = await workFactoryService.CreateWorkAsync(Account, keys, values, true, WorkStandardSchemaAddress, registryAddress, DefaultGas);
            var transactionhelper = new TransactionHelpers();
            var receipt = await transactionhelper.GetTransactionReceipt(web3, tx1);
            return tx1;
        }


        public async Task<string> UpdateMetadataWithMockUpFields(string workHash, string workTitle, string coverImageHash,
            string artistNameAddress, string genre, string contract)
        {
            var web3 = await CreateNewWeb3Instance();
            var workService = new WorkService(web3, contract);
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
            };

            var keys2 = new[] {
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
            string values = GetValues(workHash, workTitle, coverImageHash, artistNameAddress, genre, keys);
            string values2 = GetValues(workHash, workTitle, coverImageHash, artistNameAddress, genre, keys2);

            var transactionhelper = new TransactionHelpers();
            var tx1 = await workService.BulkSetValueAsync(Account, keys, values, true, DefaultGas);
            var receipt = await transactionhelper.GetTransactionReceipt(web3, tx1);
            var tx2 = await workService.BulkSetValueAsync(Account, keys2, values2, true, DefaultGas);
            var receipt2 = await transactionhelper.GetTransactionReceipt(web3, tx2);
            //ensure nonces are in order by waiting to be mined.
            return contract;
        }

        protected string GetValues(string workHash, string workTitle, string coverImageHash, string artistNameAddress, string genre, WorkSchema[] keys1)
        {
            var values = "";
            foreach (var key in keys1)
            {

                if (key == WorkSchema.name)
                {
                    values = values + workTitle + "|";
                }
                else if (key == WorkSchema.audio)
                {
                    values = values + workHash + "|";
                }
                else if (key == WorkSchema.genre)
                {
                    values = values + genre + "|";
                }
                else if (key == WorkSchema.image)
                {
                    values = values + coverImageHash + "|";
                }
                else if (key == WorkSchema.byArtist)
                {
                    values = values + artistNameAddress + "|";
                }
                else if (key == WorkSchema.creator)
                {
                    values = values + artistNameAddress + "|";
                }
                else
                {
                    values = values + key.ToString() + "|";
                }
            }

            return values;
        }

        public async Task<string> DeployContractToModernAsync(string workHash, string workTitle, string coverImageHash,
            string artistName, string genre)
        {
            var contract = await DeployContractAsync();
            var web3 = await CreateNewWeb3Instance();

            var workService = new WorkService(web3, contract);
            var keys = new[]
            {
                WorkSchema.name, WorkSchema.audio, WorkSchema.genre, WorkSchema.image,
                WorkSchema.creator
            };

            var values = string.Join("|", workTitle, workHash, genre, coverImageHash, artistName);

            await workService.BulkSetValueAsync(Account, keys, values, true, DefaultGas);

            return contract;
        }
    }
}