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
        public string ArtistAddress { get; set; }
        public string ArtistName { get; set; }
        public string[] FeaturedArtitsAddresses { get; set; }
        public string[] FeaturesArtistsNames { get; set; }
        
        /// <summary>
        /// Pipe limited Index | Address | Name | Role
        /// </summary>
        public string[] FeaturedArtists { get; set; }

        public string[] ContributingArtistsAddresses { get; set; }

        public string[] ContributingArtistsNames { get; set; }

        /// <summary>
        /// Pipe limited Address | Name | Role
        /// </summary>
        public string[] ContributingArtists { get; set; }

        public string[] PerformingArtitsAddresses { get; set; }

        public string[] PerformingArtistsNames { get; set; }

        /// <summary>
        /// Pipe limited Address | Name | Role
        /// </summary>
        public string[] PerformingArtists { get; set; }

        public string Genre { get; set; }
        public string[] Keywords { get; set; }

        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Audio { get; set; }

        public string Label { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public bool HasPartOf { get; set; }
        public bool IsPartOf { get; set; }
        public string IsFamilyFriendly { get; set; }
        public string License { get; set; }
        public string IswcCode { get; set; }

    }
}
