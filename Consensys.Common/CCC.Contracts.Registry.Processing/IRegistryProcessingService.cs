using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCC.Contracts.Registry.Services;
using Nethereum.Web3;

namespace CCC.Contracts.Registry.Processing
{
    public interface IRegistryProcessingService
    {
        Task ProcessRegistered(EventLog<RegisteredEvent> registeredEvent);
        Task ProcessUnregistered(EventLog<UnregisteredEvent> unRegisteredEvent);
    }
}
