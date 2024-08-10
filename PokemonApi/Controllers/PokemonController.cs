using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("/api/[controller]")]
    [Controller]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        public PokemonController(
            IPokemonRepository pokemonRepository,
            IMapper mapper) 
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
         }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

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

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));

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
