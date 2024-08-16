using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        { 
            _context = context;
        }

        async public Task<bool> isCategoryExists(int catId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == catId);
        }

        async public Task<ICollection<Category>> GetCategories()
        {
            return await _context.Categories.OrderBy(c => c.Id).ToListAsync();   
        }

        async public Task<Category> GetCategory(int catId)
        {
            return await _context.Categories.Where(c => c.Id == catId).FirstOrDefaultAsync();
        }

        async public Task<ICollection<Pokemon>> GetPokemonByCategory(int catId)
        {
            return await _context.PokemonCategories.Where(pc => pc.CategoryId == catId).Select(c => c.Pokemon).ToListAsync();
        }

        async public Task<bool> CreateCategory(Category category)
        {
            await _context.AddAsync<Category>(category);

            return await Save();

        }

        async public Task<bool> UpdateCategory(Category category)
        {
            _context.Update(category);
            return await Save();
        }

        async public Task<bool> DeleteCategory(Category category)
        {
            _context.Remove(category);
            return await Save();
        }

        async public Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
