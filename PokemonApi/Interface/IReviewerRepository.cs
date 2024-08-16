using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface IReviewerRepository
    {
        Task<ICollection<Reviewer>> GetReviewers();
        Task<Reviewer> GetReviewerById(int reviewerId);
        Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId);
        Task<bool> isReviewerExists(int reviewerId);
        Task<bool> CreateReviewer(Reviewer reviewer);
        Task<bool> UpdateReviewer(Reviewer reviewer);
        Task<bool> DeleteReviewer(Reviewer reviewer);
        Task<bool> Save();
    }
}
