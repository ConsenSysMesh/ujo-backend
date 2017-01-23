using System.ComponentModel.DataAnnotations;

namespace Ujo.Repository
{
    public class Artist
    {
        [Key]
        public string Address { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}