using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ujo.Work;
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
            workDocument.CoverFile = work.CoverImageIpfsHash;
            workDocument.CreatorsAddresses = new string[] { work.Creator };
            workDocument.CreatorsNames = new string[] { work.Creator };
            workDocument.Genre = work.Genre;
            workDocument.Title = work.Name;
            workDocument.WorkFile = work.WorkFileIpfsHash;
            workDocument.Tags = new string[] { work.Genre };

            await BatchUpdateAsync(new []{ workDocument });

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
