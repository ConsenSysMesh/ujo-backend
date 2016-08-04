using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using UjoSpike.Service.Model;

namespace UjoSpike.Service
{
    public class ArtistEntityRegistryService
    {
        private readonly Web3 web3;

        private readonly string abi =
            @"[{""constant"":false,""inputs"":[{""name"":""name"",""type"":""string""},{""name"":""isAGroup"",""type"":""bool""},{""name"":""category"",""type"":""string""}],""name"":""registerArtistEntity"",""outputs"":[{""name"":""success"",""type"":""bool""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""numberOfArtistEntities"",""outputs"":[{""name"":""_numberOfArtists"",""type"":""uint256""}],""type"":""function""},{""constant"":true,""inputs"":[{""name"":"""",""type"":""uint256""}],""name"":""artistEntities"",""outputs"":[{""name"":""name"",""type"":""string""},{""name"":""isAGroup"",""type"":""bool""},{""name"":""category"",""type"":""string""}],""type"":""function""},{""anonymous"":false,""inputs"":[{""indexed"":false,""name"":""artityEntityId"",""type"":""uint256""},{""indexed"":false,""name"":""name"",""type"":""string""},{""indexed"":false,""name"":""isAGroup"",""type"":""bool""},{""indexed"":false,""name"":""category"",""type"":""string""}],""name"":""ArtistEntityAdded"",""type"":""event""}]";

        private readonly Contract contract;

        public ArtistEntityRegistryService(Web3 web3, string address)
        {
            this.web3 = web3;
            contract = web3.Eth.GetContract(abi, address);
        }

        public Event GetArtistEntityAddedEvent()
        {
            return contract.GetEvent("ArtistEntityAdded");
        }

        public Function GetRegisterArtistEntityFunction()
        {
            return contract.GetFunction("registerArtistEntity");
        }

        public async Task<bool> RegisterArtistEntityAsyncCall(string name, bool isAGroup, string category)
        {
            var function = GetRegisterArtistEntityFunction();
            return await function.CallAsync<bool>(name, isAGroup, category);
        }

        public async Task<string> RegisterArtistEntityAsync(string addressFrom, string name, bool isAGroup,
            string category, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetRegisterArtistEntityFunction();
            return await function.SendTransactionAsync(addressFrom, gas, valueAmount, name, isAGroup, category);
        }

        public Function GetNumberOfArtistEntitiesFunction()
        {
            return contract.GetFunction("numberOfArtistEntities");
        }

        public async Task<long> NumberOfArtistEntitiesAsyncCall()
        {
            var function = GetNumberOfArtistEntitiesFunction();
            return await function.CallAsync<long>();
        }

        public Function GetArtistEntitiesFunction()
        {
            return contract.GetFunction("artistEntities");
        }

        public async Task<Artist> ArtistEntitiesAsyncCall(long artistId)
        {
            var function = GetArtistEntitiesFunction();
            return await function.CallDeserializingToObjectAsync<Artist>(artistId);
        }
    }
}