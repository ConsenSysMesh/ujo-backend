using System.Threading.Tasks;
using Nethereum.Web3;
using Ujo.Work.Model;
using Ujo.Work.Services.Ethereum;

namespace Ujo.Work.Services
{
    public class WorkIpfsImagesStandardDataChangedService : IStandardDataChangedService<Model.Work>
    {
        private readonly IIpfsImageQueue _ipfsImageQueue;

        public WorkIpfsImagesStandardDataChangedService(IIpfsImageQueue ipfsImageQueue)
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

        public async Task DataChangedAsync(Model.Work work, EventLog<CCC.StandardDataProcessing.DataChangedEvent> dataEventLog)
        {
            var key = dataEventLog.Event.Key;
            var val = dataEventLog.Event.Value;

            if (key == WorkSchema.Image.ToString())
            {
                _ipfsImageQueue.Add(work.CoverImageIpfsHash);
            }
        }

        public async Task RemovedAsync(string contractAddress)
        {
            //nothing
        }
    }
}