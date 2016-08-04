using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SimpleContractScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        public static string GetPageContent(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            var myResponse = myRequest.GetResponse();
            var sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            var result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            return result;

        }
    }
}
