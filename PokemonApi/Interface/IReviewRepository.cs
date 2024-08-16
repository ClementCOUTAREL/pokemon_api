using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetReviews();
        Task<Review> GetReviewById(int reviewId);
        Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId);
        Task<bool> isReviewExists(int reviewId);
        Task<bool> CreateReview(Review review);
        Task<bool> UpdateReview(Review review);
        Task<bool> DeleteReview(Review review);
        Task<bool> Save();
    }
}
