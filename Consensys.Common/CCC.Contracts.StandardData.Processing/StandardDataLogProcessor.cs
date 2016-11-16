using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCC.BlockchainProcessing;
using CCC.Contracts.StandardData.Services;
using CCC.Contracts.StandardData.Services.Model;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Nethereum.Web3.Contracts.Comparers;

namespace CCC.Contracts.StandardData.Processing
{
    public abstract class StandardDataLogProcessor<T>:ILogProcessor
    {
        protected Web3 Web3 { get; }
        protected IStandardDataRegistry StandardDataRegistry { get; }

        protected IEnumerable<IStandardDataProcessingService<T>> DataChangedServices { get; }

        public StandardDataLogProcessor(Web3 web3, IStandardDataRegistry standardDataRegistry, 
                                                          IEnumerable<IStandardDataProcessingService<T>>  dataChangedServices
                                                           )
        {
            Web3 = web3;
            StandardDataRegistry = standardDataRegistry;
            DataChangedServices = dataChangedServices;
        }

      
        public async Task ProcessDataChangeUpdateAsync(IEnumerable<EventLog<DataChangedEvent>> dataEventLogs)
        {
            foreach (var dataEventLog in dataEventLogs)
            {
                await ProcessDataChangeUpdateAsync(dataEventLog);
            }
        }

        public abstract Task<T> GetObjectModelAsync(string contractAddress);

        public async Task ProcessFullUpsertAsync(string contractAddress)
        {
            if (await StandardDataRegistry.ExistsAsync(contractAddress))
            {
                var model = await GetObjectModelAsync(contractAddress);
                foreach (var dataChangedService in DataChangedServices)
                {
                    await dataChangedService.UpsertAsync(model);
                }
            }
        }

        private async Task ProcessDataChangeUpdateAsync(EventLog<DataChangedEvent> dataEventLog)
        {
            var contractAddress = dataEventLog.Log.Address;
            if (await StandardDataRegistry.ExistsAsync(contractAddress))
            {
                var model = await GetObjectModelAsync(contractAddress);
                foreach (var dataChangedService in DataChangedServices)
                {
                    await dataChangedService.DataChangedAsync(model, dataEventLog);
                }
            }
        }

        public async Task RemoveUnregisteredAsync(string address)
        {
            foreach (var dataChangedService in DataChangedServices)
            {
                await dataChangedService.RemovedAsync(address);
            }
        }

        public bool IsLogForEvent(FilterLog log)
        {
            var worksService = new StandardDataService(Web3);
            return worksService.IsStandardDataChangeLog(log);
        }

        public Task ProcessLogsAsync(params FilterLog[] logs)
        {
            Array.Sort(logs, new FilterLogBlockNumberTransactionIndexComparer());

            return ProcessDataChangeUpdateAsync(Event.DecodeAllEvents<DataChangedEvent>(logs));
        }
    }
}