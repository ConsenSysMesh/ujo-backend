using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Ujo.WorkRegistry.Service
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
}