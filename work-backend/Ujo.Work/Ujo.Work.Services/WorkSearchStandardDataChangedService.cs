using System.Threading.Tasks;
using Nethereum.Web3;
using Ujo.Search.Service;
using Ujo.Work.Services.Ethereum;

namespace Ujo.Work.Services
{
    public class WorkSearchStandardDataChangedService : IStandardDataChangedService<Model.Work>
    {
        private readonly WorkSearchService _workSearchService;

        public WorkSearchStandardDataChangedService(WorkSearchService workSearchService)
        {
            _workSearchService = workSearchService;
        }

        public async Task UpsertAsync(Model.Work work)
        {
            if (work != null)
            {
                await _workSearchService.UploadOrMergeAsync(work);
            }
        }

        public async Task DataChangedAsync(Model.Work work, EventLog<CCC.StandardDataProcessing.DataChangedEvent> dataEventLog)
        {
            await UpsertAsync(work);
        }

        public async Task RemovedAsync(string contractAddress)
        {
            await _workSearchService.DeleteAsync(contractAddress);
        }
    }
}