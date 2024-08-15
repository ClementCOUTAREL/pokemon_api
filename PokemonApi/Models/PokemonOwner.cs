using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Models
{
    public class PokemonOwner
    {
        [Required]
        public int PokemonId { get; set; }
        [Required]
        public int OwnerId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Owner Owner { get; set; }
    }
}
