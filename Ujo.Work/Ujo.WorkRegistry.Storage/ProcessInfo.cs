using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using NBitcoin.BouncyCastle.Math.EC;
using Wintellect;
using Wintellect.Azure.Storage.Table;

namespace Ujo.WorkRegistry.Storage
{
    public class ProcessInfo : TableEntityBase
    {
        private const string PARTITION_KEY = "processinfo";

        public ProcessInfo(AzureTable at, DynamicTableEntity dte = null) : base(at, dte)
        {
            RowKey = GetRowKey();
            PartitionKey = GetPartitionKey();
        }

        public string Number
        {
            get { return Get("0"); }
            set { Set(value); }
        }

        public static string GetPartitionKey()
        {
            return PARTITION_KEY.ToLowerInvariant().HtmlEncode();
        }

        public static string GetRowKey()
        {
            return string.Empty;
        }

        public static async Task<ProcessInfo> FindAsync(AzureTable table)
        {
            var tr =
                await
                    table.ExecuteAsync(TableOperation.Retrieve(GetPartitionKey(),
                        GetRowKey())).ConfigureAwait(false);
            if ((HttpStatusCode) tr.HttpStatusCode != HttpStatusCode.NotFound)
                return new ProcessInfo(table, (DynamicTableEntity) tr.Result);

            return null;
        }

        public static ProcessInfo Create(AzureTable processInfoTable, ulong number)
        {
            var processInfo = new ProcessInfo(processInfoTable)
            {
                Number = number.ToString()
            };
            return processInfo;
        }
    }
}