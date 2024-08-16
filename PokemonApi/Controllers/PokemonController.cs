using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto.Pokemon;
using PokemonApi.Interface;
using PokemonApi.Models;
using PokemonApi.Shared.Filters;
using PokemonApi.Shared.Validation.Owner;
using PokemonApi.Shared.Validation.Pokemon;
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
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        async public Task<IActionResult> GetPokemons()
        {
            var pokemons = _mapper.Map<List<GetPokemonRequest>>(await _pokemonRepository.GetPokemons());

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(PokemonExistsValidationAttribute))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pokemon))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<IActionResult> GetPokemon(int pokeId)
        {
            var pokemon = _mapper.Map<GetPokemonRequest>(await _pokemonRepository.GetPokemon(pokeId));

            return Ok(pokemon);
        }


        [HttpGet("{pokeId}/rating")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(PokemonExistsValidationAttribute))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<IActionResult> getPokemonRating(int pokeId)
        {
            var rating = await _pokemonRepository.GetPokemonRating(pokeId);

            return Ok(rating);
        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<IActionResult> CreatePokemon(
            [FromQuery] int ownerId,
            [FromQuery] int catId,
            [FromBody] CreatePokemonRequest pokemonToCreate)
        {
            if(pokemonToCreate == null) return BadRequest(ModelState);

            var pokemons = await _pokemonRepository.GetPokemons();

            var pokemon = pokemons.Where(p => p.Name.Trim().ToUpper() == pokemonToCreate.Name.Trim().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "pokemon already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonToCreate);

            if (!await _pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap)){
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{pokeId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(PokemonExistsValidationAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<IActionResult> UpdatePokemon(
            int pokeId,
            [FromQuery] int ownerId,
            int catId ,
            [FromBody] UpdatePokemonRequest pokemonToUpdate)
        { 
            if(pokemonToUpdate == null) return BadRequest(ModelState);

            if (!await _categoryRepository.isCategoryExists(catId)) return NotFound("Category not found");
            if (!await _ownerRepository.isOwnerExists(ownerId)) return NotFound("owner not found");

            var pokemonMap = _mapper.Map<Pokemon>(pokemonToUpdate);

            if(!await _pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{pokeId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(PokemonExistsValidationAttribute))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        async public Task<IActionResult> DeletePokemon(int pokeId)
        {
            var pokemon = await _pokemonRepository.GetPokemon(pokeId);

            if (!await _pokemonRepository.DeletePokemon(pokemon))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }

    }
}
