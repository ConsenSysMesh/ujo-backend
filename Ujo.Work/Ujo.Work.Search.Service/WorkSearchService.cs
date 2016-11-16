using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCC.Contracts.StandardData.Processing;
using CCC.Contracts.StandardData.Services.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Nethereum.Web3;
using Ujo.Work.Model;

namespace Ujo.Work.Search.Service
{
    public class WorkSearchService: IStandardDataProcessingService<Work.Model.Work>
    {
        private string _searchServiceName;
        private string _apiKey;
        private string _indexName;
        private string _adminApiKey;

        public WorkSearchService(string searchServiceName, string apiKey, string adminApiKey, string indexName)
        {
            this._searchServiceName = searchServiceName;
            this._apiKey = apiKey;
            this._indexName = indexName;
            this._adminApiKey = adminApiKey;
        }

        private SearchServiceClient CreateSearchServiceClient()
        {
            return new SearchServiceClient(_searchServiceName, new SearchCredentials(_adminApiKey));   
        }

        private SearchIndexClient CreateSearchIndexClient()
        {
            return new SearchIndexClient(_searchServiceName, _indexName, new SearchCredentials(_apiKey));
        }

        public async Task CreateIndexAsync()
        {
           var searchClient = CreateSearchServiceClient();
            var definition = new Index()
            {
                Name = _indexName,
                Fields = new[]
                 {
                    new Field("address", DataType.String)                                 { IsKey = true, IsSearchable = true, IsFilterable = true },
                    new Field("name", DataType.String)                                    { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("artistName", DataType.String)                              { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("artistAddress", DataType.String)                              { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = false },

                    new Field("genre", DataType.String)                                         { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = true },
                    new Field("keywords", DataType.Collection(DataType.String))                { IsSearchable = true, IsFilterable = true, IsFacetable = true },

                    new Field("featuredArtistsNames", DataType.Collection(DataType.String))      { IsSearchable = true, IsFilterable = true, IsFacetable = false },
                    new Field("featuredArtistsAddresses", DataType.Collection(DataType.String))  { IsFilterable = true, IsFacetable = false },
                    new Field("featuredArtists", DataType.Collection(DataType.String))          { IsSearchable = true, IsFilterable = true, IsFacetable = false },

                    new Field("contributingArtistsNames", DataType.Collection(DataType.String))      { IsSearchable = true, IsFilterable = true, IsFacetable = false },
                    new Field("contributingArtistsAddresses", DataType.Collection(DataType.String))  { IsFilterable = true, IsFacetable = false },
                    new Field("contributingArtists", DataType.Collection(DataType.String))          { IsSearchable = true, IsFilterable = true, IsFacetable = false },


                    new Field("performingArtistsNames", DataType.Collection(DataType.String))      { IsSearchable = true, IsFilterable = true, IsFacetable = false },
                    new Field("performingArtistsAddresses", DataType.Collection(DataType.String))  { IsFilterable = true, IsFacetable = false },
                    new Field("performingArtists", DataType.Collection(DataType.String))          { IsSearchable = true, IsFilterable = true, IsFacetable = false },


                    new Field("label", DataType.String)                                     { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = true },
                    new Field("description", DataType.String)                               { IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("publisher", DataType.String)                                 { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = true },
                    new Field("hasPartOf", DataType.Boolean)                                { IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("isPartOf", DataType.Boolean)                                 { IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("isFamilyFriendly", DataType.String)                          { IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("license", DataType.String)                                   { IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("iswcCode", DataType.String)                                  { IsSearchable = true, IsFilterable = true, IsSortable = true, IsFacetable = false },

                    new Field("audio", DataType.String)                                   { IsFilterable = true, IsSortable = true, IsFacetable = false },
                    new Field("image", DataType.String)                                   { IsFilterable = true, IsSortable = true, IsFacetable = false }
                },
                Suggesters = new[]
                 {
                    new Suggester("sg", SuggesterSearchMode.AnalyzingInfixMatching, "address", "name", "genre", "featuredArtistsNames", "contributingArtistsNames", "performingArtistsNames", "label", "publisher", "iswcCode")
                }
           };

           await searchClient.Indexes.CreateAsync(definition);
        }

        public Task<DocumentSearchResult<WorkDocument>> GetWorksByArtistAsync(string artistAddress)
        {
            var indexClient = CreateSearchIndexClient();
            //we store the addresses in lower case
            var sp = new SearchParameters
            {
                SearchMode = SearchMode.All,
                IncludeTotalResultCount = true,
                Filter = String.Format("artistAddress eq '{0}' or featuredArtistsAddresses/any(t: t eq '{0}') or contributingArtistsAddresses/any(t: t eq '{0}') or performingArtistsAddresses/any(t: t eq '{0}')", artistAddress.ToLower())
            };

            return indexClient.Documents.SearchAsync<WorkDocument>("*", sp);
        }

        public async Task DeleteIndexAsync()
        {
            var serviceClient = CreateSearchServiceClient();

            if (await serviceClient.Indexes.ExistsAsync(_indexName))
            {
                await serviceClient.Indexes.DeleteAsync(_indexName);
            }
        }

        public async Task<DocumentSearchResult<WorkDocument>> SearchAsync(string text)
        {
            var indexClient = CreateSearchIndexClient();
            var sp = new SearchParameters
            {
                SearchMode = SearchMode.All,
                Top = 20,
                Facets = new string[] {"genre", "keywords"},
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

        public async Task UpsertAsync(params Work.Model.Work[] works)
        {
            var workDocuments = new List<WorkDocument>();
            if (works != null)
            {
                foreach (var work in works)
                {
                    var workDocument = new WorkDocument();
                    workDocument.Address = work.Address.ToLower();
                    workDocument.Image = work.CoverImageIpfsHash;
                    workDocument.ArtistAddress = work.ByArtistAddress.ToLower();
                    workDocument.ArtistName = work.ByArtistName;
                    workDocument.Genre = work.Genre;
                    workDocument.Name = work.Name;
                    workDocument.Audio = work.WorkFileIpfsHash;
                    workDocument.PerformingArtists = GetArtistsPipeDelimeted(work.PerformingArtists);
                    workDocument.PerformingArtistsNames = GetArtistsNames(work.PerformingArtists);
                    workDocument.PerformingArtistsAddresses = GetArtistsAddresses(work.PerformingArtists);
                    workDocument.ContributingArtists = GetArtistsPipeDelimeted(work.ContributingArtists);
                    workDocument.ContributingArtistsNames = GetArtistsNames(work.ContributingArtists);
                    workDocument.ContributingArtistsAddresses = GetArtistsAddresses(work.ContributingArtists);
                    workDocument.FeaturedArtists = GetArtistsPipeDelimeted(work.FeaturedArtists);
                    workDocument.FeaturedArtistsNames = GetArtistsNames(work.FeaturedArtists);
                    workDocument.FeaturedArtistsAddresses = GetArtistsAddresses(work.FeaturedArtists);
                    //workDocument.DateCreated = work.DateCreated ?? "";
                   // workDocument.DateModified = work.DateModified ?? "";
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

                    workDocuments.Add(workDocument);
                }
            }
            await BatchUpdateAsync(workDocuments.ToArray());
        }

        private string[] GetArtistsAddresses(List<WorkArtist> artists)
        {
            if(artists != null && artists.Count > 0)
            {
                return artists.Select(x => x.Address.ToLower()).ToArray();
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


        public async Task DataChangedAsync(Work.Model.Work work, EventLog<DataChangedEvent> dataEventLog)
        {
            await UpsertAsync(work);
        }

        public async Task RemovedAsync(string workAddress)
        {
            var workDocument = new WorkDocument();
            workDocument.Address = workAddress;
            await BatchUpdateAsync(null, null, new[] { workDocument });
        }

      
        public async Task BatchUpdateAsync<T>(IEnumerable<T> uploadOrMerge, IEnumerable<T> upload = null, IEnumerable<T> delete = null) where T: WorkDocument
        {
            var serviceClient = CreateSearchServiceClient();
            var indexClient = serviceClient.Indexes.GetClient(_indexName);

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

        public async Task UpsertAsync(Work.Model.Work work)
        {
            if (work != null)
            {
                await UpsertAsync(new[] { work });
            }
        }
    }
}
