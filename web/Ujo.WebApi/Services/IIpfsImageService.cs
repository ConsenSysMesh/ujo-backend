using System.Collections.Generic;
using System.Threading.Tasks;
using Ujo.IpfsImage.Storage;

namespace Ujo.WebApi.Services
{
    public interface IIpfsImageService
    {
        Task<List<IpfsImage>> FindAsync(string ipfsHash);
    }
}