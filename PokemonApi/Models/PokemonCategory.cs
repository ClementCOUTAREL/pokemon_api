using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Models
{
    public class PokemonCategory
    {
        [Required]
        public int PokemonId { get; set; }
        [Required]
        public int CategoryId {  get; set; }
        public Pokemon Pokemon { get; set; }
        public Category Category { get; set; }
    }
}
