using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect.Azure.Storage.Table;

namespace Ujo.Work.Storage
{
    public class WorkProcessInfoRepository
    {
        private readonly AzureTable _processInfoTable;

        public WorkProcessInfoRepository(CloudTable table)
        {
            _processInfoTable = new AzureTable(table);
        }

        public WorkProcessInfoRepository(AzureTable table)
        {
            _processInfoTable = table;
        }

        public async Task<ProcessInfo> FindAsync()
        {
            return await ProcessInfo.FindAsync(_processInfoTable);
        }

        public ProcessInfo NewProcessInfo(long blockNumber)
        {
            return ProcessInfo.Create(_processInfoTable, blockNumber);
        }
    }
}