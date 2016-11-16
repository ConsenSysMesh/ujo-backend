using System.Threading.Tasks;
using CCC.Contracts.Registry.Processing;
using CCC.Contracts.Registry.Services;
using CCC.Contracts.StandardData.Processing;
using Microsoft.WindowsAzure.Storage.Table;
using Nethereum.Web3;
using Wintellect.Azure.Storage.Table;

namespace Ujo.WorkRegistry.Storage
{
    public class WorkRegistryRepository : IStandardDataRegistry, IRegistryProcessingService
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

        public async Task ProcessRegistered(EventLog<RegisteredEvent> registeredEvent)
        {
            var workRegistryRecord = WorkRegistryRecord.Create(_workRegistryTable,
                registeredEvent.Event.RegisteredAddress,
                registeredEvent.Event.Owner,
                registeredEvent.Event.Time,
                registeredEvent.Event.Id);
            await workRegistryRecord.InsertOrReplaceAsync();
        }

        public async Task ProcessUnregistered(EventLog<UnregisteredEvent> unregisteredEvent)
        {
            var workRegistryRecord = await FindAsync(unregisteredEvent.Event.RegisteredAddress);
            if (workRegistryRecord != null)
            {
                await workRegistryRecord.DeleteAsync();
            }
        }
    }
}