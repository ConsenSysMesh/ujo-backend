using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CCC.Contracts.Registry.Services
{
    public class UnregisteredEvent
    {
        [Parameter("address", "registeredAddress", 1, true)]
        public string RegisteredAddress { get; set; }
      
        [Parameter("uint256", "Id", 2, true)]
        public long Id { get; set; }

    }
}