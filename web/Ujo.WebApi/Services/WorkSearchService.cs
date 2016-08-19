using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;

namespace Ujo.WebApi.Services
{
    public class WorkSearchService : IWorkSearchService
    {
        private readonly ISearchIndexClient indexClient;
        public string errorMessage;

        public WorkSearchService(IOptions<AppSettings> settings)
        {
            var searchServiceName = settings.Value.SearchServiceName;
            var apiKey = settings.Value.SearchServiceKey;
            var indexName = settings.Value.WorkSearchIndexName;

            // Create an HTTP reference to the catalog index
            var searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));

            indexClient = searchClient.Indexes.GetClient(indexName);
        }


        public async Task<DocumentSearchResult> SearchWork(string text)
        {
            var sp = new SearchParameters
            {
                SearchMode = SearchMode.All,
                Top = 20,
                IncludeTotalResultCount = true
            };

            return await indexClient.Documents.SearchAsync(text, sp);
        }

        public async Task<DocumentSuggestResult> Suggest(string searchText, bool fuzzy)
        {
            var sp = new SuggestParameters
            {
                UseFuzzyMatching = fuzzy,
                Top = 8
            };

            return await indexClient.Documents.SuggestAsync(searchText, "sg", sp);
        }
    }
}