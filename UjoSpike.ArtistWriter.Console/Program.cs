using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ethereum.BlockchainStore.Services;
using Nethereum.Web3;

namespace UjoSpike.ArtistWriter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = new MordenStorageProcessor().ExecuteAsync().Result;
            Debug.WriteLine(result);
            System.Console.WriteLine(result);
            System.Console.ReadLine();
        }

        public class MordenStorageProcessor
        {
            public async Task<bool> ExecuteAsync()
            {
                var web3 = new Web3();
                
                //var tableSetup = new CloudTableSetup("UseDevelopmentStorage=true");
                var tableSetup = new CloudTableSetup("DefaultEndpointsProtocol=https;AccountName=ujostorage;AccountKey=DPGFO3b/lkkMLCD6jy495ZZzUSkgcCaPS1/ue1HnpS9ewuOgtHErurN8bhSm960cYD0oWRTXW/86njdNkvS2ZQ==");
                var prefix = "Morden";
                var contractTable = tableSetup.GetContractsTable(prefix);
                var transactionsTable = tableSetup.GetTransactionsTable(prefix);
                var addressTransactionsTable = tableSetup.GetAddressTransactionsTable(prefix);
                var blocksTable = tableSetup.GetBlocksTable(prefix);
                var logTable = tableSetup.GetTransactionsLogTable(prefix);
                var vmStackTable = tableSetup.GetTransactionsVmStackTable(prefix);

                var procesor = new BlockProcessorService(web3, transactionsTable, addressTransactionsTable,
                    contractTable, blocksTable, logTable, vmStackTable);

                for (int i = 1133107; i < 1150000; i++)
                {
                    await procesor.ProcessBlock(i);
                    Debug.WriteLine(i + " " + DateTime.Now.ToString("s"));
                }

                return true;
            }
        }
    }
}
