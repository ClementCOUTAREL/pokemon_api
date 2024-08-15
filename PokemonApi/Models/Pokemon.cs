using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }    
        public ICollection<PokemonCategory> PokemonCategories { get; set; }  
    }
}
