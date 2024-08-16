using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PokemonApi.Data;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context) 
        {
            _context = context;
        }

        async public Task<ICollection<Pokemon>> GetPokemons()
        {
            return await _context.Pokemons.OrderBy(p => p.Id).ToListAsync();
        }

       async public Task<Pokemon> GetPokemon(int pokeId)
        {
            return await _context.Pokemons.Where(p => p.Id == pokeId).FirstOrDefaultAsync();
        }

        async public Task<Pokemon> GetPokemon(string name)
        {
            return await _context.Pokemons.Where(p => p.Name == name).FirstOrDefaultAsync();
        }

        async public Task<decimal> GetPokemonRating(int pokeId)
        {
            var reviews =await _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToListAsync();

            if (reviews.Count() <= 0 )
            {
                return 0;
            }

            return ((decimal)reviews.Sum(r => r.Rating));
        }

        async public Task<bool> IsPokemonExists(int pokeId)
        {
            return await _context.Pokemons.AnyAsync(p => p.Id == pokeId);
        }

        async public Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = await _context.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();

            var category = await _context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _context.Add(pokemonCategory);

            _context.Add(pokemon);

            return await Save();
        }

        async public Task<bool> UpdatePokemon( Pokemon pokemon)
        {
            _context.Update(pokemon);
            return await Save();
        }

        async public Task<bool> DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return await Save();
        }

        async public Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
