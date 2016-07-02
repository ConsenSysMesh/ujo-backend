using System.Collections.Generic;
using System.Threading.Tasks;
using Ethereum.BlockchainStore.Entities;
using Nethereum.RPC.Eth.DTOs;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace Ethereum.BlockchainStore.Services.ValueObjects
{
    public class TransactionProcessValueObject
    {
        public TransactionProcessValueObject()
        {
            AddressTransactions = new List<AddressTransaction>();
            TransactionLogs = new List<TransactionLog>();
        }

        public TransactionReceipt TransactionReceipt { get; set; }
        public Transaction TransactionSource { get; set; }

        public Entities.Transaction Transaction { get; set; }

        public List<AddressTransaction> AddressTransactions { get; set; }

        public List<TransactionLog> TransactionLogs { get; set; }

        public Contract Contract { get; set; }

        public TransactionVmStack TransactionVmStack { get; set; }

        public async Task SaveAllAsync()
        {
            if (Transaction != null)
            {
                await Transaction.InsertOrReplaceAsync();
            }

            if (Contract != null)
            {
                await Contract.InsertOrReplaceAsync();
            }

            foreach (var addressTransaction in AddressTransactions)
            {
                await addressTransaction.InsertOrReplaceAsync();
            }

            foreach (var transactionLog in TransactionLogs)
            {
                await transactionLog.InsertOrReplaceAsync();
            }

            if (TransactionVmStack != null)
            {
                await TransactionVmStack.InsertOrReplaceAsync();
            }
        }
    }
}