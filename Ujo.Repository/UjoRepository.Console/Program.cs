using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ujo.Repository;

namespace UjoRepository.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new UjoContext())
            {
                // Create and save a new Blog 
                System.Console.Write("Enter a name for a new Artist: ");
                var name = System.Console.ReadLine();

                var blog = new Artist() { Name = name, Address = "XXX"};
                db.Artists.Add(blog);
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
