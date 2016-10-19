using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect;
using Wintellect.Azure.Storage.Table;

namespace Ujo.Work.Storage
{
    public class WorkRepository
    {
        //TODO: create repository wrapper
        //public Task<>
    }

    public class WorkModelToWorkEntityMapper
    {
        public WorkEntity MapFromWorkModel(WorkEntity workEntity, Model.Work work)
        {
            workEntity.Address = work.Address ?? "";
            workEntity.Name = work.Name ?? "";
            workEntity.Creator = work.Creator ?? "";
            workEntity.WorkFileIpfsHash = work.WorkFileIpfsHash ?? "";
            workEntity.CoverFileIpfsHash = work.CoverImageIpfsHash ?? "";
            workEntity.DateCreated = work.DateCreated ?? "";
            workEntity.DateModified = work.DateModified ?? "";
            workEntity.Image = work.Image ?? "";
            workEntity.Audio = work.Audio ?? "";
            workEntity.Genre = work.Genre ?? "";
            workEntity.Keywords = work.Keywords ?? "";
            workEntity.ByArtist = work.ByArtist ?? "";
            workEntity.FeaturedArtist1 = work.FeaturedArtist1 ?? "";
            workEntity.FeaturedArtist2 = work.FeaturedArtist2 ?? "";
            workEntity.FeaturedArtist3 = work.FeaturedArtist3 ?? "";
            workEntity.FeaturedArtist4 = work.FeaturedArtist4 ?? "";
            workEntity.FeaturedArtist5 = work.FeaturedArtist5 ?? "";
            workEntity.FeaturedArtist6 = work.FeaturedArtist6 ?? "";
            workEntity.FeaturedArtist7 = work.FeaturedArtist7 ?? "";
            workEntity.FeaturedArtist8 = work.FeaturedArtist8 ?? "";
            workEntity.FeaturedArtist9 = work.FeaturedArtist9 ?? "";
            workEntity.FeaturedArtist10 = work.FeaturedArtist10 ?? "";
            workEntity.FeaturedArtistRole1 = work.FeaturedArtistRole1 ?? "";
            workEntity.FeaturedArtistRole2 = work.FeaturedArtistRole2 ?? "";
            workEntity.FeaturedArtistRole3 = work.FeaturedArtistRole3 ?? "";
            workEntity.FeaturedArtistRole4 = work.FeaturedArtistRole4 ?? "";
            workEntity.FeaturedArtistRole5 = work.FeaturedArtistRole5 ?? "";
            workEntity.FeaturedArtistRole6 = work.FeaturedArtistRole6 ?? "";
            workEntity.FeaturedArtistRole7 = work.FeaturedArtistRole7 ?? "";
            workEntity.FeaturedArtistRole8 = work.FeaturedArtistRole8 ?? "";
            workEntity.FeaturedArtistRole9 = work.FeaturedArtistRole9 ?? "";
            workEntity.FeaturedArtistRole10 = work.FeaturedArtistRole10 ?? "";
            workEntity.ContributingArtist1 = work.ContributingArtist1 ?? "";
            workEntity.ContributingArtist2 = work.ContributingArtist2 ?? "";
            workEntity.ContributingArtist3 = work.ContributingArtist3 ?? "";
            workEntity.ContributingArtist4 = work.ContributingArtist4 ?? "";
            workEntity.ContributingArtist5 = work.ContributingArtist5 ?? "";
            workEntity.ContributingArtist6 = work.ContributingArtist6 ?? "";
            workEntity.ContributingArtist7 = work.ContributingArtist7 ?? "";
            workEntity.ContributingArtist8 = work.ContributingArtist8 ?? "";
            workEntity.ContributingArtist9 = work.ContributingArtist9 ?? "";
            workEntity.ContributingArtist10 = work.ContributingArtist10 ?? "";
            workEntity.ContributingArtistRole1 = work.ContributingArtistRole1 ?? "";
            workEntity.ContributingArtistRole2 = work.ContributingArtistRole2 ?? "";
            workEntity.ContributingArtistRole3 = work.ContributingArtistRole3 ?? "";
            workEntity.ContributingArtistRole4 = work.ContributingArtistRole4 ?? "";
            workEntity.ContributingArtistRole5 = work.ContributingArtistRole5 ?? "";
            workEntity.ContributingArtistRole6 = work.ContributingArtistRole6 ?? "";
            workEntity.ContributingArtistRole7 = work.ContributingArtistRole7 ?? "";
            workEntity.ContributingArtistRole8 = work.ContributingArtistRole8 ?? "";
            workEntity.ContributingArtistRole9 = work.ContributingArtistRole9 ?? "";
            workEntity.ContributingArtistRole10 = work.ContributingArtistRole10 ?? "";
            workEntity.PerformingArtist1 = work.PerformingArtist1 ?? "";
            workEntity.PerformingArtist2 = work.PerformingArtist2 ?? "";
            workEntity.PerformingArtist3 = work.PerformingArtist3 ?? "";
            workEntity.PerformingArtist4 = work.PerformingArtist4 ?? "";
            workEntity.PerformingArtist5 = work.PerformingArtist5 ?? "";
            workEntity.PerformingArtist6 = work.PerformingArtist6 ?? "";
            workEntity.PerformingArtist7 = work.PerformingArtist7 ?? "";
            workEntity.PerformingArtist8 = work.PerformingArtist8 ?? "";
            workEntity.PerformingArtist9 = work.PerformingArtist9 ?? "";
            workEntity.PerformingArtist10 = work.PerformingArtist10 ?? "";
            workEntity.PerformingArtistRole1 = work.PerformingArtistRole1 ?? "";
            workEntity.PerformingArtistRole2 = work.PerformingArtistRole2 ?? "";
            workEntity.PerformingArtistRole3 = work.PerformingArtistRole3 ?? "";
            workEntity.PerformingArtistRole4 = work.PerformingArtistRole4 ?? "";
            workEntity.PerformingArtistRole5 = work.PerformingArtistRole5 ?? "";
            workEntity.PerformingArtistRole6 = work.PerformingArtistRole6 ?? "";
            workEntity.PerformingArtistRole7 = work.PerformingArtistRole7 ?? "";
            workEntity.PerformingArtistRole8 = work.PerformingArtistRole8 ?? "";
            workEntity.PerformingArtistRole9 = work.PerformingArtistRole9 ?? "";
            workEntity.PerformingArtistRole10 = work.PerformingArtistRole10 ?? "";
            workEntity.Label = work.Label ?? "";
            workEntity.Description = work.Description ?? "";
            workEntity.Publisher = work.Publisher ?? "";
            workEntity.HasPartOf = work.HasPartOf;
            workEntity.IsPartOf = work.IsPartOf;
            workEntity.IsFamilyFriendly = work.IsFamilyFriendly;
            workEntity.License = work.License ?? "";
            workEntity.IswcCode = work.IswcCode ?? "";

            return workEntity;
        }
    }
    public class WorkEntity : TableEntityBase
    {
        public WorkEntity(AzureTable at, DynamicTableEntity dte = null) : base(at, dte)
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

        public string DateCreated
        {
            get
            {
                return Get(string.Empty);
            }
            set
            {
                Set(value);
            }
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
        public string ByArtist
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist1
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist2
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist3
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist4
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist5
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist6
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist7
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist8
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist9
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtist10
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole1
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole2
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole3
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole4
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole5
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole6
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole7
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole8
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole9
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string FeaturedArtistRole10
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist1
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist2
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist3
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist4
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist5
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist6
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist7
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist8
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist9
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtist10
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole1
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole2
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole3
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole4
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole5
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole6
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole7
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole8
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole9
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string ContributingArtistRole10
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist1
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist2
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist3
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist4
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist5
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist6
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist7
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist8
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist9
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtist10
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole1
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole2
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole3
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole4
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole5
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole6
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole7
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole8
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole9
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }
        public string PerformingArtistRole10
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
            if ((HttpStatusCode)tr.HttpStatusCode != HttpStatusCode.NotFound)
                return new WorkEntity(table, (DynamicTableEntity)tr.Result);

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