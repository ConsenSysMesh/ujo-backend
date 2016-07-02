using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Wintellect;
using Wintellect.Azure.Storage.Table;

namespace Ethereum.BlockchainStore.Entities
{
    public class Contract : TableEntityBase
    {
        public Contract(AzureTable at, DynamicTableEntity dte = null) : base(at, dte)
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

        public string ABI
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Code
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string Creator
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public string TransactionHash
        {
            get { return Get(string.Empty); }
            set { Set(value); }
        }

        public static Contract CreateContract(AzureTable contractTable, string contractAddress, string code,
            Nethereum.RPC.Eth.DTOs.Transaction transactionSource)
        {
            var contract = new Contract(contractTable)
            {
                Address = contractAddress,
                Code = code,
                Creator = transactionSource.From,
                TransactionHash = transactionSource.TransactionHash
            };
            return contract;
        }

        public static async Task<Contract> FindAsync(AzureTable table, string contractAddress)
        {
            var tr =
                await
                    table.ExecuteAsync(TableOperation.Retrieve(contractAddress.ToLowerInvariant().HtmlEncode(),
                        string.Empty)).ConfigureAwait(false);
            if ((HttpStatusCode) tr.HttpStatusCode != HttpStatusCode.NotFound)
                return new Contract(table, (DynamicTableEntity) tr.Result);

            return null;
        }

        public static async Task<bool> ExistsAsync(AzureTable table, string contractAddress)
        {
            var contract = await FindAsync(table, contractAddress);
            if (contract != null) return true;
            return false;
        }
    }
}