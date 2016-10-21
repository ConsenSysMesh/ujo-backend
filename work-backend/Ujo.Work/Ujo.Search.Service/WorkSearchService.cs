using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ujo.Work;
using Ujo.Work.Model;
using Ujo.Work.Service;

namespace Ujo.Search.Service
{
    public class WorkSearchService
    {
        private string searchServiceName;
        private string apiKey;
        private string indexName;
        private string adminApiKey;

        public WorkSearchService(string searchServiceName, string apiKey, string adminApiKey, string indexName)
        {
            this.searchServiceName = searchServiceName;
            this.apiKey = apiKey;
            this.indexName = indexName;
            this.adminApiKey = adminApiKey;
        }

        private SearchServiceClient CreateSearchServiceClient()
        {
            return new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));   
        }

        private SearchIndexClient CreateSearchIndexClient()
        {
            return new SearchIndexClient(searchServiceName, indexName, new SearchCredentials(apiKey));
        }

        public async Task CreateIndexAsync()
        {
           var searchClient = CreateSearchServiceClient();
            var definition = new Index()
            {
                Name = indexName,
                Fields = new[]
                 {
                    new Field("address", DataType.String)                                 { IsKey = true, IsSearchable = true, IsFilterable = true },
                    new Field("title", DataType.String)                                   { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("genre", DataType.String)                                   { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = true },
                    new Field("tags", DataType.Collection(DataType.String))               { IsSearchable = true, IsFilterable = true, IsFacetable = true },
                    new Field("creatorsNames", DataType.Collection(DataType.String))      { IsSearchable = true, IsFilterable = true, IsFacetable = false },
                    new Field("creatorsAddresses", DataType.Collection(DataType.String))  { IsSearchable = true, IsFilterable = true, IsFacetable = false },
                    new Field("workFile", DataType.String)                                { IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("coverFile", DataType.String)                               { IsFilterable = true, IsSortable = true, IsFacetable = false }
                },
                Suggesters = new[]
                 {
                    new Suggester("sg", SuggesterSearchMode.AnalyzingInfixMatching, "address", "title", "genre", "creatorsNames", "creatorsAddresses")
                }
           };

           await searchClient.Indexes.CreateAsync(definition);
        }

        public Task<DocumentSearchResult<WorkDocument>> GetWorksByArtistAsync(string artistAddress)
        {
            var indexClient = CreateSearchIndexClient();

            var sp = new SearchParameters
            {
                SearchMode = SearchMode.All,
                IncludeTotalResultCount = true,
                Filter = String.Format("creatorsAddresses/any(t: t eq '{0}')", artistAddress)
            };

            return indexClient.Documents.SearchAsync<WorkDocument>("*", sp);
        }

        public async Task DeleteIndexAsync()
        {
            var serviceClient = CreateSearchServiceClient();

            if (await serviceClient.Indexes.ExistsAsync(indexName))
            {
                await serviceClient.Indexes.DeleteAsync(indexName);
            }
        }

        public async Task<DocumentSearchResult<WorkDocument>> Search(string text)
        {
            var indexClient = CreateSearchIndexClient();
            var sp = new SearchParameters
            {
                SearchMode = SearchMode.All,
                Top = 20,
                Facets = new string[] {"genre", "tags"},
                IncludeTotalResultCount = true
            };

            return await indexClient.Documents.SearchAsync<WorkDocument>(text, sp);
        }

        public async Task<DocumentSuggestResult<WorkDocument>> SuggestAsync(string searchText, bool fuzzy = true)
        {
            var indexClient = CreateSearchIndexClient();
            var sp = new SuggestParameters
            {
                UseFuzzyMatching = fuzzy,
                Top = 8
            };

            return await indexClient.Documents.SuggestAsync<WorkDocument>(searchText, "sg", sp);
        }

        public async Task UploadOrMergeAsync(Work.Model.Work work)
        {
            var workDocument = new WorkDocument();
            workDocument.Address = work.Address;
            workDocument.Image = work.CoverImageIpfsHash;
            workDocument.ArtistAddress = work.ByArtistAddress;
            workDocument.ArtistName =  work.ByArtistName;
            workDocument.Genre = work.Genre;
            workDocument.Name = work.Name;
            workDocument.Audio = work.WorkFileIpfsHash;
            workDocument.PerformingArtists = GetArtistsPipeDelimeted(work.PerformingArtists);
            workDocument.PerformingArtistsNames = GetArtistsNames(work.PerformingArtists);
            workDocument.PerformingArtitsAddresses = GetArtistsAddresses(work.PerformingArtists);
            workDocument.ContributingArtists = GetArtistsPipeDelimeted(work.ContributingArtists);
            workDocument.ContributingArtistsNames = GetArtistsNames(work.ContributingArtists);
            workDocument.ContributingArtistsAddresses = GetArtistsAddresses(work.ContributingArtists);
            workDocument.FeaturedArtists = GetArtistsPipeDelimeted(work.FeaturedArtists);
            workDocument.FeaturedArtistsNames = GetArtistsNames(work.FeaturedArtists);
            workDocument.FeaturedArtitsAddresses = GetArtistsAddresses(work.FeaturedArtists);
            workDocument.DateCreated = work.DateCreated ?? "";
            workDocument.DateModified = work.DateModified ?? "";
            workDocument.Label = work.Label ?? "";
            workDocument.Description = work.Description ?? "";
            workDocument.Publisher = work.Publisher ?? "";
            workDocument.HasPartOf = work.HasPartOf;
            workDocument.IsPartOf = work.IsPartOf;
            workDocument.IsFamilyFriendly = work.IsFamilyFriendly;
            workDocument.License = work.License ?? "";
            workDocument.IswcCode = work.IswcCode ?? "";

            var keyWords = new List<string>();
            keyWords.Add(work.Genre);

            if (!string.IsNullOrEmpty(work.Keywords))
                keyWords.AddRange(work.Keywords.Split(','));
            
            workDocument.Keywords = keyWords.ToArray();

            await BatchUpdateAsync(new []{ workDocument });

        }

        private string[] GetArtistsAddresses(List<WorkArtist> artists)
        {
            if(artists != null && artists.Count > 0)
            {
                return artists.Select(x => x.Address).ToArray();
            }
            return new string[] { };
        }

        private string[] GetArtistsNames(List<WorkArtist> artists)
        {
            if (artists != null && artists.Count > 0)
            {
                return artists.Select(x => x.Name).ToArray();
            }
            return new string[] { };
        }

        private string[] GetArtistsPipeDelimeted(List<WorkArtist> artists)
        {
            if (artists != null && artists.Count > 0)
            {
                return artists.Select(x => x.Index.ToString() + "|" + x.Address + "|" + x.Name + "|" + x.Role).ToArray();
            }
            return new string[] { };
        }

        public async Task DeleteAsync(string workAddress)
        {
            var workDocument = new WorkDocument();
            workDocument.Address = workAddress;
            await BatchUpdateAsync(null, null, new[] { workDocument });
        }

      
        public async Task BatchUpdateAsync<T>(IEnumerable<T> uploadOrMerge, IEnumerable<T> upload = null, IEnumerable<T> delete = null) where T: WorkDocument
        {
            var serviceClient = CreateSearchServiceClient();
            var indexClient = serviceClient.Indexes.GetClient(indexName);

            var actions = new List<IndexAction<T>>();

            if (uploadOrMerge != null)
            {
                foreach (var item in uploadOrMerge)
                {
                    actions.Add(IndexAction.MergeOrUpload<T>(item));
                }
            }

            if (upload != null)
            {
                foreach (var item in upload)
                {
                    actions.Add(IndexAction.Upload<T>(item));
                }
            }

            if (delete != null)
            {
                foreach (var item in delete)
                {
                    actions.Add(IndexAction.Delete<T>(item));
                }
            }

            var batch = IndexBatch.New(actions);

            var retryStrategy =
            new Incremental(3, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
            var retryPolicy =
                new RetryPolicy<SearchIndexErrorDetectionStrategy>(retryStrategy);
            //there is a retry policy for the client search now, we might be able to use that instead
            await retryPolicy.ExecuteAsync(async () => await indexClient.Documents.IndexAsync(batch));
        }

        private class SearchIndexErrorDetectionStrategy : ITransientErrorDetectionStrategy
        {
            public bool IsTransient(Exception ex)
            {
                return ex is IndexBatchException;
            }
        }
    }
}
