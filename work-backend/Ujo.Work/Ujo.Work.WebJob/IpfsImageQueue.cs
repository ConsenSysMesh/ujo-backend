using Microsoft.Azure.WebJobs;
using Ujo.Work.Services;

namespace Ujo.Work.WebJob
{
    public class IpfsImageQueue : IIpfsImageQueue
    {
        private readonly ICollector<string> _ipfsImageProcessionQueue;

        public IpfsImageQueue(ICollector<string> ipfsImageProcessionQueue)
        {
            _ipfsImageProcessionQueue = ipfsImageProcessionQueue;
        }

        public void Add(string ipfsInageHash)
        {
            _ipfsImageProcessionQueue.Add(ipfsInageHash);
        }
    }
}