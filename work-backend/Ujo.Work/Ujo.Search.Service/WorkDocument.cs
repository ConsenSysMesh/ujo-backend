using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ujo.Search.Service
{
    [SerializePropertyNamesAsCamelCase]
    public class WorkDocument
    {
        public string Address { get; set; }
        public string Title { get; set; }

        public string[] CreatorsAddresses { get; set; }

        public string[] CreatorsNames { get; set; }

        public string Genre { get; set; }
        public string[] Tags { get; set; }

        public string WorkFile { get; set; }

        public string CoverFile { get; set; }

    }
}
