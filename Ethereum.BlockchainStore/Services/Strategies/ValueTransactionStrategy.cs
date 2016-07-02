using Ethereum.BlockchainStore.Entities;
using Ethereum.BlockchainStore.Services.ValueObjects;
using Nethereum.RPC.Eth.DTOs;
using Wintellect.Azure.Storage.Table;
using Block = Nethereum.RPC.Eth.DTOs.Block;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace Ethereum.BlockchainStore.Services.Strategies
{
    public class ValueTransactionStrategy
    {
        private readonly AzureTable adressTransactionTable;
        private readonly Block block;
        private readonly TransactionReceipt transactionReceipt;
        private readonly Transaction transactionSource;
        private readonly AzureTable transactionTable;

        public ValueTransactionStrategy(Transaction transactionSource, TransactionReceipt transactionReceipt,
            Block block, AzureTable transactionTable, AzureTable adressTransactionTable)
        {
            this.transactionSource = transactionSource;
            this.transactionReceipt = transactionReceipt;
            this.block = block;
            this.transactionTable = transactionTable;
            this.adressTransactionTable = adressTransactionTable;
        }

        public TransactionProcessValueObject ProcessTransaction()
        {
            var transactionProcess = new TransactionProcessValueObject();
            //Value transaction
            transactionProcess.Transaction = Entities.Transaction.CreateTransaction(transactionTable, transactionSource,
                transactionReceipt,
                false, block.Timestamp);

            transactionProcess.AddressTransactions.Add(
                AddressTransaction.CreateAddressTransaction(adressTransactionTable, transactionSource,
                    transactionReceipt,
                    false, block.Timestamp, transactionSource.To));
            return transactionProcess;
        }
    }
}