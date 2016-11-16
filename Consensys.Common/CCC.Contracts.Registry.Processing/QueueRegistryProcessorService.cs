using System.Threading.Tasks;
using CCC.Contracts.Registry.Services;
using Nethereum.Web3;

namespace CCC.Contracts.Registry.Processing
{
    public class QueueRegistryProcessorService: IRegistryProcessingService
    {
        private readonly IQueueRegistry _queueRegistry;

        public QueueRegistryProcessorService(IQueueRegistry queueRegistry)
        {
            _queueRegistry = queueRegistry;
        }

        public async Task ProcessRegistered(EventLog<RegisteredEvent> registeredEvent)
        {
            await
                _queueRegistry.Add(new RegistrationMessage()
                {
                    Address = registeredEvent.Event.RegisteredAddress,
                    Registered = true
                });
        }

        public async Task ProcessUnregistered(EventLog<UnregisteredEvent> unRegisteredEvent)
        {
            await
                _queueRegistry.Add(new RegistrationMessage()
                {
                    Address = unRegisteredEvent.Event.RegisteredAddress,
                    Registered = false
                });
        }
    }
}