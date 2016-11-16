using System.Threading.Tasks;
using CCC.Contracts.Registry.Processing;
using Microsoft.Azure.WebJobs;

namespace Ujo.WorkRegistry.WebJob
{
    public class QueueRegistry : IQueueRegistry
    {
        private readonly ICollector<RegistrationMessage> _registryProcessionQueue;

        public QueueRegistry(ICollector<RegistrationMessage> registryProcessionQueue)
        {
            _registryProcessionQueue = registryProcessionQueue;
        }

        public async Task Add(RegistrationMessage registrationMessage)
        {
            _registryProcessionQueue.Add(registrationMessage);
        }
    }
}