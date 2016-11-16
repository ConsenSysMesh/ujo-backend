using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect;
using Wintellect.Azure.Storage.Table;

namespace Ujo.IpfsImage.Storage
{
    public class IpfsImageResized : TableEntityBase
    {
        public IpfsImageResized(AzureTable at, DynamicTableEntity dte = null) : base(at, dte)
        {
            RowKey = string.Empty;
        }

        public string IpfsImageHash
        {
            get { return Get(string.Empty); }
            set
            {
                PartitionKey = value.ToLowerInvariant().HtmlEncode();
                Set(value);
            }
        }

        public string NewSize
        {
            get { return Get(string.Empty); }
            set
            {
                RowKey = value.ToLowerInvariant().HtmlEncode();
                Set(value);
            }
        }

        public string IpfsImageNewSizeHash
        {
            get { return Get(string.Empty); }
            set
            {
                Set(value);
            }
        }

        public static IpfsImageResized Create(AzureTable table, string ipfsImageHash, string newSize, string ipfsImageNewSizeHash
            )
        {
            var ipfsImageResized = new IpfsImageResized(table)
            {
                IpfsImageHash = ipfsImageHash,
                NewSize = newSize,
                IpfsImageNewSizeHash = ipfsImageNewSizeHash       
            };
            return ipfsImageResized;
        }

        public static string GetHeightNewSizeKey(int height)
        {
            return "h" + height;
        }

        public static string GetWidthNewSizeKey(int width)
        {
            return "w" + width;
        }

        public static string GetCropNewSizeKey(int width, int height)
        {
            return "c" + width + "x" + height;
        }

        public new static async Task<IpfsImageResized>  FindAsync(AzureTable table, string ipfsHash, string newSize)
        {
            var tr =
                await
                    table.ExecuteAsync(TableOperation.Retrieve(ipfsHash.ToLowerInvariant().HtmlEncode(),
                        newSize.ToLowerInvariant())).ConfigureAwait(false);
            if ((HttpStatusCode)tr.HttpStatusCode != HttpStatusCode.NotFound)
                return new IpfsImageResized(table, (DynamicTableEntity)tr.Result);
            return null;
        }

        public static async Task<List<IpfsImageResized>> FindAsync(AzureTable table, string ipfsHash)
        {
            var tableQuery = new TableQuery
            {
                FilterString = new TableFilterBuilder<IpfsImageResized>().And(te => te.PartitionKey, CompareOp.EQ, ipfsHash.ToLowerInvariant())
            };

            var results = new List<IpfsImageResized>();
            for (var chunker = table.CreateQueryChunker(tableQuery); chunker.HasMore;)
            {
                results.AddRange(from entity in await chunker.TakeAsync() select new IpfsImageResized(table, entity));
            }
            return results;
        }

        public static async Task<bool> ExistsAsync(AzureTable table, string contractAddress)
        {
            var contract = await FindAsync(table, contractAddress).ConfigureAwait(false);
            if (contract != null) return true;
            return false;
        }
    }
}