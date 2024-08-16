using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        public ReviewRepository(DataContext context)
        {
            _context = context;
        }
        async public Task<ICollection<Review>> GetReviews()
        {
            return await _context.Reviews.OrderBy(r => r.Id).ToListAsync();
        }

        async public Task<Review> GetReviewById(int reviewId)
        {
            return await _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefaultAsync();
        }

        async public Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId)
        {
            return await _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToListAsync();
        }

        async public Task<bool> isReviewExists(int reviewId)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == reviewId);
        }

        async public Task<bool> CreateReview(Review review)
        {
            _context.Add(review);
            return await Save();
        }
       
        async public Task<bool> UpdateReview(Review review)
        {
            _context.Update(review);
            return await Save();
        }

        async public Task<bool> DeleteReview(Review review)
        {
            _context.Remove(review);
            return await Save();
        }
        async public Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

    }
}
