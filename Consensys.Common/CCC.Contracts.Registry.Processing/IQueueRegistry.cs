using System.Threading.Tasks;

namespace CCC.Contracts.Registry.Processing
{
    public interface IQueueRegistry
    {
        Task Add(RegistrationMessage registrationMessage);
    }
}