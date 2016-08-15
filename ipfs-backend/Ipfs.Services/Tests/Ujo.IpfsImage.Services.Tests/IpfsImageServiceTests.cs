using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Ujo.IpfsImage.Services.Tests
{
    public class IpfsImageServiceTests
    {
        [Fact]
        public async Task ShouldAddDownloadScaleAndAddImage()
        {
          var ipfsService = new IpfsImageService("https://ipfs.infura.io:5001");
          var directory = Environment.CurrentDirectory;
          var file = File.OpenRead(Path.Combine(directory, "6.png"));
          var node = await ipfsService.Add("kf.png",file);
          var image = await ipfsService.ScaleImageByHeight(node.Hash.ToString(), 200);
          node = await ipfsService.AddImage(image, "kf200h.png", ImageFormat.Png);
          Assert.NotNull(node);
         }

        [Fact]
        public async Task ShouldUploadImageToInfura()
        {
            var directory = Environment.CurrentDirectory;
            var file = Path.Combine(directory, "6.png");
            var node = UploadFileToInfura(file);
            Assert.NotNull(node);
        }

        public async Task<Ipfs.MerkleNode> UploadCurrentDirectoryFileToInfura(string fileName)
        {
            var directory = Environment.CurrentDirectory;
            var file = Path.Combine(directory, fileName);
            return await UploadFileToInfura(file);
        }

        public async Task<Ipfs.MerkleNode> UploadFileToInfura(string filePath)
        {
            var ipfsService = new IpfsImageService("https://ipfs.infura.io:5001");
            var file = File.OpenRead(filePath);
            return await ipfsService.Add(Path.GetFileName(filePath), file);
        }


       
    }
}
