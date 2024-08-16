using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetPokemons();
        Task<Pokemon> GetPokemon(int pokeId);
        Task<Pokemon> GetPokemon(string name);
        Task<decimal> GetPokemonRating(int pokeId);
        Task<bool> IsPokemonExists(int pokeId);
        Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> UpdatePokemon(Pokemon pokemon);
        Task<bool> DeletePokemon(Pokemon pokemon);
        Task<bool> Save();
    }
}
