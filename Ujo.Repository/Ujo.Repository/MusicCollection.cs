using System.Collections.Generic;

namespace Ujo.Repository
{
    public class MusicCollection : CreativeWork
    {
        public MusicCollection()
        {
            Tracks = new List<MusicCollectionTrack>();
        }
        public string CollectionType { get; set; }
        public int NumberOfTracks { get; set; }
        public virtual ICollection<MusicCollectionTrack> Tracks { get; set; }
    }
}