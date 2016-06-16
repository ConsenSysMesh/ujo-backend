using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Web3;
using UjoSpike.Service;
using UjoSpike.Service.Model;

namespace UjoSpike.WebJob
{
    public class Functions
    {
        [Singleton]
        public static async Task ProcessArtistRegistry([TimerTrigger("00:01:00")] TimerInfo timer, [Table("ArtistEntity")] CloudTable tableBinding, TextWriter log)
        {
            

                log.WriteLine("Start job");
                var web3 = new Web3("https://eth3.augur.net");
                var service = new ArtistEntityRegistryService(web3, "0x77caa46901bbad6e6f19615643093dff7bc19394");
                log.WriteLine("Getting Number of Artists registered");
                var totalNumber = await service.NumberOfArtistEntitiesAsyncCall();
                log.WriteLine("Number of Entities " + totalNumber.ToString());
                var storageService = new ArtistEntityStorageService(tableBinding);
                var processed = await storageService.GetProcessInfo();
                log.WriteLine("Number of Entities Processed " + processed.Number.ToString());

                if (totalNumber > processed.Number + 1)
                {
                    log.WriteLine("Adding new artists to storage");


                    for (var i = processed.Number; i < totalNumber; i++)
                    {
                        var artist = await service.ArtistEntitiesAsyncCall(i);

                        var artistEntity = new ArtistEntity();
                        artistEntity.Id = i;
                        artistEntity.Name = artist.Name;
                        artistEntity.Category = RemoveInvalidCharacters(artist.Category);
                        artistEntity.IsGroup = artist.IsAGroup;
                        log.WriteLine(artistEntity.Id + " " + artistEntity.Name + " " + artistEntity.Category + " " +
                                      artistEntity.IsGroup);
                        await storageService.UpsertArtist(artistEntity);
                        processed.Number = i;
                        await storageService.UpsertProcessInfo(processed);
                    }

                    log.WriteLine("Number of Entities Processed after update " + processed.Number);
                }
        }

        public static string RemoveInvalidCharacters(string value)
        {
            return value.Replace("/", "").Replace("\\", "").Replace("#", "").Replace("?", "");
        }

}
}
