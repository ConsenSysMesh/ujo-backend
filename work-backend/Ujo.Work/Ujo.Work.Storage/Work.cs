using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect;
using Wintellect.Azure.Storage.Table;

namespace Ujo.Work.Storage
{
    public class Work : TableEntityBase
    {
        public Work(AzureTable at, DynamicTableEntity dte = null) : base(at, dte)
        {
            RowKey = string.Empty;
        }

        public void SetUnknownKey(string value, string key)
        {
            this.Set(value, key);
        }

        public string Address
        {
            get { return Get(string.Empty); }
            set
            {
                PartitionKey = value.ToLowerInvariant().HtmlEncode();
                Set(value);
            }
        }

        public string Name
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Creator
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Genre
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string CoverFileIpfsHash
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string WorkFileIpfsHash
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public static Work Create(AzureTable table, string address, string name, string creator, string genre, string workFileIpfsHash, string coverFileIpfsHash
            )
        {
            var workRegistry = new Work(table)
            {
                Address = address ?? "",
                Name = name ?? "",
                Creator = creator ?? "",
                WorkFileIpfsHash = workFileIpfsHash ?? "",
                CoverFileIpfsHash = coverFileIpfsHash ?? "",
                Genre = genre ?? ""
            };

            return workRegistry;
        }

        public static async Task<Work> FindAsync(AzureTable table, string address)
        {

            var tr =
                await
                    table.ExecuteAsync(TableOperation.Retrieve(address.ToLowerInvariant().HtmlEncode(),
                        string.Empty)).ConfigureAwait(false);
            if ((HttpStatusCode)tr.HttpStatusCode != HttpStatusCode.NotFound)
                return new Work(table, (DynamicTableEntity)tr.Result);

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