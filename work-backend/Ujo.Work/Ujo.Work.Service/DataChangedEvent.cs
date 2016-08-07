using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Ujo.Work.Service
{
    public class DataChangedEvent
    {
        [Parameter("uint", "key", 1, true)]
        public long Key { get; set; }

        [Parameter("string", "value", 2)]
        public string Value { get; set; }
    }
}