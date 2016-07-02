using System.Threading.Tasks;
using Ethereum.BlockchainStore.Services.Strategies;
using Ethereum.BlockchainStore.Services.ValueObjects;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Wintellect.Azure.Storage.Table;

namespace Ethereum.BlockchainStore.Services
{
    public class TransactionProcessorService
    {
        private readonly Web3 web3;
        private readonly AzureTable addressTransactionTable;
        private readonly AzureTable contractTable;
        private readonly AzureTable logTable;
        private readonly AzureTable transactionTable;
        private readonly AzureTable transactionVmTable;

        public TransactionProcessorService(Web3 web3, CloudTable transactionCloudTable,
            CloudTable addressTransactionCloudTable, CloudTable contractCloudTable, CloudTable transactionLogCloudTable,
            CloudTable transactionVmCloudTable)
        {
            this.web3 = web3;
            transactionTable = new AzureTable(transactionCloudTable);
            addressTransactionTable = new AzureTable(addressTransactionCloudTable);
            contractTable = new AzureTable(contractCloudTable);
            transactionTable = new AzureTable(transactionCloudTable);
            addressTransactionTable = new AzureTable(addressTransactionCloudTable);
            contractTable = new AzureTable(contractCloudTable);
            logTable = new AzureTable(transactionLogCloudTable);
            transactionVmTable = new AzureTable(transactionVmCloudTable);
        }

        public async Task<TransactionProcessValueObject> ProcessTransaction(string transactionHash, Block block)
        {
            TransactionProcessValueObject transactionProcess;

            var transactionSource = await GetTransaction(transactionHash);
            var transactionReceipt = await GetTransactionReceipt(transactionHash);
            var createContractTransaction = new CreateContractTransactionStrategy(transactionSource, transactionReceipt,
                block, web3, contractTable, transactionTable, addressTransactionTable);
            var contractTransactionStrategy = new ContractTransactionStrategy(transactionSource, transactionReceipt,
                block, web3, contractTable, transactionTable, addressTransactionTable, logTable, transactionVmTable);
            var valueTransactionStrategy = new ValueTransactionStrategy(transactionSource, transactionReceipt, block,
                transactionTable, addressTransactionTable);

            if (createContractTransaction.IsTransactionType())
            {
                transactionProcess = await createContractTransaction.ProcessTransaction();
            }
            else
            {
                if (await contractTransactionStrategy.IsTransactionType())
                {
                    transactionProcess = await contractTransactionStrategy.ProcessTransaction();
                }
                else
                {
                    transactionProcess = valueTransactionStrategy.ProcessTransaction();
                }
            }

            return transactionProcess;
        }

        public async Task<Transaction> GetTransaction(string txnHash)
        {
            return await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txnHash);
        }

        public async Task<TransactionReceipt> GetTransactionReceipt(string txnHash)
        {
            return await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txnHash);
        }
    }
}