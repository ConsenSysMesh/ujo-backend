using Nethereum.ABI.FunctionEncoding.Attributes;
namespace Nethereum.ContractRegistry
{
    [FunctionOutput]
    public class RegistryRecord
    {
        [Parameter("address", "registeredAddress", 1)]
        public string RegisteredAddress { get; set; }
        [Parameter("address", "owner", 2)]
        public string Owner { get; set; }
        [Parameter("uint256", "time", 3)]
        public long Time { get; set; }
        [Parameter("uint256", "Id", 4)]
        public long Id { get; set; }
    }

    public class RegisteredEvent
    {
        [Parameter("address", "registeredAddress", 1, true)]
        public string RegisteredAddress { get; set; }
        [Parameter("address", "owner", 3, true)]
        public string Owner { get; set; }

        [Parameter("uint256", "time", 3)]
        public long Time { get; set; }

        [Parameter("uint256", "Id", 3, true)]
        public long Id { get; set; }

    }

    public class UnregisteredEvent
    {
        [Parameter("address", "registeredAddress", 1, true)]
        public string RegisteredAddress { get; set; }
      
        [Parameter("uint256", "Id", 2, true)]
        public long Id { get; set; }

    }
}