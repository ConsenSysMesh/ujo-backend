using System.Data.Entity;

namespace Ujo.Repository
{
    public class UjoContext : DbContext
    {
        public UjoContext() : base("name=Ujo")
        {
            
        }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<CreativeWork> CreativeWorks { get; set; }
        public DbSet<MusicCollection> MusicAlbums { get; set; }
        public DbSet<MusicCollectionTrack> MusicCollectionTracks { get; set; }
        public DbSet<MusicRecording> MusicRecordings { get; set; }
        public DbSet<CreativeWorkArtist> CreativeWorkArtists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}