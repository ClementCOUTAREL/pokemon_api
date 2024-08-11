﻿using PokemonApi.Models;

namespace PokemonApi.Interface
{
    public interface IPokemonRepository
    {
        public ICollection<Pokemon> GetPokemons();
        public Pokemon GetPokemon(int pokeId);
        public Pokemon GetPokemon(string name);
        public decimal GetPokemonRating(int pokeId);
        public bool IsPokemonExists(int pokeId);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();
    }
}
