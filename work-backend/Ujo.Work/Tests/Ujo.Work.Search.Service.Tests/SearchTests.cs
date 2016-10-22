using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ujo.Search.Service.Tests
{
    public class SearchTests
    {
        string apiAdminKey = "61AC004C8217E1C7E498C81D64B99422";
        string apiSearchKey = "C25D90CA9CF1F3317351A0476D0C2668";
        string serviceName = "ujo";
        string indexName = "worktestindex";

        [Fact]
        public async Task Test()
        {
            var service = new WorkSearchService(serviceName, apiSearchKey, apiAdminKey, indexName);
            await service.DeleteIndexAsync();
            await service.CreateIndexAsync();

            var works = new Work.Model.Work[]
            {
                new Work.Model.Work()
                {
                    Address = "0x050c98dfa840cf812c948fa5b4e247fff75bb063",
                    ByArtistName = "The band",
                    ByArtistAddress = "0x050c98dfa840cf812c948fa5b4e247fff75bb063_0",
                    FeaturedArtists = new List<Work.Model.WorkArtist>(new [] { new Work.Model.WorkArtist(1, "Simon", "0x050c98dfa840cf812c948fa5b4e247fff75bb063_1", "Guitar"),
                                                                               new Work.Model.WorkArtist(2, "Juan", "0x050c98dfa840cf812c948fa5b4e247fff75bb063_2", "Tamborine") }),
                    PerformingArtists = new List<Work.Model.WorkArtist>(new [] { new Work.Model.WorkArtist(3, "Gael", "0x050c98dfa840cf812c948fa5b4e247fff75bb063_3", "Piano"),
                                                                                new Work.Model.WorkArtist(4, "Karl", "0x050c98dfa840cf812c948fa5b4e247fff75bb063_4", "Vocal") }),

                    ContributingArtists = new List<Work.Model.WorkArtist>(new[] { new Work.Model.WorkArtist(4, "Jesse", "0x050c98dfa840cf812c948fa5b4e247fff75bb063_5", "Lyrics"),
                                                                                new Work.Model.WorkArtist(5, "Gabe", "0x050c98dfa840cf812c948fa5b4e247fff75bb063_6", "Producer"),
                                                                                new Work.Model.WorkArtist(6, "Vlad", "0x050c98dfa840cf812c948fa5b4e247fff75bb063_7", "Cover") }),
                    Genre = "Techno",
                    Keywords = "TechHouse, House",
                    Name = "Blackout",
                    Image = "QmbwG5QB9ssqu49WDyw93hwGBYMZiueqNKYCkGL6DZC7Vb",
                    Audio = "workFile",
                    IswcCode = "123",
                    Label = "White Label",
                    Publisher = "White publishing",
                    Description = "immersing techno house, with tribal vocal hints",
                    

                },

                new Work.Model.Work()
                {
                    Address = "0x23c575374941865b641e733c44073c8f02a11229",
                    ByArtistName = "The band 2",
                    ByArtistAddress = "0x23c575374941865b641e733c44073c8f02a11229_1",
                    FeaturedArtists = new List<Work.Model.WorkArtist>(new [] { new Work.Model.WorkArtist(1, "Laurent Garnier", "0x23c575374941865b641e733c44073c8f02a11229_1", "Guitar"),
                                                                               new Work.Model.WorkArtist(2, "Juan", "0x050c98dfa840cf812c948fa5b4e247fff75bb063_2", "Tamborine") }),
                    Genre = "Techno",
                    Keywords = "Trance, House",
                    Name = "Wake Up",
                    Image = "QmbwG5QB9ssqu49WDyw93hwGBYMZiueqNKYCkGL6DZC7Vb",
                    Audio = "workFile",
                    IswcCode = "124",
                    Label = "White Label",
                    Publisher = "White publishing",
                    Description = "immersing hard techno trance"
                }
            };

            await service.UploadOrMergeAsync(works);

            //Wait to be indexed
            Thread.Sleep(2000);

            var result = await service.Search("House");
            Assert.Equal(2, result.Count);
            Assert.Equal(4, result.Facets["keywords"].Count);
            //we have 1 house, 1 techHouse, 1 Trance
            Assert.Equal(1, result.Facets["genre"].Count);
            //we have 1 main genre Techno

            //we may want to add genre as the fisrt tag as per sound cloud

            result = await service.Search("Juan");
            Assert.Equal(2, result.Count);

            result = await service.GetWorksByArtistAsync("0x050c98dfa840cf812c948fa5b4e247fff75bb063_2");
            Assert.Equal(2, result.Count);

            result = await service.GetWorksByArtistAsync("0x23c575374941865b641e733c44073c8f02a11229_1");
            Assert.Equal(1, result.Count);

            var suggestResult = await service.SuggestAsync("Laur", true);
            Assert.Equal(1, suggestResult.Results.Count);

            await service.DeleteIndexAsync();
        }
    }
}
