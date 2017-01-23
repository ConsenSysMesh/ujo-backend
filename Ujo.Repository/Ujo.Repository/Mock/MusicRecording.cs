using System.Collections.Generic;

namespace Ujo.Repository.Mock
{
    public class MusicRecording: CreativeWork
    {
       // public Artist ByArtist { get; set; }
        public string IsrcCode { get; set; }
        public string IswcCode { get; set; }
        public string Label { get; set; }
        //public List<CreativeWorkArtist> PerformingArtists;
        //public List<CreativeWorkArtist> FeaturedArtists;
       // public List<CreativeWorkArtist> ContributingArtists;

    }
}