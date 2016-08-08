using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Ipfs;

namespace Ujo.IpfsImage.Services
{
    public class IpfsImageService
    {
        private string ipfsUrl;
        public IpfsImageService(string ipfsUrl)
        {
            this.ipfsUrl = ipfsUrl;
        }

        public async Task<Image> DownloadImage(string ipfsHash)
        {
            using (var ipfs = new IpfsClient(ipfsUrl))
            {
                Stream outputStream = await ipfs.Cat(ipfsHash).ConfigureAwait(false);
                return Image.FromStream(outputStream);
            }
        }

        public async Task<Image> ScaleImageByHeight(string ipfsHash, int maxHeight)
        {
            using (var image = await DownloadImage(ipfsHash).ConfigureAwait(false))
            {
                return ScaleImageByHeight(image, maxHeight);
            }
        }

        public async Task<Image> ScaleImageByWidth(string ipfsHash, int maxWidth)
        {
            using (var image = await DownloadImage(ipfsHash).ConfigureAwait(false))
            {
                return ScaleImageByHeight(image, maxWidth);
            }
        }

        public async Task<MerkleNode> Add(string name, Stream stream)
        {
            using (var ipfs = new IpfsClient(ipfsUrl))
            {
                var inputStream = new IpfsStream(name, stream);
                return await ipfs.Add(inputStream).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Adds an image to Ipfs return
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task<MerkleNode> AddImage(Image image, string name, ImageFormat format)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, format);
                return await Add(name, memoryStream).ConfigureAwait(false);
            }
        }

        public static Image ScaleImageByHeight(System.Drawing.Image image, int maxHeight)
        {
            var ratio = (double)maxHeight / image.Height;
            return ScaleImageByRatio(image, ratio);
        }

        public static Image ScaleImageByWidth(System.Drawing.Image image, int maxWidth)
        {
            var ratio = (double)maxWidth / image.Width;
            return ScaleImageByRatio(image, ratio);
        }

        public static Image ScaleImageByRatio(Image image, double ratio)
        {
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
    }
}