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

        public Task<string> GetAttributeAsyncCall(long key)
        {
            var function = GetGetAttributeFunction();
            return function.CallAsync<string>(key);
        }


        public async Task<Work> GetWorkAsync()
        {
            var work = new Work();
            work.Name = await GetAttributeAsyncCall((long)StorageKeys.Name);
            work.WorkFileIpfsHash = await GetAttributeAsyncCall((long)StorageKeys.WorkFileIpfsHash);
            work.CoverImageIpfsHash = await GetAttributeAsyncCall((long)StorageKeys.CoverImageIpfsHash);
            return work;
        }

        public Task<string> SetAttributeAsync(string addressFrom, long key, string data,
            HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            //ensure bytes32? string?
            var function = GetSetAttributeFunction();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, key, data);
        }
    }
}