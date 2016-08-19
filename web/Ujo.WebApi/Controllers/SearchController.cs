using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search.Models;
using Ujo.WebApi.Services;

namespace Ujo.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly IWorkSearchService workSearchService;

        public SearchController(IWorkSearchService workSearchService)
        {
            this.workSearchService = workSearchService;
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> Get(string query)
        {
            return new  JsonResult((await workSearchService.SearchWork(query)).Results);

        }

        [HttpGet("suggest/{query}")]
        public async Task<IActionResult> Suggest(string query)
        {
            return new JsonResult((await workSearchService.Suggest(query, true)).Results);

        }
    }
}
