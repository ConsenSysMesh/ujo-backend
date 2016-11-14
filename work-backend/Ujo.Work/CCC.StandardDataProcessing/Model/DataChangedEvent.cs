using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CCC.StandardDataProcessing
{
    public class DataChangedEvent
    {
        [Parameter("bytes32", "key", 1, true)]
        public string Key { get; set; }

        [Parameter("string", "value", 2)]
        public string Value { get; set; }
    }
}