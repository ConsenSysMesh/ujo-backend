
namespace Ujo.WebApi.Services
{
    using System.Threading.Tasks;
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;
    using Microsoft.Extensions.Options;
    using Search.Service;

    public class WorkSearchService : IWorkSearchService
    {
        private Ujo.Search.Service.WorkSearchService innerSearchService;

        public WorkSearchService(IOptions<AppSettings> settings)
        {
            var searchServiceName = settings.Value.SearchServiceName;
            var apiKey = settings.Value.SearchServiceKey;
            var indexName = settings.Value.WorkSearchIndexName;

            innerSearchService = new Search.Service.WorkSearchService(searchServiceName, apiKey, null, indexName);
        }

        public Task<DocumentSearchResult<WorkDocument>> SearchAsync(string text)
        {
            return innerSearchService.SearchAsync(text);
        }


        public Task<DocumentSuggestResult<WorkDocument>> SuggestAsync(string searchText, bool fuzzy = true)
        {
            return innerSearchService.SuggestAsync(searchText, fuzzy);
        }

        public Task<DocumentSearchResult<WorkDocument>> GetWorksByArtistAsync(string artistAddress)
        {
            return innerSearchService.GetWorksByArtistAsync(artistAddress);
        }
    }
}

