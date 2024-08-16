namespace PokemonApi.Dto.Review
{
    public class UpdateReviewRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }
}
