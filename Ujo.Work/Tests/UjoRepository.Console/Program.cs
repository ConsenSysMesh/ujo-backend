using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ujo.Model;
using Ujo.Repository;

namespace UjoRepository.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new UjoContext())
            {

                System.Console.Write("Enter a name for a new Artist: ");
                var name = System.Console.ReadLine();

                var artist = new Artist() { Name = name, Address = "XXX" + name };
                db.Artists.AddOrUpdate(artist);
                db.SaveChanges();

                var musicRecording = new MusicRecording()
                {
                    Address = "xxxRec" + name,
                    Genre = "Monkey Techno",
                    Name = "Pooping head2",
                    ByArtistAddress = "XXX" + name
                };
                
                musicRecording.OtherArtists.Add(
                    new CreativeWorkArtist()
                    {
                        ContributionType =  "Featured2",
                        Role = "Guitar",
                        NonRegisteredArtistName = "Simon",
                        CreativeWorkAddress =  musicRecording.Address

                    });

                db.MusicRecordings.AddOrUpdate(musicRecording);
                db.CreativeWorkArtists.AddOrUpdate(musicRecording.OtherArtists.First());

                var creativeWorkArtist = new CreativeWorkArtist()
                {
                    ContributionType = "Featured",
                    NonRegisteredArtistName = "Ujo",
                    CreativeWorkAddress =  "xxxRec" + name
                };

                db.CreativeWorkArtists.AddOrUpdate(creativeWorkArtist);
                db.SaveChanges();

               

                // Display all Blogs from the database 
                var query = from b in db.Artists
                            orderby b.Name
                            select b;

                System.Console.WriteLine("All artists in the database:");
                foreach (var item in query)
                {
                    System.Console.WriteLine(item.Name);
                }

                System.Console.WriteLine("Press any key to exit...");
                System.Console.ReadKey();
            }
        }
    }
}
