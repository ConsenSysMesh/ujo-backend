using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ujo.Messaging
{
    public abstract class CreativeWorkDTO 
    {
        public CreativeWorkDTO()
        {
            OtherArtists = new List<CreativeWorkArtistDTO>();
        }

        public string ArtistName { get; set; }
        public string Address { get; set; }
        public string ByArtistAddress { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Audio { get; set; }
        public string Creator { get; set; }
        public string Genre { get; set; }
        public string Keywords { get; set; }
        public string Publisher { get; set; }
        public bool? HasPart { get; set; }
        public bool? IsPartOf { get; set; }
        public string IsFamilyFriendly { get; set; }
        public string License { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public ICollection<CreativeWorkArtistDTO> OtherArtists { get; set; }
        public string[] CreativeWorksUsed { get; set; }

    }
}
