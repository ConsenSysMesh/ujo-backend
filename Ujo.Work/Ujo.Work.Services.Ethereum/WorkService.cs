using System;
using System.Linq;
using System.Threading.Tasks;
using CCC.Contracts.StandardData.Services;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Ujo.Work.Model;

namespace Ujo.Work.Services.Ethereum
{
    public class WorkService : StandardDataService
    {  

        public WorkService(Web3 web3, string address) : base(web3)
        {
            this.Abi = WorkContractDefinition.ABI;
            Contract = web3.Eth.GetContract(Abi, address);
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

        public async Task<string> RegisterWorkWithRegistryAsync(string addressFrom, string registryAddres,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetRegisterWorkWithRegistryFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, registryAddres);
        }

        public async Task<string> BulkSetValueAsync(string addressFrom, WorkSchema[] keys, string vals,
            bool standard = true, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetBulkSetValueFunction();
            var keysStr = keys.Select(x => x.ToString()).ToArray();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, keysStr, vals, standard);
        }

        public async Task<string> ChangeControllerAsync(string addressFrom, string newController,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetChangeControllerFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, newController);
        }

        public async Task<string> RegisterLicenseAndAttachToThisWorkAsync(string addressFrom, string registry,
            string license, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetRegisterLicenseAndAttachToThisWorkFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, registry, license);
        }

        private bool IsAddress(string nameOrAddress)
        {
            return nameOrAddress.StartsWith("0x");
        }

        public async Task<Model.Work> GetWorkAsync()
        {
            var work = new Model.Work();
            work.Address = Contract.Address;
            work.WorkFileIpfsHash = await GetWorkAttributeAsyncCall(WorkSchema.Audio);
            work.CoverImageIpfsHash = await GetWorkAttributeAsyncCall(WorkSchema.Image);
            work.DateCreated = await GetWorkAttributeAsyncCall(WorkSchema.DateCreated);
            work.DateModified = await GetWorkAttributeAsyncCall(WorkSchema.DateModified);
            work.Creator = await GetWorkAttributeAsyncCall(WorkSchema.Creator);
            work.Name = await GetWorkAttributeAsyncCall(WorkSchema.Name);
            work.Image = await GetWorkAttributeAsyncCall(WorkSchema.Image);
            work.Audio = await GetWorkAttributeAsyncCall(WorkSchema.Audio);
            work.Genre = await GetWorkAttributeAsyncCall(WorkSchema.Genre);
            work.Keywords = await GetWorkAttributeAsyncCall(WorkSchema.Keywords);

            var byArtist = await GetWorkAttributeAsyncCall(WorkSchema.ByArtist);

            if (IsAddress(byArtist))
                work.ByArtistAddress = byArtist;
            else
                work.ByArtistName = byArtist;

            var featuredArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist1);
            var featuredArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist2);
            var featuredArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist3);
            var featuredArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist4);
            var featuredArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist5);
            var featuredArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist6);
            var featuredArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist7);
            var featuredArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist8);
            var featuredArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist9);
            var featuredArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtist10);
            var featuredArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole1);
            var featuredArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole2);
            var featuredArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole3);
            var featuredArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole4);
            var featuredArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole5);
            var featuredArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole6);
            var featuredArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole7);
            var featuredArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole8);
            var featuredArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole9);
            var featuredArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.FeaturedArtistRole10);

            work.AddFeatureArtist(1, featuredArtist1, featuredArtistRole1);
            work.AddFeatureArtist(2, featuredArtist2, featuredArtistRole2);
            work.AddFeatureArtist(3, featuredArtist3, featuredArtistRole3);
            work.AddFeatureArtist(4, featuredArtist4, featuredArtistRole4);
            work.AddFeatureArtist(5, featuredArtist5, featuredArtistRole5);
            work.AddFeatureArtist(6, featuredArtist6, featuredArtistRole6);
            work.AddFeatureArtist(7, featuredArtist7, featuredArtistRole7);
            work.AddFeatureArtist(8, featuredArtist8, featuredArtistRole8);
            work.AddFeatureArtist(9, featuredArtist9, featuredArtistRole9);
            work.AddFeatureArtist(10, featuredArtist10, featuredArtistRole10);

            var contributingArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist1);
            var contributingArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist2);
            var contributingArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist3);
            var contributingArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist4);
            var contributingArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist5);
            var contributingArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist6);
            var contributingArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist7);
            var contributingArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist8);
            var contributingArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist9);
            var contributingArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtist10);
            var contributingArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole1);
            var contributingArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole2);
            var contributingArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole3);
            var contributingArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole4);
            var contributingArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole5);
            var contributingArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole6);
            var contributingArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole7);
            var contributingArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole8);
            var contributingArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole9);
            var contributingArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.ContributingArtistRole10);

            work.AddContributingArtist(1, contributingArtist1, contributingArtistRole1);
            work.AddContributingArtist(2, contributingArtist2, contributingArtistRole2);
            work.AddContributingArtist(3, contributingArtist3, contributingArtistRole3);
            work.AddContributingArtist(4, contributingArtist4, contributingArtistRole4);
            work.AddContributingArtist(5, contributingArtist5, contributingArtistRole5);
            work.AddContributingArtist(6, contributingArtist6, contributingArtistRole6);
            work.AddContributingArtist(7, contributingArtist7, contributingArtistRole7);
            work.AddContributingArtist(8, contributingArtist8, contributingArtistRole8);
            work.AddContributingArtist(9, contributingArtist9, contributingArtistRole9);
            work.AddContributingArtist(10, contributingArtist10, contributingArtistRole10);

            var performingArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist1);
            var performingArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist2);
            var performingArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist3);
            var performingArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist4);
            var performingArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist5);
            var performingArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist6);
            var performingArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist7);
            var performingArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist8);
            var performingArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist9);
            var performingArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtist10);
            var performingArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole1);
            var performingArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole2);
            var performingArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole3);
            var performingArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole4);
            var performingArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole5);
            var performingArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole6);
            var performingArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole7);
            var performingArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole8);
            var performingArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole9);
            var performingArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.PerformingArtistRole10);

            work.AddPerformingArtist(1, performingArtist1, performingArtistRole1);
            work.AddPerformingArtist(2, performingArtist2, performingArtistRole2);
            work.AddPerformingArtist(3, performingArtist3, performingArtistRole3);
            work.AddPerformingArtist(4, performingArtist4, performingArtistRole4);
            work.AddPerformingArtist(5, performingArtist5, performingArtistRole5);
            work.AddPerformingArtist(6, performingArtist6, performingArtistRole6);
            work.AddPerformingArtist(7, performingArtist7, performingArtistRole7);
            work.AddPerformingArtist(8, performingArtist8, performingArtistRole8);
            work.AddPerformingArtist(9, performingArtist9, performingArtistRole9);
            work.AddPerformingArtist(10, performingArtist10, performingArtistRole10);


            work.Label = await GetWorkAttributeAsyncCall(WorkSchema.Label);
            work.Description = await GetWorkAttributeAsyncCall(WorkSchema.Description);
            work.Publisher = await GetWorkAttributeAsyncCall(WorkSchema.Publisher);
            work.HasPartOf = TryParseToBolean(await GetWorkAttributeAsyncCall(WorkSchema.HasPartOf), false);
            work.IsPartOf = TryParseToBolean(await GetWorkAttributeAsyncCall(WorkSchema.IsPartOf), false);
            work.IsFamilyFriendly = await GetWorkAttributeAsyncCall(WorkSchema.IsFamilyFriendly);
            work.License = await GetWorkAttributeAsyncCall(WorkSchema.License);
            work.IswcCode = await GetWorkAttributeAsyncCall(WorkSchema.IswcCode);

            return work;
        }

        private bool TryParseToBolean(string value, bool defaultValue)
        {
            var boolReturn = false;
            if (bool.TryParse(value, out boolReturn))
                return boolReturn;
            return defaultValue;
        }

        public async Task<string> UnregisterWorkWithRegistryAsync(string addressFrom, string registry,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetUnregisterWorkWithRegistryFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, registry);
        }

        public async Task<string> UnregisterLicenseAsync(string addressFrom, string registry, string license,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetUnregisterLicenseFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, registry, license);
        }

        public Task<string> SetAttributeAsync(string addressFrom, WorkSchema key, string value, bool standard = true,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetSetValueFunction();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, key.ToString(), value, true);
        }

        public Function GetRegisterWorkWithRegistryFunction()
        {
            return Contract.GetFunction("registerWorkWithRegistry");
        }

        public Function GetChangeControllerFunction()
        {
            return Contract.GetFunction("changeController");
        }

        public Function GetUnregisterWorkWithRegistryFunction()
        {
            return Contract.GetFunction("unregisterWorkWithRegistry");
        }

        public Function GetUnregisterLicenseFunction()
        {
            return Contract.GetFunction("unregisterLicense");
        }

        public Function GetSha3OfValueAtKeyFunction()
        {
            return Contract.GetFunction("sha3OfValueAtKey");
        }

        public Function GetRegisterLicenseAndAttachToThisWorkFunction()
        {
            return Contract.GetFunction("registerLicenseAndAttachToThisWork");
        }
    }
}