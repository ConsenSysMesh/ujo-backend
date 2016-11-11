using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.BlockchainStore.Bootstrap;
using Nethereum.BlockchainStore.Entities;
using Nethereum.BlockchainStore.Processors;
using Nethereum.BlockchainStore.Processors.PostProcessors;
using Nethereum.BlockchainStore.Processors.Transactions;
using Nethereum.BlockchainStore.Repositories;
using Nethereum.JsonRpc.IpcClient;
using NLog.Fluent;

namespace Nethereum.Blockchain.Processing.Console
{
    public class StorageProcessor
    {
        private readonly Web3.Web3 web3;

        private int start;
        private int end;
        private int retryNumber = 0;
        private IBlockProcessor procesor;
        private readonly CloudTable contractTable;
        private const int MaxRetries = 3;

        public StorageProcessor(string url, string connectionString, string prefix, bool postVm = false)
        {

            web3 = url.EndsWith(".ipc") ? new Web3.Web3(new IpcClient(url)) : new Web3.Web3(url);
            var tableSetup = new CloudTableSetup(connectionString);

            contractTable = tableSetup.GetContractsTable(prefix);
            var transactionsTable = tableSetup.GetTransactionsTable(prefix);
            var addressTransactionsTable = tableSetup.GetAddressTransactionsTable(prefix);
            var blocksTable = tableSetup.GetBlocksTable(prefix);
            var logTable = tableSetup.GetTransactionsLogTable(prefix);
            var vmStackTable = tableSetup.GetTransactionsVmStackTable(prefix);

            var blockRepository = new BlockRepository(blocksTable);
            var transactionRepository = new TransactionRepository(transactionsTable);
            var addressTransactionRepository = new AddressTransactionRepository(addressTransactionsTable);
            var contractRepository = new ContractRepository(contractTable);
            var logRepository = new TransactionLogRepository(logTable);
            var vmStackRepository = new TransactionVMStackRepository(vmStackTable);

            var contractTransactionProcessor = new ContractTransactionProcessor(web3, contractRepository, transactionRepository, addressTransactionRepository, vmStackRepository, logRepository);
            var contractCreationTransactionProcessor = new ContractCreationTransactionProcessor(web3, contractRepository, transactionRepository, addressTransactionRepository);
            var valueTrasactionProcessor = new ValueTransactionProcessor(transactionRepository, addressTransactionRepository);

            var tranactionProcessor = new TransactionProcessor(web3, contractTransactionProcessor,
                valueTrasactionProcessor, contractCreationTransactionProcessor);

           
            if (postVm)
            {
                procesor = new BlockVmPostProcessor(web3, blockRepository, tranactionProcessor);
            }
            else
            {
                procesor = new BlockProcessor(web3, blockRepository, tranactionProcessor);
            }
        }

        public async Task Init()
        {
            await Contract.InitContractsCacheAsync(contractTable).ConfigureAwait(false);
        }

        public async Task<bool> ExecuteAsync(int startBlock, int endBlock)
        {
            await Init();
            while (startBlock <= endBlock)
            {
                try
                {
                    await procesor.ProcessBlockAsync(start).ConfigureAwait(false);
                    retryNumber = 0;
                    if (start.ToString().EndsWith("0"))
                        System.Console.WriteLine(start + " " + DateTime.Now.ToString("s"));

                    startBlock = startBlock + 1;
                }
                catch (Exception ex)
                {
                    if (ex.StackTrace.Contains("Only one usage of each socket address"))
                    {
                        System.Threading.Thread.Sleep(1000);
                        System.Console.WriteLine("SOCKET ERROR:" + start + " " + DateTime.Now.ToString("s"));
                        await ExecuteAsync(startBlock, endBlock).ConfigureAwait(false);
                    }
                    else
                    {
                        if (retryNumber != MaxRetries)
                        {
                            retryNumber = retryNumber + 1;
                            await ExecuteAsync(startBlock, endBlock).ConfigureAwait(false);

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