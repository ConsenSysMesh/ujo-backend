using System.Diagnostics;
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
        
        string userName = "0xdc4f716883423facd4e13763391ea2d9bcb28022";
        string password = "password";
        private static HexBigInteger defaultGas = new HexBigInteger(4000000);
        public static string StandardSchemaContractAddress = "0xe168aa45fb2c84c0486305db6c2d6fdf01a66754";

        [Fact]
        public async Task ShouldDeployContractToMordenAsync()
        {
            var transactionHelper = new TransactionHelpers();
            //var address = System.Configuration.ConfigurationManager.
            //string contract = await
            //     transactionHelper.DeployContract(new Web3(), userName, password,
            //         DefaultSettings.ContractByteCode);
            var workService = new WorkService(new Web3(), "0xdF597079182391EaFB478412F2352CfAfc7E29A3");
            await workService.SetAttributeAsync(userName, WorkSchema.name, "Hello", true, defaultGas);
            await workService.SetAttributeAsync(userName, WorkSchema.audio, "MP3HASH", true, defaultGas);
            await workService.SetAttributeAsync(userName, WorkSchema.image, "QmZTEAwF3f1oidQtZNKqM2VVK79xDdMNDnSDjC632AzZnU", true, defaultGas);

        }

        [Fact]
        public async Task ShouldDeployStandardSchemaToMordern()
        {
            var transactionHelper = new TransactionHelpers();
            var standardContract = await transactionHelper.DeployContract(new Web3(),
               userName, password, DefaultSettings.StandardSchemaContractByteCode, false);
            Debug.WriteLine(standardContract);
        }

        public async Task<string> DeployContractToModernAsync(string workHash, string workTitle, string coverImageHash,
            string artistName, string genre)
        {
            var transactionHelper = new TransactionHelpers();
            string contract = await
                 transactionHelper.DeployContract(WorkService.ABI, new Web3(), userName, password,
                     DefaultSettings.ContractByteCode, false, new [] {StandardSchemaContractAddress});

            var workService = new WorkService(new Web3(), contract);
            var keys = new[]
            {
                WorkSchema.name, WorkSchema.audio, WorkSchema.genre, WorkSchema.image,
                WorkSchema.creator
            };

            var values = string.Join("|", workTitle, workHash, genre, coverImageHash, artistName);

            await workService.BulkSetValueAsync(userName, keys, values, true, defaultGas);

            return contract;
        }


        public async Task<string> DeployContractToModernAllFieldsAsync(string workHash, string workTitle, string coverImageHash,
            string artistNameAddress, string genre)
        {
            var transactionHelper = new TransactionHelpers();
            string contract = await
                 transactionHelper.DeployContract(WorkService.ABI, new Web3(), userName, password,
                     DefaultSettings.ContractByteCode, false, new[] { (string)StandardSchemaContractAddress });

            var workService = new WorkService(new Web3(), contract);
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
    
            await workService.BulkSetValueAsync(userName, keys, values, true, defaultGas);

            return contract;
        }


        [Fact]
        public async Task ShouldChangeDataInContract()
        {
            var web3 = new Web3();
            var workService = new WorkService(web3, "0xdF597079182391EaFB478412F2352CfAfc7E29A3");
            await web3.Personal.UnlockAccount.SendRequestAsync(userName, password, new HexBigInteger(6000));
            await workService.SetAttributeAsync(userName, WorkSchema.image, "QmY6CYcPbpmvz2R2v5Jfv5DLgTjyyN6HmWPKsBTnG2Ajv7", true, defaultGas);
            
        }

        public async Task ShouldUpdateWorkFileInContract(string address, string fileHash)
        {
            var web3 = new Web3();
            var workService = new WorkService(web3, address);
            await web3.Personal.UnlockAccount.SendRequestAsync(userName, password, new HexBigInteger(6000));
            await workService.SetAttributeAsync(userName, WorkSchema.audio, fileHash, true, defaultGas);

        }
    }
}