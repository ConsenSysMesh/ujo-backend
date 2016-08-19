using Microsoft.Azure.Search.Models;

namespace Ujo.WebApi.Services
{
    public interface IWorkSearchService
    {
        DocumentSearchResult SearchWork(string text);
        DocumentSuggestResult Suggest(string searchText, bool fuzzy);
    }
}