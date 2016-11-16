using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ujo.Work.Search.Service;

namespace Ujo.Work.Search.Console
{
    class Program
    {
        static string _apiAdminKey = "61AC004C8217E1C7E498C81D64B99422";
        static string _apiSearchKey = "C25D90CA9CF1F3317351A0476D0C2668";
        static string _serviceName = "ujo";
        static string _indexName = "workindex";

        static void Main(string[] args)
        {
            var service = new WorkSearchService(_serviceName, _apiSearchKey, _apiAdminKey, _indexName);

            if (args == null || args.Length == 0 ||  args[0].ToLower() == "deploy")
            {
                 service.CreateIndexAsync().Wait();
            }
            else if( args[0].ToLower() == "delete")
            {
                service.DeleteIndexAsync().Wait();
            }
        }
    }
}
