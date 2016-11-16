using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCC.BlockchainProcessing;
using CCC.Contracts.Registry.Services;
using Nethereum.Web3;

namespace CCC.Contracts.Registry.Processing
{
    public class BlockchainRegistryProcessor: IBlockchainProcessor
    {
        private readonly Web3 _web3;
        private readonly ILogger _logger;
        private readonly IEnumerable<IRegistryProcessingService> _registryProcessors;
        private readonly IByteCodeMatcher _registeredContractByteCodeMatcher;
        private RegistryService _registryService;

        public BlockchainRegistryProcessor(Web3 web3, string registryAddress, ILogger logger, 
                    IEnumerable<IRegistryProcessingService> registryProcessors,
                    IByteCodeMatcher registeredContractByteCodeMatcher)
        {
            _web3 = web3;
            _logger = logger;
            _registryProcessors = registryProcessors;
            _registeredContractByteCodeMatcher = registeredContractByteCodeMatcher;
            _registryService = new RegistryService(web3, registryAddress);
        }

        public async Task ProcessAsync(ulong fromBlockNumber, ulong toBlockNumber)
        {
            for (ulong i = fromBlockNumber; i <= toBlockNumber; i++)
            {
                try
                {
                    _logger.WriteLine("Getting all events of registered and unregistered from: " + i + "to: " +
                                      i);
                    var eventLogs = await _registryService.GetRegisteredUnregistered(i, i);

                    _logger.WriteLine("Found total of registered and unregistered logs: " + eventLogs.Count);

                    foreach (var eventLog in eventLogs)
                    {
                        if (eventLog is EventLog<RegisteredEvent>)
                        {
                            if (await _registeredContractByteCodeMatcher.IsMatchAsync((eventLog as EventLog<RegisteredEvent>).Event.RegisteredAddress))
                            {
                                await
                                    ProcessRegisteredWork(eventLog as EventLog<RegisteredEvent>);
                            }
                        }
                        else if (eventLog is EventLog<UnregisteredEvent>)
                        {
                            await
                                ProcessUnregistedWork(eventLog as EventLog<UnregisteredEvent>);
                        }
                        else
                        {
                            _logger.WriteLine("Unknown event type, should not reach here");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Put in error storage to log and process later on
                    System.Diagnostics.Trace.TraceError("Work Registry error, BlockNumber " + i + " Error:" +
                                                        ex.StackTrace.ToString());

                    _logger.WriteLine("Error:" + ex.Message);
                }
            }
        }

        private async Task ProcessUnregistedWork(EventLog<UnregisteredEvent> eventLog)
        {
            _logger.WriteLine("Unregistering " + eventLog.Event.RegisteredAddress);

            foreach (var registryProcessor in _registryProcessors)
            {
                await registryProcessor.ProcessUnregistered(eventLog);
            }
        }

        private async Task ProcessRegisteredWork(EventLog<RegisteredEvent> eventLog)
        {
            _logger.WriteLine("Registering " + eventLog.Event.RegisteredAddress);

            foreach (var registryProcessor in _registryProcessors)
            {
                await registryProcessor.ProcessRegistered(eventLog);
            }
        }
    }
}