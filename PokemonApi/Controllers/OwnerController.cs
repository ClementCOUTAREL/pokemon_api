using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto.Owner;
using PokemonApi.Dto.Pokemon;
using PokemonApi.Interface;
using PokemonApi.Models;
using PokemonApi.Shared.Filters;
using PokemonApi.Shared.Validation.Country;
using PokemonApi.Shared.Validation.Owner;
using PokemonApi.Shared.Validation.Pokemon;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public OwnerController(
            IOwnerRepository ownerRepository,
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        async public Task<IActionResult> GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(await _ownerRepository.GetOwners());

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(OwnerExistValidationAttribute))]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        async public Task<IActionResult> GetOwner(int ownerId)
        {
            var owner = _mapper.Map<OwnerDto>(await _ownerRepository.GetOwner(ownerId));

            return Ok(owner);
        }

        [HttpGet("pokemons/{pokeId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(PokemonExistsValidationAttribute))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        async public Task<IActionResult> GetOwnerOfAPokemon(int pokeId)
        {
            var owners = _mapper.Map<List<OwnerDto>>(await _ownerRepository.GetOwnerOfAPokemon(pokeId));

            return Ok(owners);
        }

        [HttpGet("{ownerId}/pokemons")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(OwnerExistValidationAttribute))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        async public Task<IActionResult> GetPokemonOfAnOwner(int ownerId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(await _ownerRepository.GetPokemonOfAnOwner(ownerId));

            return Ok(pokemons);
        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
       async public Task<IActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerToCreate)
        {
            if (ownerToCreate == null) return BadRequest(ModelState);

            if(!await _countryRepository.isCountryExists(countryId)) return UnprocessableEntity("country doesn't exist");

            var owners = await _ownerRepository.GetOwners();
            var owner = owners.Where(o => o.LastName.Trim().ToUpper() == ownerToCreate.LastName.Trim().ToUpper()).FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "owner already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerToCreate);

            ownerMap.Country = await _countryRepository.GetCountry(countryId);

            if (!await _ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{ownerId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(OwnerExistValidationAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        async public Task<IActionResult> UpdateOwner(int ownerId, [FromBody] OwnerDto ownerToUpdate)
        {
            if (ownerToUpdate == null) return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerToUpdate);

            if (!await _ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(OwnerExistValidationAttribute))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        async public Task<IActionResult> DeleteOwner(int ownerId)
        {
            var owner = await _ownerRepository.GetOwner(ownerId);

            if (!await _ownerRepository.DeleteOwner(owner))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }
    }
}
