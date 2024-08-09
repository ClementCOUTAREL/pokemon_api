using Microsoft.AspNetCore.Mvc;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("/api/[controller]")]
    [Controller]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        public PokemonController(IPokemonRepository pokemonRepository) 
        {
            _pokemonRepository = pokemonRepository;
         }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200,Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.IsPokemonExists(pokeId))
                return BadRequest();

            var pokemon = _pokemonRepository.GetPokemon(pokeId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemon);
        }


        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult getPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.IsPokemonExists(pokeId))
                return BadRequest();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            return Ok(rating);
        }
    }
}
