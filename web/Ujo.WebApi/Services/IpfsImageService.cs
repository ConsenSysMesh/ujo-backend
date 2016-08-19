using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Ujo.IpfsImage.Storage;
using Wintellect.Azure.Storage.Table;

namespace Ujo.WebApi.Services
{
    public class IpfsImage
    {
        public string Original { get; set; }
        public string Resized { get; set; }
        public string Dimensions { get; set; }
    }
    
    public class IpfsImageService : IIpfsImageService
    {
        private readonly IOptions<AppSettings> settings;

        public IpfsImageService(IOptions<AppSettings> settings)
        {
            this.settings = settings;
        }

        public async Task<List<IpfsImage>> FindAsync(string ipfsHash)
        {
            var images =  await IpfsImageResized.FindAsync(GetIpfsImageCloudTable(), ipfsHash);
            return images.Select(x => new IpfsImage()
            {
                Original = x.IpfsImageHash,
                Resized = x.IpfsImageNewSizeHash,
                Dimensions = x.NewSize
            }).ToList();
        }

        private CloudTable GetIpfsImageCloudTable()
        {
            CloudStorageAccount storageAccount =
            CloudStorageAccount.Parse(settings.Value.IpfsImageStoreConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            return tableClient.GetTableReference(settings.Value.IpfsImageTableName);
        }
    }
}