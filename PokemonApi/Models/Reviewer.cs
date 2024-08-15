using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Models
{
    public class Reviewer
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
