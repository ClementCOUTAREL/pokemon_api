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

        public bool isCategoryExists(int catId)
        {
            return _context.Categories.Any(c => c.Id == catId);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Id).ToList();   
        }

        public Category GetCategory(int catId)
        {
            return _context.Categories.Where(c => c.Id == catId).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int catId)
        {
            return _context.PokemonCategories.Where(pc => pc.CategoryId == catId).Select(c => c.Pokemon).ToList();
        }

        public bool CreateCategory(Category category)
        {
            _context.Add<Category>(category);

            return Save();

        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }
    }
}
