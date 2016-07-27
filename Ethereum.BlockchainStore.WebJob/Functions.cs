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
}
