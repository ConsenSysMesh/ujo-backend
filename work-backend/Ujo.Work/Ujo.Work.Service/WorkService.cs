using System.Linq;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Ujo.Work.Model;
using System;

namespace Ujo.Work.Service
{
    public class WorkService : WorkServiceBase
    {
        public WorkService(Web3 web3, string address) : base(web3)
        {
           this.contract = web3.Eth.GetContract(ABI, address);
        }

        public async Task<byte[]> Sha3OfValueAtKeyAsyncCall(string key)
        {
            var function = GetSha3OfValueAtKeyFunction();
            return await function.CallAsync<byte[]>(key);
        }

        public Task<string> GetWorkAttributeAsyncCall(WorkSchema key)
        {
            var function = GetStoreFunction();
            return function.CallAsync<string>(key.ToString());
        }

        public Task<string> GetSchemaAddress()
        {
            var function = GetSchemaAddressFunction();
            return function.CallAsync<string>();
        }

        public async Task<string> RegisterWorkWithRegistryAsync(string addressFrom, string registryAddres, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetRegisterWorkWithRegistryFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, registryAddres);
        }

        public async Task<string> BulkSetValueAsync(string addressFrom, WorkSchema[] keys, string vals, bool standard = true, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetBulkSetValueFunction();
            var keysStr = keys.Select(x => x.ToString()).ToArray();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, keysStr, vals, standard);
        }

        public async Task<string> ChangeControllerAsync(string addressFrom, string newController, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetChangeControllerFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, newController);
        }

        public async Task<string> RegisterLicenseAndAttachToThisWorkAsync(string addressFrom, string registry, string license, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetRegisterLicenseAndAttachToThisWorkFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, registry, license);
        }

        public async Task<Model.Work> GetWorkAsync()
        {
            var work = new Model.Work();
            work.Address = this.contract.Address;
            work.WorkFileIpfsHash = await GetWorkAttributeAsyncCall(WorkSchema.audio);
            work.CoverImageIpfsHash = await GetWorkAttributeAsyncCall(WorkSchema.image);
            work.DateCreated = await GetWorkAttributeAsyncCall(WorkSchema.dateCreated);
            work.DateModified = await GetWorkAttributeAsyncCall(WorkSchema.dateModified);
            work.Creator = await GetWorkAttributeAsyncCall(WorkSchema.creator);
            work.Name = await GetWorkAttributeAsyncCall(WorkSchema.name);
            work.Image = await GetWorkAttributeAsyncCall(WorkSchema.image);
            work.Audio = await GetWorkAttributeAsyncCall(WorkSchema.audio);
            work.Genre = await GetWorkAttributeAsyncCall(WorkSchema.genre);
            work.Keywords = await GetWorkAttributeAsyncCall(WorkSchema.keywords);
            work.ByArtist = await GetWorkAttributeAsyncCall(WorkSchema.byArtist);
            work.FeaturedArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist1);
            work.FeaturedArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist2);
            work.FeaturedArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist3);
            work.FeaturedArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist4);
            work.FeaturedArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist5);
            work.FeaturedArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist6);
            work.FeaturedArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist7);
            work.FeaturedArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist8);
            work.FeaturedArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist9);
            work.FeaturedArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist10);
            work.FeaturedArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole1);
            work.FeaturedArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole2);
            work.FeaturedArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole3);
            work.FeaturedArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole4);
            work.FeaturedArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole5);
            work.FeaturedArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole6);
            work.FeaturedArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole7);
            work.FeaturedArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole8);
            work.FeaturedArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole9);
            work.FeaturedArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole10);
            work.ContributingArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist1);
            work.ContributingArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist2);
            work.ContributingArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist3);
            work.ContributingArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist4);
            work.ContributingArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist5);
            work.ContributingArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist6);
            work.ContributingArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist7);
            work.ContributingArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist8);
            work.ContributingArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist9);
            work.ContributingArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist10);
            work.ContributingArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole1);
            work.ContributingArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole2);
            work.ContributingArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole3);
            work.ContributingArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole4);
            work.ContributingArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole5);
            work.ContributingArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole6);
            work.ContributingArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole7);
            work.ContributingArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole8);
            work.ContributingArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole9);
            work.ContributingArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole10);
            work.PerformingArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist1);
            work.PerformingArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist2);
            work.PerformingArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist3);
            work.PerformingArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist4);
            work.PerformingArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist5);
            work.PerformingArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist6);
            work.PerformingArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist7);
            work.PerformingArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist8);
            work.PerformingArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist9);
            work.PerformingArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist10);
            work.PerformingArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole1);
            work.PerformingArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole2);
            work.PerformingArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole3);
            work.PerformingArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole4);
            work.PerformingArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole5);
            work.PerformingArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole6);
            work.PerformingArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole7);
            work.PerformingArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole8);
            work.PerformingArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole9);
            work.PerformingArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole10);
            work.Label = await GetWorkAttributeAsyncCall(WorkSchema.label);
            work.Description = await GetWorkAttributeAsyncCall(WorkSchema.description);
            work.Publisher = await GetWorkAttributeAsyncCall(WorkSchema.publisher);
            work.HasPartOf = TryParseToBolean(await GetWorkAttributeAsyncCall(WorkSchema.hasPartOf), false);
            work.IsPartOf = TryParseToBolean(await GetWorkAttributeAsyncCall(WorkSchema.isPartOf), false);
            work.IsFamilyFriendly = await GetWorkAttributeAsyncCall(WorkSchema.isFamilyFriendly);
            work.License = await GetWorkAttributeAsyncCall(WorkSchema.license);
            work.IswcCode = await GetWorkAttributeAsyncCall(WorkSchema.iswcCode);

            return work;
        }

        private bool TryParseToBolean(string value, bool defaultValue)
        {
            bool boolReturn = false;
            if(Boolean.TryParse(value, out boolReturn))
            {
                return boolReturn;
            }
            return defaultValue;
        }

        public async Task<string> UnregisterWorkWithRegistryAsync(string addressFrom, string _registry, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetUnregisterWorkWithRegistryFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, _registry);
        }

        public async Task<string> UnregisterLicenseAsync(string addressFrom, string _registry, string _license, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetUnregisterLicenseFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, _registry, _license);
        }

        public Task<string> SetAttributeAsync(string addressFrom, WorkSchema key, string value, bool standard = true,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetSetValueFunction();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, key.ToString(), value, true);
        }
    }
}