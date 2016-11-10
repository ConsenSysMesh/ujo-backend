using System;
using System.Threading.Tasks;
using Nethereum.Web3;
using Ujo.Work.Model;
using Ujo.Work.Services.Ethereum;
using Ujo.Work.Storage;

namespace Ujo.Work.Services
{
    public class WorkRepositoryStandardDataChangeService : IStandardDataChangedService<Model.Work>
    {
        private readonly WorkRepository _workRepository;

        public WorkRepositoryStandardDataChangeService(WorkRepository workRepository)
        {
            _workRepository = workRepository;
        }

        public async Task UpsertAsync(Model.Work work)
        {
            if (work != null)
            {
                var workEntity = _workRepository.NewWork(work);
                await workEntity.InsertOrReplaceAsync();
            }
        }

        public async Task DataChangedAsync(Model.Work work, EventLog<CCC.StandardDataProcessing.DataChangedEvent> dataEventLog)
        {
            var workEntity = await _workRepository.FindAsync(dataEventLog.Log.Address);
            var key = dataEventLog.Event.Key;
            var val = dataEventLog.Event.Value;

            WorkSchema schemaField;

            if (Enum.TryParse(key, out schemaField))
            {
                workEntity.Initialise(work);
            }
            else
            {
                workEntity.SetUnknownKey(val, key);
            }
            await workEntity.InsertOrMergeAsync();
        }

        public async Task RemovedAsync(string contractAddress)
        {
            var workEntity = await _workRepository.FindAsync(contractAddress);
            if (workEntity != null)
            {
                await workEntity.DeleteAsync();
            }
        }
    }
}