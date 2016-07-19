using System;
using System.Threading.Tasks;
using Ethereum.BlockchainStore.Entities;
using Ethereum.BlockchainStore.Services.ValueObjects;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using Wintellect.Azure.Storage.Table;
using Block = Nethereum.RPC.Eth.DTOs.Block;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace Ethereum.BlockchainStore.Services.Strategies
{
    public class ContractVmStackPostProcessTransactionStrategy : ContractTransactionStrategy
    {
        public ContractVmStackPostProcessTransactionStrategy(Transaction transactionSource, TransactionReceipt transactionReceipt, Block block, Web3 web3, AzureTable contractTable, AzureTable transactionTable, AzureTable adressTransactionTable, AzureTable logTable, AzureTable transactionVmTable) : base(transactionSource, transactionReceipt, block, web3, contractTable, transactionTable, adressTransactionTable, logTable, transactionVmTable)
        {
        }

        public override async Task<TransactionProcessValueObject> ProcessTransaction()
        {
            var transactionProcess = new TransactionProcessValueObject();
            var transactionHash = TransactionSource.TransactionHash;
          
            JObject stackTrace = null;

            try
            {
                stackTrace = await GetTransactionVmStack(transactionHash).ConfigureAwait(false);
            }
            catch
            {
                Console.WriteLine("ERROR: STACK" + transactionHash);   
            }

            if (stackTrace != null)
            {
                var error = VmStackErrorChecker.GetError(stackTrace);
                var hasError = !string.IsNullOrEmpty(error);
              
                transactionProcess.TransactionVmStack = TransactionVmStack.CreateTransactionVmStack(TransactionVmTable,
                    transactionHash, TransactionSource.To, stackTrace);

                transactionProcess.Transaction = Entities.Transaction.CreateTransaction(TransactionTable, TransactionSource,
                    TransactionReceipt,
                    hasError, Block.Timestamp, true, error);

                transactionProcess.AddressTransactions.Add(
                    AddressTransaction.CreateAddressTransaction(AdressTransactionTable, TransactionSource,
                        TransactionReceipt,
                        hasError, Block.Timestamp, TransactionSource.To, error, true));

                var logs = TransactionReceipt.Logs;

                for (var i = 0; i < logs.Length; i++)
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
                                    hasError, Block.Timestamp, logAddress, error, true));
                        }
                    }
                }
            }

            return transactionProcess;
        }
    }
}