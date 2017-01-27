using System.ComponentModel.DataAnnotations;

namespace Ujo.Model
{
    public class Artist:Entity
    {
        [Key]
        public string Address { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Genre { get; set; }
        public string Image { get; set; }
    }
}