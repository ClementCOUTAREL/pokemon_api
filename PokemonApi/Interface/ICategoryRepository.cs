using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int catId);
        ICollection<Pokemon> GetPokemonByCategory(int catId);
        bool isCategoryExists(int catId);
    }
}
