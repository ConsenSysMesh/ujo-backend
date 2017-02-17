using System.Collections.Generic;
using System.Linq;

namespace Ujo.Messaging
{
    public class MusicRecordingDTO:CreativeWorkDTO
    {
        public static string FeaturedArtistContributionTypeKey = "FeaturedArtist";
        public static string PerformingArtistContributionTypeKey = "PerformingArtist";
        public static string ContributingArtistContributionTypeKey = "ContributingArtist";

        public string IsrcCode { get; set; }
        public string IswcCode { get; set; }
        public string Label { get; set; }

        public IEnumerable<CreativeWorkArtistDTO> GetPerformingArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == PerformingArtistContributionTypeKey);
        }

        public IEnumerable<CreativeWorkArtistDTO> GetFeaturedArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == FeaturedArtistContributionTypeKey);
        }

        public IEnumerable<CreativeWorkArtistDTO> GetContributingArtists()
        {
            return OtherArtists.Where(x => x.ContributionType == ContributingArtistContributionTypeKey);
        }

    }
}