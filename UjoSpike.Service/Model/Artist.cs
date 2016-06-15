using Nethereum.ABI.FunctionEncoding.Attributes;

namespace UjoSpike.Service.Model
{
    [FunctionOutput]
    public class Artist
    {
        [Parameter("string", "name", 1)]
        public string Name { get; set; }

        [Parameter("bool", "isAGroup", 2)]
        public bool IsAGroup { get; set; }

        [Parameter("string", "category", 3)]
        public string Category { get; set; }
    }
}
