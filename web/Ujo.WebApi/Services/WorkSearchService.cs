using System;
using System.Configuration;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Ujo.WebApi.Services
{
    public class WorkSearchService:IWorkSearchService
    {
        private readonly SearchIndexClient indexClient;
        public string errorMessage;

        public WorkSearchService(IOptions<AppSettings> settings)
        {
            try
            {
                string searchServiceName = settings.Value.SearchServiceName;
                string apiKey = settings.Value.SearchServiceKey;
                string indexName = settings.Value.WorkSearchIndexName;

                // Create an HTTP reference to the catalog index
                var searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
                indexClient = searchClient.Indexes.GetClient(indexName);

            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
        }


        public DocumentSearchResult SearchWork(string text)
        {
            // Execute search based on query string
            try
            {
                SearchParameters sp = new SearchParameters()
                {
                    SearchMode = SearchMode.All,
                    Top = 20,
                };
                //todo async
                return indexClient.Documents.Search(text, sp);
            }
            catch (Exception ex)
            {
                //logging
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }

        public DocumentSuggestResult Suggest(string searchText, bool fuzzy)
        {
            // Execute search based on query string
            try
            {
                SuggestParameters sp = new SuggestParameters()
                {
                    UseFuzzyMatching = fuzzy,
                    Top = 8
                };

                return indexClient.Documents.Suggest(searchText, "sg", sp);
            }
            catch (Exception ex)
            {
                //logging
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }



    }
}