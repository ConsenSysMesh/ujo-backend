using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCC.Contracts.StandardData.Services;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Ujo.Model;
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

        public async Task<Ujo.Model.MusicRecording> GetMusicRecordingAsync()
        {
            var musicRecording = new MusicRecording();
            musicRecording.Address = Contract.Address;
            musicRecording.Audio = await GetWorkAttributeAsyncCall(WorkSchema.audio);
            musicRecording.Image = await GetWorkAttributeAsyncCall(WorkSchema.image);
            musicRecording.DateCreated = await GetWorkAttributeAsyncCall(WorkSchema.dateCreated);
            musicRecording.DateModified = await GetWorkAttributeAsyncCall(WorkSchema.dateModified);
            musicRecording.Creator = await GetWorkAttributeAsyncCall(WorkSchema.creator);
            musicRecording.Name = await GetWorkAttributeAsyncCall(WorkSchema.name);
            musicRecording.Genre = await GetWorkAttributeAsyncCall(WorkSchema.genre);
            musicRecording.Keywords = await GetWorkAttributeAsyncCall(WorkSchema.keywords);
            musicRecording.ByArtistAddress = await GetWorkAttributeAsyncCall(WorkSchema.byArtist);
            musicRecording.Label = await GetWorkAttributeAsyncCall(WorkSchema.label);
            musicRecording.Description = await GetWorkAttributeAsyncCall(WorkSchema.description);
            musicRecording.Publisher = await GetWorkAttributeAsyncCall(WorkSchema.publisher);
            musicRecording.HasPart = TryParseToBolean(await GetWorkAttributeAsyncCall(WorkSchema.hasPartOf), false);
            musicRecording.IsPartOf = TryParseToBolean(await GetWorkAttributeAsyncCall(WorkSchema.isPartOf), false);
            musicRecording.IsFamilyFriendly = await GetWorkAttributeAsyncCall(WorkSchema.isFamilyFriendly);
            musicRecording.License = await GetWorkAttributeAsyncCall(WorkSchema.license);
            musicRecording.IswcCode = await GetWorkAttributeAsyncCall(WorkSchema.iswcCode);
            musicRecording.IsrcCode = await GetWorkAttributeAsyncCall(WorkSchema.isrcCode);
            musicRecording.OtherArtists = await GetAllOtherArtists();
            return musicRecording;
        }

        public async Task<ICollection<CreativeWorkArtist>> GetAllOtherArtists()
        {
            var artistCollection = new List<CreativeWorkArtist>();
            await AddOtherArtist(WorkSchema.contributingArtist1, WorkSchema.contributingArtistRole1, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist2, WorkSchema.contributingArtistRole2, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist3, WorkSchema.contributingArtistRole3, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist4, WorkSchema.contributingArtistRole4, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist5, WorkSchema.contributingArtistRole5, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist6, WorkSchema.contributingArtistRole6, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist7, WorkSchema.contributingArtistRole7, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist8, WorkSchema.contributingArtistRole8, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist9, WorkSchema.contributingArtistRole9, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.contributingArtist10, WorkSchema.contributingArtistRole10, MusicRecording.ContributingArtistContributionTypeKey, artistCollection);


            await AddOtherArtist(WorkSchema.featuredArtist1, WorkSchema.featuredArtistRole1, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist2, WorkSchema.featuredArtistRole2, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist3, WorkSchema.featuredArtistRole3, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist4, WorkSchema.featuredArtistRole4, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist5, WorkSchema.featuredArtistRole5, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist6, WorkSchema.featuredArtistRole6, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist7, WorkSchema.featuredArtistRole7, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist8, WorkSchema.featuredArtistRole8, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist9, WorkSchema.featuredArtistRole9, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.featuredArtist10, WorkSchema.featuredArtistRole10, MusicRecording.FeaturedArtistContributionTypeKey, artistCollection);

            await AddOtherArtist(WorkSchema.performingArtist1, WorkSchema.performingArtistRole1, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist2, WorkSchema.performingArtistRole2, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist3, WorkSchema.performingArtistRole3, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist4, WorkSchema.performingArtistRole4, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist5, WorkSchema.performingArtistRole5, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist6, WorkSchema.performingArtistRole6, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist7, WorkSchema.performingArtistRole7, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist8, WorkSchema.performingArtistRole8, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist9, WorkSchema.performingArtistRole9, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);
            await AddOtherArtist(WorkSchema.performingArtist10, WorkSchema.performingArtistRole10, MusicRecording.PerformingArtistContributionTypeKey, artistCollection);

            return artistCollection;

        }

        public async Task AddOtherArtist(WorkSchema artistAddressSchema, WorkSchema artistRoleSchema,
            string contributionType, ICollection<CreativeWorkArtist> otherArtists)
        {
            var artist = await GetOtherArtist(artistAddressSchema, artistRoleSchema, contributionType);
            if(artist != null) otherArtists.Add(artist);
        }

        public async Task<CreativeWorkArtist> GetOtherArtist(WorkSchema artistAddressSchema, WorkSchema artistRoleSchema,
            string contributionType)
        {
            var artist = await GetWorkAttributeAsyncCall(artistAddressSchema);
            if (string.IsNullOrEmpty(artist)) return null;
            var artistRole = await GetWorkAttributeAsyncCall(artistRoleSchema);
            var creativeWork = new CreativeWorkArtist();
            if (IsAddress(artist))
            {
                creativeWork.ArtistAddres = artist;
            }
            else
            {
                creativeWork.NonRegisteredArtistName = artist;
            }
            creativeWork.ContributionType = contributionType;
            creativeWork.Role = artistRole;
            return creativeWork;
        }

        public async Task<Model.Work> GetWorkAsync()
        {
            var work = new Model.Work();
            work.Address = Contract.Address;
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

            var byArtist = await GetWorkAttributeAsyncCall(WorkSchema.byArtist);

            if (IsAddress(byArtist))
                work.ByArtistAddress = byArtist;
            else
                work.ByArtistName = byArtist;

            var featuredArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist1);
            var featuredArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist2);
            var featuredArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist3);
            var featuredArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist4);
            var featuredArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist5);
            var featuredArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist6);
            var featuredArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist7);
            var featuredArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist8);
            var featuredArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist9);
            var featuredArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtist10);
            var featuredArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole1);
            var featuredArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole2);
            var featuredArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole3);
            var featuredArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole4);
            var featuredArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole5);
            var featuredArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole6);
            var featuredArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole7);
            var featuredArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole8);
            var featuredArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole9);
            var featuredArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.featuredArtistRole10);

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

            var contributingArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist1);
            var contributingArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist2);
            var contributingArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist3);
            var contributingArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist4);
            var contributingArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist5);
            var contributingArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist6);
            var contributingArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist7);
            var contributingArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist8);
            var contributingArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist9);
            var contributingArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtist10);
            var contributingArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole1);
            var contributingArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole2);
            var contributingArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole3);
            var contributingArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole4);
            var contributingArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole5);
            var contributingArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole6);
            var contributingArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole7);
            var contributingArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole8);
            var contributingArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole9);
            var contributingArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.contributingArtistRole10);

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

            var performingArtist1 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist1);
            var performingArtist2 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist2);
            var performingArtist3 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist3);
            var performingArtist4 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist4);
            var performingArtist5 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist5);
            var performingArtist6 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist6);
            var performingArtist7 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist7);
            var performingArtist8 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist8);
            var performingArtist9 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist9);
            var performingArtist10 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtist10);
            var performingArtistRole1 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole1);
            var performingArtistRole2 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole2);
            var performingArtistRole3 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole3);
            var performingArtistRole4 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole4);
            var performingArtistRole5 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole5);
            var performingArtistRole6 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole6);
            var performingArtistRole7 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole7);
            var performingArtistRole8 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole8);
            var performingArtistRole9 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole9);
            var performingArtistRole10 = await GetWorkAttributeAsyncCall(WorkSchema.performingArtistRole10);

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