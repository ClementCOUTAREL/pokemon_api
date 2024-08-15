using System.ComponentModel.DataAnnotations;

namespace PokemonApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Text { get; set; } = null!;
        [Required]
        public int Rating { get; set; }
        public Reviewer Reviewer { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
