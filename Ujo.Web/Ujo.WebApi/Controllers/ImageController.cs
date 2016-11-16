using Microsoft.AspNetCore.Mvc;
using Ujo.WebApi.Services;

namespace Ujo.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly IIpfsImageService ipfsImageService;

        public ImageController(IIpfsImageService ipfsImageService)
        {
            this.ipfsImageService = ipfsImageService;
        }

        [HttpGet("{ipfsHash}")]
        public IActionResult Get(string ipfsHash)
        {
            return new JsonResult(ipfsImageService.FindAsync(ipfsHash));
        }
    }
}