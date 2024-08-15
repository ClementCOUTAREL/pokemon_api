using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Models
{
    public class Category
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; } = null!;
        [Required]
        public ICollection<PokemonCategory> PokemonCategories { get; set; }  
    }
}
