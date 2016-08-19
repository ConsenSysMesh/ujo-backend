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
        public IActionResult Get(string query)
        {
           // If blank search, assume they want to search everything
            if (string.IsNullOrWhiteSpace(query))
                query = "*";
            return new JsonResult(workSearchService.SearchWork(query).Results);

        }      
     }
        
}
