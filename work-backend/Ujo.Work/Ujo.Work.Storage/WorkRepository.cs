using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect.Azure.Storage.Table;

namespace Ujo.Work.Storage
{
    public class WorkRepository
    {
        private readonly AzureTable _worksTable;

        public WorkRepository(CloudTable table)
        {
            _worksTable = new AzureTable(table);
        }

        public WorkRepository(AzureTable table)
        {
            _worksTable = table;
        }

        public async Task<WorkEntity> FindAsync(string address)
        {
            return await WorkEntity.FindAsync(_worksTable, address);
        }

        public WorkEntity NewWork(Model.Work work)
        {
            var workEntity = new WorkEntity(_worksTable);
            workEntity.Initialise(work);
            return workEntity;
        }

        public async Task<bool> ExistsAsync(string contractAddress)
        {
            return await WorkEntity.ExistsAsync(_worksTable, contractAddress);
        }
    }
}