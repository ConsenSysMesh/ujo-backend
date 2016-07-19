using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ethereum.BlockchainStore.Services;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Web3;
using Contract = Ethereum.BlockchainStore.Entities.Contract;
using Newtonsoft.Json.Linq;
using NLog.Fluent;

namespace UjoSpike.ArtistWriter.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            //string url = "./geth.ipc";

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
            
            string prefix = "Morden";
            string connectionString =
                "DefaultEndpointsProtocol=https;AccountName=ujostorage;AccountKey=DPGFO3b/lkkMLCD6jy495ZZzUSkgcCaPS1/ue1HnpS9ewuOgtHErurN8bhSm960cYD0oWRTXW/86njdNkvS2ZQ==";

            var proc = new StorageProcessor(url, start, end, connectionString, prefix, postVm);
           // proc.Init().Wait();
            var result = proc.ExecuteAsync().Result;
            
            Debug.WriteLine(result);
            System.Console.WriteLine(result);
            System.Console.ReadLine();
        }

        public class StorageProcessor
        {
            private readonly Web3 web3;
          
            private int start;
            private int end;
            private int retryNumber = 0;
            private BlockProcessorService procesor;
            private CloudTable contractTable;
            private const int MaxRetries = 3;

            public StorageProcessor(string url, int start, int end, string connectionString, string prefix, bool postVm = false)
            {
              
                
                this.start = start;
                this.end = end;

                web3 = url.EndsWith(".ipc") ? new Web3(new IpcClient(url)) : new Web3(url);
                var tableSetup = new CloudTableSetup(connectionString);
               
                contractTable = tableSetup.GetContractsTable(prefix);
                var transactionsTable = tableSetup.GetTransactionsTable(prefix);
                var addressTransactionsTable = tableSetup.GetAddressTransactionsTable(prefix);
                var blocksTable = tableSetup.GetBlocksTable(prefix);
                var logTable = tableSetup.GetTransactionsLogTable(prefix);
                var vmStackTable = tableSetup.GetTransactionsVmStackTable(prefix);
               
                //TODO FACTORY to process only contracts and other scenarios
                //This could be a base class
                if (postVm)
                {
                    procesor = new BlockVmStackPostProcessorService(web3, transactionsTable, addressTransactionsTable,
                        contractTable, blocksTable, logTable, vmStackTable);
                }
                else
                {
                    procesor = new BlockProcessorService(web3, transactionsTable, addressTransactionsTable,
                        contractTable, blocksTable, logTable, vmStackTable);
                }
            }

            public async Task Init()
            {
                await Contract.InitContractsCacheAsync(contractTable).ConfigureAwait(false);
            }

            public async Task<bool> ExecuteAsync()
            {
                await Init();
                while (start <= end)
                {
                    try
                    {
                        await procesor.ProcessBlock(start).ConfigureAwait(false);
                        retryNumber = 0;
                        if (start.ToString().EndsWith("0"))
                            System.Console.WriteLine(start + " " + DateTime.Now.ToString("s"));

                        start = start + 1;
                    }
                    catch (Exception ex)
                    {
                        if (ex.StackTrace.Contains("Only one usage of each socket address"))
                        {
                            System.Threading.Thread.Sleep(1000);
                            System.Console.WriteLine("SOCKET ERROR:" + start + " " + DateTime.Now.ToString("s"));
                            await ExecuteAsync().ConfigureAwait(false);
                        }
                        else
                        {
                            if (retryNumber != MaxRetries)
                            {
                                retryNumber = retryNumber + 1;
                                await ExecuteAsync().ConfigureAwait(false);

                            }
                            else
                            {
                                start = start + 1;
                                Log.Error().Exception(ex).Message("BlockNumber" + start).Write();
                                System.Console.WriteLine("ERROR:" + start + " " + DateTime.Now.ToString("s"));
                            }
                        }
                    }
                }

                return true;
            }
        }


    }
}
