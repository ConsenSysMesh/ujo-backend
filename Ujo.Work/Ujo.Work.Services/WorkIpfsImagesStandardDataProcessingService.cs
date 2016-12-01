using System.Threading.Tasks;
using CCC.Contracts.StandardData.Processing;
using CCC.Contracts.StandardData.Services.Model;
using Nethereum.Web3;
using Ujo.Work.Model;

namespace Ujo.Work.Services
{
    public class WorkIpfsImagesStandardDataProcessingService : IStandardDataProcessingService<Model.Work>
    {
        private readonly IIpfsImageQueue _ipfsImageQueue;

        public WorkIpfsImagesStandardDataProcessingService(IIpfsImageQueue ipfsImageQueue)
        {
            _ipfsImageQueue = ipfsImageQueue;
        }

        public async Task UpsertAsync(Model.Work work)
        {
            if (!string.IsNullOrEmpty(work.Image))
            {
                _ipfsImageQueue.Add(work.Image);
            }
        }

        public async Task DataChangedAsync(Model.Work work, EventLog<DataChangedEvent> dataEventLog)
        {
            var key = dataEventLog.Event.Key;
            var val = dataEventLog.Event.Value;

            if (key == WorkSchema.Image.ToString())
            {
                _ipfsImageQueue.Add(work.Image);
            }
        }

        public async Task RemovedAsync(string contractAddress)
        {
            //nothing
        }
    }
}