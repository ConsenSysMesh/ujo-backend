using System.Collections.Generic;

namespace Ujo.Model
{
    public class MusicCollection : CreativeWork
    {
        public MusicCollection()
        {
            Tracks = new List<MusicCollectionTrack>();
        }
        public string MusicCollectionType { get; set; }
        public int NumberOfTracks { get; set; }
        public virtual ICollection<MusicCollectionTrack> Tracks { get; set; }
    }
}