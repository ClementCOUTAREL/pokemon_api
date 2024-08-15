using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Owner> Owner { get; set; }
    }
}
