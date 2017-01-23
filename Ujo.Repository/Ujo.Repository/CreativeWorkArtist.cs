using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ujo.Repository
{
    public class CreativeWorkArtist
    {
        [Key]
        public int CreativeWorkArtistId { get; set; }
        [ForeignKey("CreativeWork")]
        public string CreativeWorkAddress { get; set; }
        [ForeignKey("Artist")]
        public string ArtistAddres { get; set; }
        public string ContributionType { get; set; }
        public string Role { get; set; }
        public string NonRegisteredArtistName { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual CreativeWork CreativeWork { get; set; }
    }
}