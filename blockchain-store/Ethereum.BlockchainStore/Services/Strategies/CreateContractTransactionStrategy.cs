using System.Threading.Tasks;
using Ethereum.BlockchainStore.Entities;
using Ethereum.BlockchainStore.Services.ValueObjects;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Wintellect.Azure.Storage.Table;
using Block = Nethereum.RPC.Eth.DTOs.Block;
using Contract = Ethereum.BlockchainStore.Entities.Contract;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace Ethereum.BlockchainStore.Services.Strategies
{
    public class CreateContractTransactionStrategy
    {
        private readonly AzureTable adressTransactionTable;
        private readonly Block block;
        private readonly AzureTable contractTable;
        private readonly TransactionReceipt transactionReceipt;
        private readonly Transaction transactionSource;
        private readonly AzureTable transactionTable;
        private readonly Web3 web3;

        public CreateContractTransactionStrategy(Transaction transactionSource, TransactionReceipt transactionReceipt,
            Block block, Web3 web3, AzureTable contractTable, AzureTable transactionTable,
            AzureTable adressTransactionTable)
        {
            this.transactionSource = transactionSource;
            this.transactionReceipt = transactionReceipt;
            this.block = block;
            this.web3 = web3;
            this.contractTable = contractTable;
            this.transactionTable = transactionTable;
            this.adressTransactionTable = adressTransactionTable;
        }

        public async Task<TransactionProcessValueObject> ProcessTransaction()
        {
            if (IsTransactionType())
            {
                var transactionProcess = new TransactionProcessValueObject();
                var contractAddress = GetContractAddress();
                var code = await GetCode(contractAddress).ConfigureAwait(false);
                var failedCreatingContract = HasFailedToCreateContract(code);

                if (!failedCreatingContract)
                {
                    transactionProcess.Contract = Contract.CreateContract(contractTable, contractAddress, code,
                        transactionSource);
                }

                transactionProcess.Transaction = Entities.Transaction.CreateTransaction(transactionTable,
                    transactionSource, transactionReceipt,
                    failedCreatingContract, block.Timestamp, contractAddress);

                transactionProcess.AddressTransactions.Add(
                    AddressTransaction.CreateAddressTransaction(adressTransactionTable, transactionSource,
                        transactionReceipt,
                        failedCreatingContract, block.Timestamp, null, null, false, contractAddress));
                return transactionProcess;
            }
            return null;
        }


        public async Task<string> GetCode(string contractAddres)
        {
            return await web3.Eth.GetCode.SendRequestAsync(contractAddres).ConfigureAwait(false);
        }

        public bool HasFailedToCreateContract(string code)
        {
            return code == null || code == "0x";
        }

        public bool IsTransactionType()
        {
            return string.IsNullOrEmpty(transactionSource.To) && !string.IsNullOrEmpty(GetContractAddress());
        }

        private string GetContractAddress()
        {
            return transactionReceipt.ContractAddress;
        }
    }
}