using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ipfs;

namespace IPFSPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessImage().Wait();
            Console.WriteLine("Done...");
            Console.ReadLine();
        }


        public static async Task ProcessImage()
        {
            using (var ipfs = new IpfsClient("https://ipfs.infura.io:5001"))
            {
                //Name of the file to add
                string fileName = "kids.jpg";

                //Wrap our stream in an IpfsStream, so it has a file name.
                IpfsStream inputStream = new IpfsStream(fileName, File.OpenRead(fileName));

                MerkleNode node = await ipfs.Add(inputStream);

                Debug.WriteLine(node.Hash.ToString());

                Stream outputStream = await ipfs.Cat(node.Hash.ToString());
                using (var image = Image.FromStream(outputStream))
                {
                    var newImage = ScaleImage(image, 500);
                    newImage.Save("newKids.jpg", ImageFormat.Jpeg);

                    inputStream = new IpfsStream("kids500", File.OpenRead("newKids.jpg"));

                    node = await ipfs.Add(inputStream);

                    Debug.WriteLine(node.Hash.ToString());
                }

            }
        }

        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxHeight)
        {
            var ratio = (double)maxHeight / image.Height;
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static async Task ProcessFile()
        {
            using (var ipfs = new IpfsClient())
            {
                //Name of the file to add
                string fileName = "test.txt";

                //Wrap our stream in an IpfsStream, so it has a file name.
                IpfsStream inputStream = new IpfsStream(fileName, File.OpenRead(fileName));

                MerkleNode node = await ipfs.Add(inputStream);

                Stream outputStream = await ipfs.Cat(node.Hash.ToString());

                using (StreamReader sr = new StreamReader(outputStream)) 
                {
                    string contents = sr.ReadToEnd();

                    Console.WriteLine(contents); //Contents of test.txt are printed here!
                }
            }
        }
    }
}
