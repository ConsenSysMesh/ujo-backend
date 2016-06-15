using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.Web3;
using UjoSpike.Service;
using UjoSpike.Service.Model;

namespace UjoSpike.ArtistWriter.Console
{
    public class DumpRegister
    {
        public async Task<Artist[]> GetAllArtists()
        {
           

            var web3 = new Web3("https://eth3.augur.net");
            var service = new ArtistEntityRegistryService(web3, "0x77caa46901bbad6e6f19615643093dff7bc19394");
            var totalNumber = await service.NumberOfArtistEntitiesAsyncCall();
            var artists = new List<Artist>();
            for (int i = 0; i < totalNumber; i++)
            {
                artists.Add(await service.ArtistEntitiesAsyncCall(i));
            }
            return artists.ToArray();
        } 
    }
}