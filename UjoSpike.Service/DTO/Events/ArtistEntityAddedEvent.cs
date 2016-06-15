using Nethereum.ABI.FunctionEncoding.Attributes;

namespace UjoSpike.Service.DTO.Events
{
    public class ArtistEntityAddedEvent
    {
        [Parameter("uint", "artityEntityId", 1)]
        public long ArtistId { get; set; }

        [Parameter("string", "name", 2)]
        public string Name { get; set; }

        [Parameter("bool", "isAGroup", 3)]
        public bool IsAGroup { get; set; }

        [Parameter("string", "category", 4)]
        public string Category { get; set; }
    }
}
