using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect;
using Wintellect.Azure.Storage.Table;

namespace Ujo.Work.Storage
{
    public class WorkEntity : TableEntityBase
    {
        public WorkEntity(AzureTable at, DynamicTableEntity dte = null) : base(at, dte)
        {
            RowKey = string.Empty;
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

        public string DateCreated
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string DateModified
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Image
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Audio
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Keywords
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string ByArtistAddress
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string ByArtistName
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string FeaturedArtists
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string ContributingArtists
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string PerformingArtists
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Label
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Description
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Publisher
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public bool HasPartOf
        {
            get { return Get(false); }
            set { Set(value); }
        }

        public bool IsPartOf
        {
            get { return Get(false); }
            set { Set(value); }
        }

        public string IsFamilyFriendly
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string License
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string IswcCode
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public void SetUnknownKey(string value, string key)
        {
            Set(value, key);
        }


        public static WorkEntity Create(AzureTable table, Model.Work work)
        {
            var workEntity = new WorkEntity(table);
            workEntity.Initialise(work);
            return workEntity;
        }

        public void Initialise(Model.Work work)
        {
            new WorkModelToWorkEntityMapper().MapFromWorkModel(this, work);
        }

        public static async Task<WorkEntity> FindAsync(AzureTable table, string address)
        {
            var tr =
                await
                    table.ExecuteAsync(TableOperation.Retrieve(address.ToLowerInvariant().HtmlEncode(),
                        string.Empty)).ConfigureAwait(false);
            if ((HttpStatusCode) tr.HttpStatusCode != HttpStatusCode.NotFound)
                return new WorkEntity(table, (DynamicTableEntity) tr.Result);

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