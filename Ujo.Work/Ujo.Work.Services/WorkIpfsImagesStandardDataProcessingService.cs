using System.Threading.Tasks;
using CCC.Contracts.StandardData.Processing;
using CCC.Contracts.StandardData.Services.Model;
using Nethereum.Web3;
using Ujo.Messaging;
using Ujo.Work.Model;

namespace Ujo.Work.Services
{
    public class WorkIpfsImagesStandardDataProcessingService : IStandardDataProcessingService<MusicRecordingDTO>
    {
        private readonly IIpfsImageQueue _ipfsImageQueue;

        public WorkIpfsImagesStandardDataProcessingService(IIpfsImageQueue ipfsImageQueue)
        {
            _ipfsImageQueue = ipfsImageQueue;
        }

        public async Task UpsertAsync(MusicRecordingDTO work)
        {
            if (!string.IsNullOrEmpty(work.Image))
            {
                _ipfsImageQueue.Add(work.Image);
            }
        }

        public async Task DataChangedAsync(MusicRecordingDTO work, EventLog<DataChangedEvent> dataEventLog)
        {
            var key = dataEventLog.Event.Key;
            var val = dataEventLog.Event.Value;

            if (key == WorkSchema.image.ToString())
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