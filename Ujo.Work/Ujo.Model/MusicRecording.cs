using System.Collections.Generic;
using System.Linq;

namespace Ujo.Model
{
    public class MusicRecording: CreativeWork
    {
        public static string FeaturedArtistContributionTypeKey = "FeaturedArtist";
        public static string PerformingArtistContributionTypeKey = "PerformingArtist";
        public static string ContributingArtistContributionTypeKey = "ContributingArtist";

        public string IsrcCode { get; set; }
        public string IswcCode { get; set; }
        public string Label { get; set; }

        public IEnumerable<CreativeWorkArtist> GetPerformingArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == PerformingArtistContributionTypeKey);
        }

        public IEnumerable<CreativeWorkArtist> GetFeaturedArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == FeaturedArtistContributionTypeKey);
        }

        public IEnumerable<CreativeWorkArtist> GetContributingArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == ContributingArtistContributionTypeKey);
        }
   
    }
}