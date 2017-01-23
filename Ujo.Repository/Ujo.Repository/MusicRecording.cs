using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ujo.Repository
{
    public class MusicRecording: CreativeWork
    {
        public string IsrcCode { get; set; }
        public string IswcCode { get; set; }
        public string Label { get; set; }

        public IEnumerable<CreativeWorkArtist> GetPerformingArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == "PerformingArtist");
        }

        public IEnumerable<CreativeWorkArtist> GetFeaturedArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == "FeaturedArtist");
        }

        public IEnumerable<CreativeWorkArtist> GetContributingArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == "ContributingArtist");
        }
   
    }
}