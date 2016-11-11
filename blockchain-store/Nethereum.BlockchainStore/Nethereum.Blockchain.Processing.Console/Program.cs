using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Nethereum.Blockchain.Processing.Console
{
    class Program
    {
        static string prefix = "Morden";
        static string connectionString =
            "DefaultEndpointsProtocol=https;AccountName=XX;AccountKey=XXX";

        static void Main(string[] args)
        {
            //string url = "http://localhost:8045";
            //int start = 500599;
            //int end = 1000000;
            //bool postVm = true;

            string url = args[0];
            int start = Convert.ToInt32(args[1]);
            int end = Convert.ToInt32(args[2]);
            bool postVm = false;
            if (args.Length > 3)
            {
                if (args[3].ToLower() == "postvm")
                {
                    postVm = true;
                }
            }

            var proc = new StorageProcessor(url, connectionString, prefix, postVm);
            proc.Init().Wait();
            var result = proc.ExecuteAsync(start, end).Result;

            Debug.WriteLine(result);
            System.Console.WriteLine(result);
            System.Console.ReadLine();
        }
    }
}

