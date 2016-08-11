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
            var ipfsService = new IpfsImageService("https://ipfs.infura.io:5001");
            var directory = Environment.CurrentDirectory;
            var file = File.OpenRead(Path.Combine(directory, "6.png"));
            var node = await ipfsService.Add("kf.png", file);
            Assert.NotNull(node);
        }


       
    }
}
