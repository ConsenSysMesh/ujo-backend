using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Wintellect.Azure.Storage.Table;
using Block = Ethereum.BlockchainStore.Entities.Block;

namespace Ethereum.BlockchainStore.Services
{
    public class BlockProcessorService
    {
        private readonly Web3 web3;
        private readonly AzureTable blockTable;
        private readonly TransactionProcessorService transactionProcessor;


        public BlockProcessorService(Web3 web3, CloudTable transactionCloudTable,
            CloudTable addressTransactionCloudTable, CloudTable contractCloudTable, CloudTable blockCloudTable,
            CloudTable transactionLogCloudTable, CloudTable transactionVmCloudTable)
        {
            this.web3 = web3;
            blockTable = new AzureTable(blockCloudTable);
            transactionProcessor = new TransactionProcessorService(web3, transactionCloudTable,
                addressTransactionCloudTable, contractCloudTable, transactionLogCloudTable, transactionVmCloudTable);
        }

        public async Task ProcessBlock(long blockNumber)
        {
            var block =
                await
                    web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(
                        new HexBigInteger(blockNumber));

            var blockEntity = MapBlock(block, new Block(blockTable));

            await blockEntity.InsertOrReplaceAsync();

            foreach (var txnHash in block.TransactionHashes)
            {
                var tran = await transactionProcessor.ProcessTransaction(txnHash, block);
                await tran.SaveAllAsync();
            }
        }

        public Block MapBlock(BlockWithTransactionHashes blockSource, Block blockOutput)
        {
            //13 properties
            blockOutput.SetBlockNumber(blockSource.Number);
            blockOutput.SetDifficulty(blockSource.Difficulty);
            blockOutput.SetGasLimit(blockSource.GasLimit);
            blockOutput.SetGasUsed(blockSource.GasUsed);
            blockOutput.SetSize(blockSource.Size);
            blockOutput.SetTimeStamp(blockSource.Timestamp);
            blockOutput.SetTotalDifficulty(blockSource.TotalDifficulty);
            blockOutput.ExtraData = blockSource.ExtraData ?? string.Empty;
            blockOutput.Hash = blockSource.BlockHash ?? string.Empty;
            blockOutput.ParentHash = blockSource.ParentHash ?? string.Empty;
            blockOutput.Miner = blockOutput.Miner ?? string.Empty;
            blockOutput.Nonce = blockOutput.Nonce ?? string.Empty;
            blockOutput.TransactionCount = blockSource.TransactionHashes.Length;

            return blockOutput;
        }
    }
}