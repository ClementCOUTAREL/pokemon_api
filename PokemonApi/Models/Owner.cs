using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Models
{
    public class Owner
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Gym { get; set; } = null!;
        public Country Country { get; set; } = null!;
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        
    }
}
