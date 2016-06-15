using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace UjoSpike.WebJob
{
    public class ArtistEntity:TableEntity
    {
        public string Category
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        public long Id
        {
            get { return Convert.ToInt64(RowKey); }
            set { RowKey = value.ToString(); }
        }

        public string Name { get; set; }
        public bool IsGroup { get; set; }
    }

    public class ProcessInfo : TableEntity
    {
        public const string PARTITION_KEY = "Artist_ProcessInfo";
        public const string ROW_KEY = "Index";
        public ProcessInfo()
        {
            this.PartitionKey = PARTITION_KEY;
            this.RowKey = ROW_KEY;
        }

        public long Number { get; set; }
      
    }
}