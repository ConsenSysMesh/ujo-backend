using System.Threading.Tasks;
using Microsoft.Azure.Search.Models;
using Ujo.Search.Service;

namespace Ujo.WebApi.Services
{
    public interface IWorkSearchService
    {
        Task<DocumentSearchResult<WorkDocument>> SearchAsync(string text);
        Task<DocumentSuggestResult<WorkDocument>> SuggestAsync(string searchText, bool fuzzy);
        Task<DocumentSearchResult<WorkDocument>> GetWorksByArtistAsync(string artistAddress);
    }
}
