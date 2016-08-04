using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;

namespace Ethereum.BlockchainStore.WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log)
        {
            log.WriteLine(message);
        }
        
    }

   
    public class BlockchainStoreProcessService
    {
        private readonly CloudTable cloudTable;

        public class ProcessInfo : TableEntity
        {
            public const string PARTITION_KEY = "ProcessInfo";
            public const string ROW_KEY = "Index";
            public ProcessInfo()
            {
                this.PartitionKey = PARTITION_KEY;
                this.RowKey = ROW_KEY;
            }

            public long Number { get; set; }

        }

        public BlockchainStoreProcessService(CloudTable cloudTable)
        {
            this.cloudTable = cloudTable;
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

        public async Task<ProcessInfo> GetProcessInfo()
        {
            var processInfo = await RetrieveSingleEntity<ProcessInfo>(ProcessInfo.PARTITION_KEY, ProcessInfo.ROW_KEY);
            if (processInfo == null) return new ProcessInfo() { Number = 0 };
            return processInfo;
        }

        private async Task<T> RetrieveSingleEntity<T>(string partitionKey, string rowKey) where T : ITableEntity
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult retrievedResult = await cloudTable.ExecuteAsync(retrieveOperation);
            return (T)retrievedResult.Result;
        }
    }
}
