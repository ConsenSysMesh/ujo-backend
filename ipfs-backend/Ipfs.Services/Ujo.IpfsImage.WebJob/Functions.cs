
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Ujo.IpfsImage.Services;
using Ujo.IpfsImage.Storage;
using Wintellect.Azure.Storage.Table;

namespace Ujo.IpfsImage.WebJob
{
    public class Functions
    {
        public static async Task ProcessIpfsCoverImage([QueueTrigger("IpfsCoverImageProcessingQueue")] string ipfsImageHash, [Table("IpfsImageResized")] CloudTable tableBinding, TextWriter log)
        {
            log.WriteLine("Start job");
            
            log.WriteLine("Processing cover image");
            await ProcessResizeImageByCrop(ipfsImageHash, tableBinding, new Size(62,62));
            //image sizes?
            //todo write to work cloud table
            log.WriteLine("Finished processing cover image");
        }

        //this could be refactor to a generic project not related to Ujo
        //other resizes not related to work could be in a another web job queues.
        public static async Task ProcessResizeImageByHeight(string ipfsImageHash, CloudTable ipfsImageResizedCloudTable, int height)
        {
            var service = new IpfsImageService(ConfigurationSettings.GetIpfsRPCUrl());
            var image = await service.ScaleImageByHeight(ipfsImageHash, height);
            var sizeKey = IpfsImageResized.GetHeightNewSizeKey(height);
            var node = await service.AddImage(image, ipfsImageHash + "_" + sizeKey, ImageFormat.Png);
            var entity = IpfsImageResized.Create(ipfsImageResizedCloudTable, ipfsImageHash, sizeKey,
                node.Hash.ToString());
            await entity.InsertOrReplaceAsync();
        }

        public static async Task ProcessResizeImageByCrop(string ipfsImageHash, CloudTable ipfsImageResizedCloudTable, Size dimensions)
        {
            var service = new IpfsImageService(ConfigurationSettings.GetIpfsRPCUrl());
            var image = await service.ScaleImage(ipfsImageHash, dimensions);
            var sizeKey = IpfsImageResized.GetCropNewSizeKey(dimensions.Width, dimensions.Height);
            var node = await service.AddImage(image, ipfsImageHash + "_" + sizeKey, ImageFormat.Png);
            var entity = IpfsImageResized.Create(ipfsImageResizedCloudTable, ipfsImageHash, sizeKey,
                node.Hash.ToString());
            await entity.InsertOrReplaceAsync();
        }

        public static async Task ProcessResizeImageByWidth(string ipfsImageHash, CloudTable ipfsImageResizedCloudTable, int width)
        {
            var service = new IpfsImageService(ConfigurationSettings.GetIpfsRPCUrl());
            var image = await service.ScaleImageByWidth(ipfsImageHash, width);
            var sizeKey = IpfsImageResized.GetWidthNewSizeKey(width);
            var node = await service.AddImage(image, ipfsImageHash + "_" + sizeKey, ImageFormat.Png);
            var entity = IpfsImageResized.Create(ipfsImageResizedCloudTable, ipfsImageHash, sizeKey,
                node.Hash.ToString());
            await entity.InsertOrReplaceAsync();
        }
    }
}
