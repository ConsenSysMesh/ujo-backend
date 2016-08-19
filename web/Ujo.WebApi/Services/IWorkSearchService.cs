using System.Threading.Tasks;
using Microsoft.Azure.Search.Models;

namespace Ujo.WebApi.Services
{
    public interface IWorkSearchService
    {
        Task<DocumentSearchResult> SearchWork(string text);
        Task<DocumentSuggestResult> Suggest(string searchText, bool fuzzy);
    }
}