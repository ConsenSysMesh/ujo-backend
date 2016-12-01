using System;
using System.Threading.Tasks;
using CCC.BlockchainProcessing;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect.Azure.Storage.Table;

namespace Ujo.WorkRegistry.Storage
{
    public class ProcessInfoRepository: IBlockProcessProgressRepository
    {
        private readonly AzureTable _processInfoTable;

        public ProcessInfoRepository(CloudTable table)
        {
            _processInfoTable = new AzureTable(table);
        }

        public ProcessInfoRepository(AzureTable table)
        {
            _processInfoTable = table;
        }

        public async Task<ProcessInfo> FindAsync()
        {
            return await ProcessInfo.FindAsync(_processInfoTable);
        }

        public ProcessInfo NewProcessInfo(ulong blockNumber)
        {
            return ProcessInfo.Create(_processInfoTable, blockNumber);
        }

        public async Task UpsertBlockNumberProcessedTo(ulong blockNumber)
        {
            var processInfo = NewProcessInfo(blockNumber);
            await processInfo.InsertOrMergeAsync();
        }

        public async Task UpsertProgressAsync(ulong blockNumber)
        {
            var processInfo = NewProcessInfo(blockNumber);
            await processInfo.InsertOrMergeAsync();
        }

        public async Task<ulong?> GetLatestAsync()
        {
            var processInfo = await FindAsync();
            if (processInfo == null) return null;
            return Convert.ToUInt64(processInfo.Number);
        }
    }
}