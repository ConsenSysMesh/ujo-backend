using System.Threading.Tasks;
using Ethereum.BlockchainStore.Services.Strategies;
using Ethereum.BlockchainStore.Services.ValueObjects;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Wintellect.Azure.Storage.Table;

namespace Ethereum.BlockchainStore.Services
{
    public class TransactionVmStackPostProcessorService : TransactionProcessorService
    {
        public TransactionVmStackPostProcessorService(Web3 web3, CloudTable transactionCloudTable, CloudTable addressTransactionCloudTable, CloudTable contractCloudTable, CloudTable transactionLogCloudTable, CloudTable transactionVmCloudTable) : base(web3, transactionCloudTable, addressTransactionCloudTable, contractCloudTable, transactionLogCloudTable, transactionVmCloudTable)
        {
           
        }

        public override async Task<TransactionProcessValueObject> ProcessTransaction(string transactionHash, Block block)
        {
            TransactionProcessValueObject transactionProcess = new TransactionProcessValueObject();

            var transactionSource = await GetTransaction(transactionHash).ConfigureAwait(false);
            var transactionReceipt = await GetTransactionReceipt(transactionHash).ConfigureAwait(false);

            var contractTransactionStrategy = new ContractVmStackPostProcessTransactionStrategy(transactionSource, transactionReceipt,
                block, Web3, ContractTable, TransactionTable, AddressTransactionTable, LogTable, TransactionVmTable);

            if (await contractTransactionStrategy.IsTransactionType())
            {
                transactionProcess = await contractTransactionStrategy.ProcessTransaction().ConfigureAwait(false);
            }

            return transactionProcess;
        }
    }

    public class TransactionProcessorService
    {
        protected Web3 Web3 { get; }
        protected AzureTable AddressTransactionTable { get; }
        protected AzureTable ContractTable { get; }
        protected AzureTable LogTable { get; }
        protected AzureTable TransactionTable { get; }
        protected AzureTable TransactionVmTable { get; }

        public TransactionProcessorService(Web3 web3, CloudTable transactionCloudTable,
            CloudTable addressTransactionCloudTable, CloudTable contractCloudTable, CloudTable transactionLogCloudTable,
            CloudTable transactionVmCloudTable)
        {
            this.Web3 = web3;
            TransactionTable = new AzureTable(transactionCloudTable);
            AddressTransactionTable = new AzureTable(addressTransactionCloudTable);
            ContractTable = new AzureTable(contractCloudTable);
            TransactionTable = new AzureTable(transactionCloudTable);
            AddressTransactionTable = new AzureTable(addressTransactionCloudTable);
            ContractTable = new AzureTable(contractCloudTable);
            LogTable = new AzureTable(transactionLogCloudTable);
            TransactionVmTable = new AzureTable(transactionVmCloudTable);
        }

        public virtual async Task<TransactionProcessValueObject> ProcessTransaction(string transactionHash, Block block)
        {
            TransactionProcessValueObject transactionProcess = new TransactionProcessValueObject();

            var transactionSource = await GetTransaction(transactionHash).ConfigureAwait(false);
            var transactionReceipt = await GetTransactionReceipt(transactionHash).ConfigureAwait(false);
            var createContractTransaction = new CreateContractTransactionStrategy(transactionSource, transactionReceipt,
                block, Web3, ContractTable, TransactionTable, AddressTransactionTable);
            var contractTransactionStrategy = new ContractTransactionStrategy(transactionSource, transactionReceipt,
                block, Web3, ContractTable, TransactionTable, AddressTransactionTable, LogTable, TransactionVmTable);
            var valueTransactionStrategy = new ValueTransactionStrategy(transactionSource, transactionReceipt, block,
                TransactionTable, AddressTransactionTable);

            if (createContractTransaction.IsTransactionType())
            {
                transactionProcess = await createContractTransaction.ProcessTransaction().ConfigureAwait(false);
            }
            else { 
            
                    if (await contractTransactionStrategy.IsTransactionType())
                    {
                        transactionProcess = await contractTransactionStrategy.ProcessTransaction().ConfigureAwait(false);
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
            return await Web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txnHash).ConfigureAwait(false);
        }

        public async Task<TransactionReceipt> GetTransactionReceipt(string txnHash)
        {
            return await Web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txnHash).ConfigureAwait(false);
        }
    }
}