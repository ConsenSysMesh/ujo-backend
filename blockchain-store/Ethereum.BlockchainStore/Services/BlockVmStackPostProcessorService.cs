using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Ethereum.BlockchainStore.Services
{
    public class BlockVmStackPostProcessorService: BlockProcessorService
    {
        public BlockVmStackPostProcessorService(Web3 web3, CloudTable transactionCloudTable, CloudTable addressTransactionCloudTable, CloudTable contractCloudTable, CloudTable blockCloudTable, CloudTable transactionLogCloudTable, CloudTable transactionVmCloudTable) : base(web3, transactionCloudTable, addressTransactionCloudTable, contractCloudTable, blockCloudTable, transactionLogCloudTable, transactionVmCloudTable)
        {
            this.TransactionProcessor = new TransactionVmStackPostProcessorService(web3, transactionCloudTable, addressTransactionCloudTable, contractCloudTable, transactionLogCloudTable, transactionVmCloudTable);
        }

        public override async Task ProcessBlock(long blockNumber)
        {
            var block =
                await
                    Web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(
                        new HexBigInteger(blockNumber)).ConfigureAwait(false);

            foreach (var txnHash in block.TransactionHashes)
            {
                var tran = await TransactionProcessor.ProcessTransaction(txnHash, block).ConfigureAwait(false);
                await tran.SaveAllAsync().ConfigureAwait(false);
            }
        }

    }
}