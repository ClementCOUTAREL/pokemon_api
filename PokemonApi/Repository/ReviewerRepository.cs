using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        { 
            _context = context; 
        }

        async public Task<ICollection<Reviewer>> GetReviewers()
        {
           return await _context.Reviewers.OrderBy(r => r.Id).ToListAsync();
        }

        async public Task<Reviewer> GetReviewerById(int reviewerId)
        {
            return await _context.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefaultAsync();
        }

        async public Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId)
        {
            return await _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToListAsync();
        }

        async public Task<bool> isReviewerExists(int reviewerId)
        {
            return await _context.Reviewers.AnyAsync(r => r.Id == reviewerId);
        }

        async public Task<bool> CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return await Save();
        }

        async public Task<bool> UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return await Save();
        }

        async public Task<bool> DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return await Save();
        }

        async public Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
