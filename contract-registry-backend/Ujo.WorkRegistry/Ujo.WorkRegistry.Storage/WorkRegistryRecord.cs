using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect;
using Wintellect.Azure.Storage.Table;

namespace Ujo.WorkRegistry.Storage
{
    public class WorkRegistryRecord : TableEntityBase
    {
        public WorkRegistryRecord(AzureTable at, DynamicTableEntity dte = null) : base(at, dte)
        {
            RowKey = string.Empty;
        }

        public string RegisteredAddress
        {
            get { return Get(string.Empty); }
            set
            {
                PartitionKey = value.ToLowerInvariant().HtmlEncode();
                Set(value);
            }
        }

        public string Owner
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public long Time
        {
            get { return Get(0); }
            set { Set(value); }
        }

        public long Id
        {
            get { return Get(0); }
            set { Set(value); }
        }

            
        public static WorkRegistryRecord Create(AzureTable registryRecordTable, string registeredAddress, string owner, long time, long id
            )
        {
            var workRegistry = new WorkRegistryRecord(registryRecordTable)
            {
                RegisteredAddress = registeredAddress,
                Owner = owner,
                Time = time,
                Id = id
            };
               

            return workRegistry;
        }

        public static async Task<WorkRegistryRecord> FindAsync(AzureTable table, string registeredAddress)
        {
               
            var tr =
                await
                    table.ExecuteAsync(TableOperation.Retrieve(registeredAddress.ToLowerInvariant().HtmlEncode(),
                        string.Empty)).ConfigureAwait(false);
            if ((HttpStatusCode)tr.HttpStatusCode != HttpStatusCode.NotFound)
                return new WorkRegistryRecord(table, (DynamicTableEntity)tr.Result);

            return null;
        }

        public static async Task<bool> ExistsAsync(AzureTable table, string contractAddress)
        {
            var contract = await FindAsync(table, contractAddress).ConfigureAwait(false);
            if (contract != null) return true;
            return false;
        }
    }
}