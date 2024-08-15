using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto.Pokemon;
using PokemonApi.Interface;
using PokemonApi.Models;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public PokemonController(
            IPokemonRepository pokemonRepository,
            IOwnerRepository ownerRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _categoryRepository = categoryRepository;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon(
            [FromQuery] int ownerId,
            [FromQuery] int catId,
            [FromBody] PokemonDto pokemonToCreate)
        {
            if(pokemonToCreate == null) return BadRequest(ModelState);

            var pokemon = _pokemonRepository
                .GetPokemons()
                .Where(p => p.Name.Trim().ToUpper() == pokemonToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "pokemon already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonToCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap)){
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePokemon(int pokeId, [FromQuery] int ownerId, int catId , [FromBody] PokemonToUpdateDto pokemonToUpdate)
        { 
            if(pokemonToUpdate == null) return BadRequest(ModelState);

            if (!_pokemonRepository.IsPokemonExists(pokeId)) return NotFound();
            if (!_categoryRepository.isCategoryExists(catId)) return NotFound();
            if (!_ownerRepository.isOwnerExists(ownerId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonToUpdate);

            if(!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.IsPokemonExists(pokeId)) return BadRequest(ModelState);
            var pokemon = _pokemonRepository.GetPokemon(pokeId);

            if (!_pokemonRepository.DeletePokemon(pokemon))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }

    }
}
