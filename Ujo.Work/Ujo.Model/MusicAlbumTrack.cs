using System.ComponentModel.DataAnnotations.Schema;

namespace Ujo.Model
{
    public class MusicCollectionTrack: Entity
    {
        public int MusicCollectionTrackId { get; set; }
        [ForeignKey("MusicCollection")]
        public string MusicCollectionAddress { get; set; }
        [ForeignKey("MusicRecording")]
        public string MusicRecordingAddress { get; set; }
        public int Number { get; set; }
        public virtual MusicRecording MusicRecording { get; set; }
        public virtual MusicCollection MusicCollection { get; set; }
        //Cd1 / Side A / B
        public string OtherInfo { get; set; }
    }



}
