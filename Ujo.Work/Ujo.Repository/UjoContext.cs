using System.Data.Entity;
using Ujo.Model;
using Ujo.Repository.Infrastructure;

namespace Ujo.Repository
{
    public class UjoContext : DataContext
    {
        public UjoContext() : base("name=Ujo")
        {
            
        }

        public UjoContext(string connectionStringOrName) : base(connectionStringOrName)
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

            modelBuilder.Entity<CreativeWorkArtist>()
                .HasOptional(o => o.Artist);

        }

    }
}