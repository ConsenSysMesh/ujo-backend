using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UjoSpike.ArtistWriter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = new RegisterArtists().ExecuteRegistryAsync().Result;
            Debug.WriteLine(result);
            System.Console.WriteLine(result);
            System.Console.ReadLine();
        }
    }
}
