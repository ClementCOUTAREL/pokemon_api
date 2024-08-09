namespace PokemonApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public int Rating { get; set; }

        public Reviewer Reviewer { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
