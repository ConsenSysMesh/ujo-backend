using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCC.BlockchainProcessing;
using Nethereum.RPC.Eth.Filters;
using Nethereum.Web3;
using Nethereum.Web3.Contracts.Comparers;
using Ujo.Work.Services;

namespace CCC.StandardDataProcessing
{
    public abstract class StandardDataChangedLogProcessor<T>:ILogProcessor
    {
        protected Web3 Web3 { get; }
        protected IStandardDataRegistry StandardDataRegistry { get; }

        protected IEnumerable<IStandardDataChangedService<T>> DataChangedServices { get; }

        public StandardDataChangedLogProcessor(Web3 web3, IStandardDataRegistry standardDataRegistry, 
                                                          IEnumerable<IStandardDataChangedService<T>>  dataChangedServices
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

        public abstract bool IsLogForEvent(FilterLog log);
       

        public Task ProcessLogsAsync(params FilterLog[] logs)
        {
            Array.Sort(logs, new FilterLogBlockNumberTransactionIndexComparer());

            return ProcessDataChangeUpdateAsync(Event.DecodeAllEvents<DataChangedEvent>(logs));
        }
    }
}