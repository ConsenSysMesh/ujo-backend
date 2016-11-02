using System;
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
    public abstract class WorkIntegration
    {
        protected string Account { get; set; } 
        protected string PublicNode { get; set; } 

        protected string ByteCode { get; set; } = DefaultSettings.ContractByteCode;

        protected string WorkStandardSchemaByteCode { get; set; } = DefaultSettings.StandardSchemaContractByteCode;

        protected string WorkStandardSchemaAddress { get; set; }

        protected HexBigInteger defaultGas = new HexBigInteger(2400000);

        public abstract Task<string> DeployContractAsync();
        public abstract Task<Web3> CreateNewWeb3Instance();

        public async Task ShouldUpdateWorkFileInContract(string address, string fileHash)
        {
            var web3 = await CreateNewWeb3Instance();
            var workService = new WorkService(web3, address);
            await workService.SetAttributeAsync(Account, WorkSchema.audio, fileHash, true, defaultGas);

        }

        public async Task<string> DeployContractToModernAllFieldsAsync(string workHash, string workTitle, string coverImageHash,
           string artistNameAddress, string genre)
        {
            var web3 = await CreateNewWeb3Instance();
            var contract = await DeployContractAsync();
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

            var tx1 = await workService.BulkSetValueAsync(Account, keys, values, true, defaultGas);
            var tx2 = await workService.BulkSetValueAsync(Account, keys2, values2, true, defaultGas);


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

            await workService.BulkSetValueAsync(Account, keys, values, true, defaultGas);

            return contract;
        }
    }

    public class WorkPublicNodeIntegrationTests: WorkIntegration
    {
        public WorkPublicNodeIntegrationTests()
        {
            this.Account = "0x471c1C9cDFFAaDcaC29Fe4F5c50a556106E23dbe";
            this.WorkStandardSchemaAddress = "0xd36f5f247482c99fc604f5feb70d0e1e13f696ba";
            this.PublicNode = "https://consensysnet.infura.io:8545";
        }

        protected string PrivateKey { get; set; } = "6994c19e4712d5a3b236a798ca78b681831ce70b8a0bb54d75483446a0842a52";
    

        [Fact]
        public override async Task<string> DeployContractAsync()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            var contract = await
                 transactionHelper.DeployContract(PrivateKey, WorkService.ABI, web3, Account,
                     ByteCode, new[] { WorkStandardSchemaAddress });
            Debug.WriteLine("Contract created: " + contract);
            return contract;
        }

        [Fact]
        public virtual async Task ShouldDeployStandardSchema()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            string contract = await
                 transactionHelper.DeployContract(PrivateKey, web3, Account,
                     WorkStandardSchemaByteCode);
        }


        public override async Task<Web3> CreateNewWeb3Instance()
        {
            var web3 = new Web3(PublicNode);
            web3.Client.OverridingRequestInterceptor = new Nethereum.Web3.Interceptors.TransactionRequestToOfflineSignedTransactionInterceptor(Account, PrivateKey, web3);
            return web3;
        }
    }

    public class WorkMordenTests:WorkIntegration
    {
        public WorkMordenTests()
        {
            this.Account = "0xdc4f716883423facd4e13763391ea2d9bcb28022";
            this.WorkStandardSchemaAddress = "0xe168aa45fb2c84c0486305db6c2d6fdf01a66754";
        }
        
        string password = "password";

        public override async Task<Web3> CreateNewWeb3Instance()
        {
            var web3 = new Web3();
            await web3.Personal.UnlockAccount.SendRequestAsync(Account, password, new HexBigInteger(6000));
            return web3;
        }

        [Fact]
        public async Task ShouldDeployContractToMordenAsync()
        {
            var workService = new WorkService(new Web3(), "0xdF597079182391EaFB478412F2352CfAfc7E29A3");
            await workService.SetAttributeAsync(Account, WorkSchema.name, "Hello", true, defaultGas);
            await workService.SetAttributeAsync(Account, WorkSchema.audio, "MP3HASH", true, defaultGas);
            await workService.SetAttributeAsync(Account, WorkSchema.image, "QmZTEAwF3f1oidQtZNKqM2VVK79xDdMNDnSDjC632AzZnU", true, defaultGas);
        }

        [Fact]
        public async Task ShouldChangeDataInContract()
        {
            var web3 = await CreateNewWeb3Instance();
            var workService = new WorkService(web3, "0xdF597079182391EaFB478412F2352CfAfc7E29A3");
            await workService.SetAttributeAsync(Account, WorkSchema.image, "QmY6CYcPbpmvz2R2v5Jfv5DLgTjyyN6HmWPKsBTnG2Ajv7", true, defaultGas);

        }

        [Fact]
        public virtual async Task ShouldDeployStandardSchema()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            string contract = await
                 transactionHelper.DeployContract(web3, Account, password,
                     WorkStandardSchemaByteCode, false);
        }


        public override async Task<string> DeployContractAsync()
        {
            var transactionHelper = new TransactionHelpers();
            var web3 = await CreateNewWeb3Instance();
            var contract = await
                 transactionHelper.DeployContract(WorkService.ABI, web3, Account, password,
                     ByteCode, new[] { WorkStandardSchemaAddress });
            Debug.WriteLine("Contract created: " + contract);
            return contract;
        }
    }
}