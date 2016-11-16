using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
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

        protected HexBigInteger DefaultGas = new HexBigInteger(2400000);

        public abstract Task<string> DeployContractAsync();
        public abstract Task<Web3> CreateNewWeb3Instance();

        public async Task ShouldUpdateWorkFileInContract(string address, string fileHash)
        {
            var web3 = await CreateNewWeb3Instance();
            var workService = new WorkService(web3, address);
            await workService.SetAttributeAsync(Account, WorkSchema.Audio, fileHash, true, DefaultGas);

        }

      
        public async Task<string> UpdateMetadataWithMockUpFields(string workHash, string workTitle, string coverImageHash,
            string artistNameAddress, string genre, string contract)
        {
            var web3 = await CreateNewWeb3Instance();
            var workService = new WorkService(web3, contract);
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
            };

            var keys2 = new[] {
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
            string values = GetValues(workHash, workTitle, coverImageHash, artistNameAddress, genre, keys);
            string values2 = GetValues(workHash, workTitle, coverImageHash, artistNameAddress, genre, keys2);

            var tx1 = await workService.BulkSetValueAsync(Account, keys, values, true, DefaultGas);
            var tx2 = await workService.BulkSetValueAsync(Account, keys2, values2, true, DefaultGas);


            return contract;
        }

        protected string GetValues(string workHash, string workTitle, string coverImageHash, string artistNameAddress, string genre, WorkSchema[] keys1)
        {
            var values = "";
            foreach (var key in keys1)
            {

                if (key == WorkSchema.Name)
                {
                    values = values + workTitle + "|";
                }
                else if (key == WorkSchema.Audio)
                {
                    values = values + workHash + "|";
                }
                else if (key == WorkSchema.Genre)
                {
                    values = values + genre + "|";
                }
                else if (key == WorkSchema.Image)
                {
                    values = values + coverImageHash + "|";
                }
                else if (key == WorkSchema.ByArtist)
                {
                    values = values + artistNameAddress + "|";
                }
                else if (key == WorkSchema.Creator)
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
                WorkSchema.Name, WorkSchema.Audio, WorkSchema.Genre, WorkSchema.Image,
                WorkSchema.Creator
            };

            var values = string.Join("|", workTitle, workHash, genre, coverImageHash, artistName);

            await workService.BulkSetValueAsync(Account, keys, values, true, DefaultGas);

            return contract;
        }
    }
}