using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace UjoSpike.WebJob
{
    public class ArtistEntityStorageService
    {
        private readonly CloudTable cloudTable;

        public ArtistEntityStorageService(CloudTable cloudTable)
        {
            this.cloudTable = cloudTable;
        }

        public async Task<ProcessInfo> GetProcessInfo()
        {
            var processInfo = await RetrieveSingleEntity<ProcessInfo>(ProcessInfo.PARTITION_KEY, ProcessInfo.ROW_KEY);
            if (processInfo == null) return new ProcessInfo() {Number = 0};
            return processInfo;
        }

        public async Task UpsertArtist(ArtistEntity artistEntity)
        {
            await Upsert(artistEntity);
        } 

        public async Task UpsertProcessInfo(ProcessInfo processInfo)
        {
            await Upsert(processInfo);
        }

        private async Task Upsert<T>(T entity) where T : ITableEntity
        {
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
            await cloudTable.ExecuteAsync(insertOrReplaceOperation);
        }

        private async Task<T> RetrieveSingleEntity<T>(string partitionKey, string rowKey) where  T: ITableEntity
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult retrievedResult = await cloudTable.ExecuteAsync(retrieveOperation);
            return (T)retrievedResult.Result;
        }
    }
}
