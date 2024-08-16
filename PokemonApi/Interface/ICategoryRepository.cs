using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetCategories();
        Task<Category> GetCategory(int catId);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int catId);
        Task<bool> isCategoryExists(int catId);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(Category category);
        Task<bool> Save();
    }
}
