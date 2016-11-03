using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect.Azure.Storage.Table;

namespace Ujo.Work.WebJob
{
    public class WorkRegistryProcessInfoRepository
    {
        private readonly AzureTable _processInfoTable;

        public WorkRegistryProcessInfoRepository(CloudTable table)
        {
            _processInfoTable = new AzureTable(table);
        }

        public WorkRegistryProcessInfoRepository(AzureTable table)
        {
            _processInfoTable = table;
        }

        public async Task<WorkRegistry.Storage.ProcessInfo> FindAsync()
        {
            return await WorkRegistry.Storage.ProcessInfo.FindAsync(_processInfoTable);
        }

        public WorkRegistry.Storage.ProcessInfo NewProcessInfo(long blockNumber)
        {
            return WorkRegistry.Storage.ProcessInfo.Create(_processInfoTable, blockNumber);
        }
    }
}