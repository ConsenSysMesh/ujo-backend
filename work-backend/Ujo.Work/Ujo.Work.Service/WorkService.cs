using System.Linq;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Ujo.Work.Service
{
    public class WorkService : WorkServiceBase
    {

        public WorkService(Web3 web3, string address) : base(web3)
        {
            this.contract = web3.Eth.GetContract(abi, address);
        }

        public async Task<byte[]> Sha3OfValueAtKeyAsyncCall(string key)
        {
            var function = GetSha3OfValueAtKeyFunction();
            return await function.CallAsync<byte[]>(key);
        }

        public Task<string> GetWorkAttributeAsyncCall(StandardSchema key)
        {
            var function = GetStoreFunction();
            return function.CallAsync<string>(key.ToString());
        }

        public async Task<string> RegisterWorkWithRegistryAsync(string addressFrom, string registryAddres, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetRegisterWorkWithRegistryFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, registryAddres);
        }

        public async Task<string> BulkSetValueAsync(string addressFrom, StandardSchema[] keys, string vals, bool standard = true, HexBigInteger gas = null, HexBigInteger valueAmount = null)
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

        public async Task<Work> GetWorkAsync()
        {
            var work = new Work();
            work.Name = await GetWorkAttributeAsyncCall(StandardSchema.name);
            work.WorkFileIpfsHash = await GetWorkAttributeAsyncCall(StandardSchema.audio);
            work.CoverImageIpfsHash = await GetWorkAttributeAsyncCall(StandardSchema.image);
            work.Genre = await GetWorkAttributeAsyncCall(StandardSchema.genre);
            work.Creator = await GetWorkAttributeAsyncCall(StandardSchema.creator);
            return work;
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

        public Task<string> SetAttributeAsync(string addressFrom, StandardSchema key, string value, bool standard = true,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetSetValueFunction();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, key.ToString(), value, true);
        }
    }
}