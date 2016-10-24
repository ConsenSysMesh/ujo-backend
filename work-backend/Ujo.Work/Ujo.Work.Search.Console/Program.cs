using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ujo.Search.Service;

namespace Ujo.Work.Search.Console
{
    class Program
    {
        static string apiAdminKey = "61AC004C8217E1C7E498C81D64B99422";
        static string apiSearchKey = "C25D90CA9CF1F3317351A0476D0C2668";
        static string serviceName = "ujo";
        static string indexName = "workindex";

        static void Main(string[] args)
        {
            var service = new WorkSearchService(serviceName, apiSearchKey, apiAdminKey, indexName);

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
