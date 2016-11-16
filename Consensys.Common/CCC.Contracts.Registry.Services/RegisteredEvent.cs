using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CCC.Contracts.Registry.Services
{
    public class RegisteredEvent
    {
        [Parameter("address", "registeredAddress", 1, true)]
        public string RegisteredAddress { get; set; }

        [Parameter("address", "owner", 3, true)]
        public string Owner { get; set; }

        [Parameter("uint256", "time", 4)]
        public long Time { get; set; }

        [Parameter("uint256", "Id", 2, true)]
        public long Id { get; set; }

    }
}