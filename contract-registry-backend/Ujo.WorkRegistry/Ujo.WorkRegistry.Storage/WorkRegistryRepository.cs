using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Ujo.WorkRegistry.Storage;
using Wintellect.Azure.Storage.Table;

namespace Ujo.Work.WebJob
{
    public class WorkRegistryRepository
    {
        private readonly AzureTable _workRegistryTable;

        public WorkRegistryRepository(CloudTable table)
        {
            _workRegistryTable = new AzureTable(table);
        }

        public async Task<WorkRegistryRecord> FindAsync(string address)
        {
            return await WorkRegistryRecord.FindAsync(_workRegistryTable, address);
        }

        public async Task<bool> ExistsAsync(string contractAddress)
        {
            return await WorkRegistryRecord.ExistsAsync(_workRegistryTable, contractAddress);
        }
    }
}