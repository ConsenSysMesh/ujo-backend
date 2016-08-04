using System;
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
        protected  AzureTable AdressTransactionTable {get;  set;}
        protected  Block Block {get;  set;}
        protected  AzureTable ContractTable {get;  set;}
        protected  AzureTable LogTable {get;  set;}
        protected  TransactionReceipt TransactionReceipt {get;  set;}
        protected  Transaction TransactionSource {get;  set;}
        protected  AzureTable TransactionTable {get;  set;}
        protected  AzureTable TransactionVmTable {get;  set;}
        protected  VmStackErrorChecker VmStackErrorChecker {get;  set;}
        protected  Web3 Web3 {get;  set;}

        public ContractTransactionStrategy(Transaction transactionSource, TransactionReceipt transactionReceipt,
            Block block, Web3 web3, AzureTable contractTable, AzureTable transactionTable,
            AzureTable adressTransactionTable, AzureTable logTable, AzureTable transactionVmTable)
        {
            this.TransactionSource = transactionSource;
            this.TransactionReceipt = transactionReceipt;
            this.Block = block;
            this.Web3 = web3;
            this.ContractTable = contractTable;
            this.TransactionTable = transactionTable;
            this.AdressTransactionTable = adressTransactionTable;
            this.LogTable = logTable;
            this.TransactionVmTable = transactionVmTable;
            VmStackErrorChecker = new VmStackErrorChecker();
        }


        public async Task<bool> IsTransactionType()
        {
            if (TransactionSource.To == null) return false;
            return await Contract.ExistsAsync(ContractTable, TransactionSource.To).ConfigureAwait(false);
        }

        public virtual async Task<TransactionProcessValueObject> ProcessTransaction()
        {
            var transactionProcess = new TransactionProcessValueObject();
            var transactionHash = TransactionSource.TransactionHash;
            var hasStackTrace = false;
            JObject stackTrace = null;
            var error = string.Empty;
            var hasError = false;

            try
            {
                stackTrace = await GetTransactionVmStack(transactionHash).ConfigureAwait(false);
                
            }
            catch (Exception ex)
            {
                if (TransactionSource.Gas == TransactionReceipt.GasUsed)
                {
                    hasError = true;
                }
            }
            
            if (stackTrace != null)
            {
                error = VmStackErrorChecker.GetError(stackTrace);
                hasError = !string.IsNullOrEmpty(error);
                hasStackTrace = true;
                transactionProcess.TransactionVmStack = TransactionVmStack.CreateTransactionVmStack(TransactionVmTable,
                    transactionHash, TransactionSource.To, stackTrace);
            }

            var logs = TransactionReceipt.Logs;

            transactionProcess.Transaction = Entities.Transaction.CreateTransaction(TransactionTable, TransactionSource,
                TransactionReceipt,
                hasError, Block.Timestamp, hasStackTrace, error);

            transactionProcess.AddressTransactions.Add(
                AddressTransaction.CreateAddressTransaction(AdressTransactionTable, TransactionSource,
                    TransactionReceipt,
                    hasError, Block.Timestamp, TransactionSource.To, error, hasStackTrace));

            for (var i = 0; i < logs.Count; i++)
            {
                var log = logs[i] as JObject;
                if (log != null)
                {
                    var logAddress = log["address"].Value<string>();
                    if (!transactionProcess.AddressTransactions.Exists(x => x.Address == logAddress))
                    {
                        transactionProcess.AddressTransactions.Add(
                            AddressTransaction.CreateAddressTransaction(AdressTransactionTable, TransactionSource,
                                TransactionReceipt,
                                hasError, Block.Timestamp, logAddress, error, hasStackTrace));
                    }

                    transactionProcess.TransactionLogs.Add(TransactionLog.CreateTransactionLog(LogTable, transactionHash,
                        i, log));
                }
            }

            return transactionProcess;
        }

        protected async Task<JObject> GetTransactionVmStack(string transactionHash)
        {
            return
                await
                    Web3.DebugGeth.TraceTransaction.SendRequestAsync(transactionHash,
                        new TraceTransactionOptions {DisableMemory = true, DisableStorage = true, DisableStack = true});
        }
    }
}