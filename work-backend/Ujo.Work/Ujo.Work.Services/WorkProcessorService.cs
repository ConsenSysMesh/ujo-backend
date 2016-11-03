using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.Web3;
using Ujo.Search.Service;
using Ujo.Work.Model;
using Ujo.Work.Services.Ethereum;
using Ujo.Work.Storage;
using Ujo.Work.WebJob;

namespace Ujo.Work.Services
{
    public class WorkProcessorService
    {
        private readonly WorkSearchService _workSearchService;
        private readonly IIpfsImageQueue _ipfsImageQueue;
        private readonly WorkRepository _workRepository;
        private readonly Web3 _web3;
        private readonly WorkRegistryRepository _workRegistryRepository;

        public WorkProcessorService(WorkSearchService workSearchService,
            IIpfsImageQueue ipfsImageQueue, WorkRepository workRepository, Web3 web3, WorkRegistryRepository workRegistryRepository)
        {
            _workSearchService = workSearchService;
            _ipfsImageQueue = ipfsImageQueue;
            _workRepository = workRepository;
            _web3 = web3;
            _workRegistryRepository = workRegistryRepository;
        }

        public async Task ProcessWorksAsync(long fromBlockNumber, long toBlockNumber)
        {
            await ProcessWorksAsync(Convert.ToUInt64(fromBlockNumber), Convert.ToUInt64(toBlockNumber));
        }

        public async Task ProcessWorksAsync(ulong fromBlockNumber, ulong toBlockNumber)
        {
            var worksService = new WorksService(_web3);
            var dataEventLogs = await worksService.GetDataChangedEventsAsync(fromBlockNumber, toBlockNumber);
            //TODO: ensure sorted
            await ProcessDataChangeUpdateAsync(dataEventLogs);
        }

        public async Task ProcessDataChangeUpdateAsync(IEnumerable<EventLog<DataChangedEvent>> dataEventLogs)
        {
            foreach (var dataEventLog in dataEventLogs)
            {
                await ProcessDataChangeUpdateAsync(dataEventLog);
            }
        }

        public async Task ProcessWorkAsync(string workContractAddress)
        {
            var workService = new WorkService(_web3, workContractAddress);
            var work = await workService.GetWorkAsync();

            if (work != null)
            {
                var workEntity = _workRepository.NewWork(work);
                await _workSearchService.UploadOrMergeAsync(work);
                var result = await workEntity.InsertOrReplaceAsync();
                if (!string.IsNullOrEmpty(work.CoverImageIpfsHash))
                {
                    _ipfsImageQueue.Add(work.CoverImageIpfsHash);
                }
            }
        }

        public async Task ProcessDataChangeUpdateAsync(EventLog<DataChangedEvent> dataEventLog)
        {
            var workContractAddress = dataEventLog.Log.Address;
            if (await _workRegistryRepository.ExistsAsync(workContractAddress))
            {
                var work = await _workRepository.FindAsync(workContractAddress);
                if (work == null)
                {
                    await ProcessWorkAsync(workContractAddress);
                }
                else
                {
                    await ProcessDataChangeUpdateAsync(work, dataEventLog, workContractAddress);
                }
            }
        }

        private async Task ProcessDataChangeUpdateAsync(WorkEntity workEntity, EventLog<DataChangedEvent> dataEventLog, string workContractAddress)
        {
            var workService = new WorkService(_web3, workContractAddress);
            var work = await workService.GetWorkAsync();
           
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

            if (key == WorkSchema.Image.ToString())
            {
                _ipfsImageQueue.Add(work.CoverImageIpfsHash);
            }

            await _workSearchService.UploadOrMergeAsync(work);
            await workEntity.InsertOrMergeAsync();
        }

        public async Task RemoveUnregisteredAsync(string address)
        {
            var workEntity = await _workRepository.FindAsync(address);
            if (workEntity != null)
            {
                await workEntity.DeleteAsync();
                await _workSearchService.DeleteAsync(address);
            }
        }
    }
}