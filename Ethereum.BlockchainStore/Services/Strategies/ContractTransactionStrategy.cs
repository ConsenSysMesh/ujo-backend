using System.Threading.Tasks;
using Ethereum.BlockchainStore.Entities;
using Ethereum.BlockchainStore.Services.ValueObjects;
using Nethereum.RPC.DebugGeth.DTOs;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using Wintellect.Azure.Storage.Table;
using Block = Nethereum.RPC.Eth.DTOs.Block;
using Contract = Ethereum.BlockchainStore.Entities.Contract;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace Ethereum.BlockchainStore.Services.Strategies
{
    public class ContractTransactionStrategy
    {
        private readonly AzureTable adressTransactionTable;
        private readonly Block block;
        private readonly AzureTable contractTable;
        private readonly AzureTable logTable;
        private readonly TransactionReceipt transactionReceipt;
        private readonly Transaction transactionSource;
        private readonly AzureTable transactionTable;
        private readonly AzureTable transactionVmTable;
        private readonly VmStackErrorChecker vmStackErrorChecker;
        private readonly Web3 web3;

        public ContractTransactionStrategy(Transaction transactionSource, TransactionReceipt transactionReceipt,
            Block block, Web3 web3, AzureTable contractTable, AzureTable transactionTable,
            AzureTable adressTransactionTable, AzureTable logTable, AzureTable transactionVmTable)
        {
            this.transactionSource = transactionSource;
            this.transactionReceipt = transactionReceipt;
            this.block = block;
            this.web3 = web3;
            this.contractTable = contractTable;
            this.transactionTable = transactionTable;
            this.adressTransactionTable = adressTransactionTable;
            this.logTable = logTable;
            this.transactionVmTable = transactionVmTable;
            vmStackErrorChecker = new VmStackErrorChecker();
        }


        public async Task<bool> IsTransactionType()
        {
            return await Contract.ExistsAsync(contractTable, transactionSource.To);
        }

        public async Task<TransactionProcessValueObject> ProcessTransaction()
        {
            var transactionProcess = new TransactionProcessValueObject();
            var transactionHash = transactionSource.TransactionHash;
            var hasStackTrace = false;
            var stackTrace = await GetTransactionVmStack(transactionHash);
            var error = string.Empty;
            var hasError = false;

            if (stackTrace != null)
            {
                error = vmStackErrorChecker.GetError(stackTrace);
                hasError = !string.IsNullOrEmpty(error);
                hasStackTrace = true;
                transactionProcess.TransactionVmStack = TransactionVmStack.CreateTransactionVmStack(transactionVmTable,
                    transactionHash, transactionSource.To, stackTrace);
            }

            var logs = transactionReceipt.Logs;

            transactionProcess.Transaction = Entities.Transaction.CreateTransaction(transactionTable, transactionSource,
                transactionReceipt,
                hasError, block.Timestamp, hasStackTrace, error);

            transactionProcess.AddressTransactions.Add(
                AddressTransaction.CreateAddressTransaction(adressTransactionTable, transactionSource,
                    transactionReceipt,
                    hasError, block.Timestamp, transactionSource.To, error, hasStackTrace));

            for (var i = 0; i < logs.Length; i++)
            {
                var log = logs[i] as JObject;
                if (log != null)
                {
                    var logAddress = log["address"].Value<string>();
                    if (!transactionProcess.AddressTransactions.Exists(x => x.Address == logAddress))
                    {
                        transactionProcess.AddressTransactions.Add(
                            AddressTransaction.CreateAddressTransaction(adressTransactionTable, transactionSource,
                                transactionReceipt,
                                hasError, block.Timestamp, logAddress, error, hasStackTrace));
                    }

                    transactionProcess.TransactionLogs.Add(TransactionLog.CreateTransactionLog(logTable, transactionHash,
                        i, log));
                }
            }

            return transactionProcess;
        }

        private async Task<JObject> GetTransactionVmStack(string transactionHash)
        {
            return
                await
                    web3.DebugGeth.TraceTransaction.SendRequestAsync(transactionHash,
                        new TraceTransactionOptions {DisableMemory = true, DisableStorage = true});
        }
    }
}