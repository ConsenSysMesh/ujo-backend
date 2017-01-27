namespace Ujo.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Address = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Bio = c.String(),
                        Genre = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Address);
            
            CreateTable(
                "dbo.CreativeWorkArtists",
                c => new
                    {
                        CreativeWorkArtistId = c.Int(nullable: false, identity: true),
                        CreativeWorkAddress = c.String(nullable: false, maxLength: 128),
                        ArtistAddres = c.String(maxLength: 128),
                        ContributionType = c.String(),
                        Role = c.String(),
                        NonRegisteredArtistName = c.String(),
                    })
                .PrimaryKey(t => t.CreativeWorkArtistId)
                .ForeignKey("dbo.Artists", t => t.ArtistAddres)
                .ForeignKey("dbo.CreativeWorks", t => t.CreativeWorkAddress, cascadeDelete: true)
                .Index(t => t.CreativeWorkAddress)
                .Index(t => t.ArtistAddres);
            
            CreateTable(
                "dbo.CreativeWorks",
                c => new
                    {
                        Address = c.String(nullable: false, maxLength: 128),
                        ByArtistAddress = c.String(maxLength: 128),
                        Name = c.String(),
                        Image = c.String(),
                        Description = c.String(),
                        Audio = c.String(),
                        Creator = c.String(),
                        Genre = c.String(),
                        Keywords = c.String(),
                        Publisher = c.String(),
                        HasPart = c.Boolean(),
                        IsPartOf = c.Boolean(),
                        IsFamilyFriendly = c.String(),
                        License = c.String(),
                        DateCreated = c.String(),
                        DateModified = c.String(),
                        MusicCollectionType = c.String(),
                        NumberOfTracks = c.Int(),
                        IsrcCode = c.String(),
                        IswcCode = c.String(),
                        Label = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Address)
                .ForeignKey("dbo.Artists", t => t.ByArtistAddress)
                .Index(t => t.ByArtistAddress);
            
            CreateTable(
                "dbo.MusicCollectionTracks",
                c => new
                    {
                        MusicCollectionTrackId = c.Int(nullable: false, identity: true),
                        MusicCollectionAddress = c.String(maxLength: 128),
                        MusicRecordingAddress = c.String(maxLength: 128),
                        Number = c.Int(nullable: false),
                        OtherInfo = c.String(),
                    })
                .PrimaryKey(t => t.MusicCollectionTrackId)
                .ForeignKey("dbo.CreativeWorks", t => t.MusicCollectionAddress)
                .ForeignKey("dbo.CreativeWorks", t => t.MusicRecordingAddress)
                .Index(t => t.MusicCollectionAddress)
                .Index(t => t.MusicRecordingAddress);
            
            CreateTable(
                "dbo.CreativeWorkCreativeWorks",
                c => new
                    {
                        CreativeWork_Address = c.String(nullable: false, maxLength: 128),
                        CreativeWork_Address1 = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CreativeWork_Address, t.CreativeWork_Address1 })
                .ForeignKey("dbo.CreativeWorks", t => t.CreativeWork_Address)
                .ForeignKey("dbo.CreativeWorks", t => t.CreativeWork_Address1)
                .Index(t => t.CreativeWork_Address)
                .Index(t => t.CreativeWork_Address1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreativeWorkArtists", "CreativeWorkAddress", "dbo.CreativeWorks");
            DropForeignKey("dbo.MusicCollectionTracks", "MusicRecordingAddress", "dbo.CreativeWorks");
            DropForeignKey("dbo.MusicCollectionTracks", "MusicCollectionAddress", "dbo.CreativeWorks");
            DropForeignKey("dbo.CreativeWorkCreativeWorks", "CreativeWork_Address1", "dbo.CreativeWorks");
            DropForeignKey("dbo.CreativeWorkCreativeWorks", "CreativeWork_Address", "dbo.CreativeWorks");
            DropForeignKey("dbo.CreativeWorks", "ByArtistAddress", "dbo.Artists");
            DropForeignKey("dbo.CreativeWorkArtists", "ArtistAddres", "dbo.Artists");
            DropIndex("dbo.CreativeWorkCreativeWorks", new[] { "CreativeWork_Address1" });
            DropIndex("dbo.CreativeWorkCreativeWorks", new[] { "CreativeWork_Address" });
            DropIndex("dbo.MusicCollectionTracks", new[] { "MusicRecordingAddress" });
            DropIndex("dbo.MusicCollectionTracks", new[] { "MusicCollectionAddress" });
            DropIndex("dbo.CreativeWorks", new[] { "ByArtistAddress" });
            DropIndex("dbo.CreativeWorkArtists", new[] { "ArtistAddres" });
            DropIndex("dbo.CreativeWorkArtists", new[] { "CreativeWorkAddress" });
            DropTable("dbo.CreativeWorkCreativeWorks");
            DropTable("dbo.MusicCollectionTracks");
            DropTable("dbo.CreativeWorks");
            DropTable("dbo.CreativeWorkArtists");
            DropTable("dbo.Artists");
        }
    }
}
