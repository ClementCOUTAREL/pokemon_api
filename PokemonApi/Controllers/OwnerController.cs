using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interface;
using PokemonApi.Models;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class OwnerController : Controller
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.isOwnerExists(ownerId)) return BadRequest();

            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid) return BadRequest();

            return Ok(owner);
        }

        [HttpGet("pokemons/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerOfAPokemon(int pokeId)
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnerOfAPokemon(pokeId));

            if (!ModelState.IsValid) return BadRequest();

            return Ok(owners);
        }

        [HttpGet("{ownerId}/pokemons")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonOfAnOwner(int ownerId)
        {
            if (!_ownerRepository.isOwnerExists(ownerId)) return BadRequest();

            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonOfAnOwner(ownerId));

            if (!ModelState.IsValid) return BadRequest();

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerToCreate)
        {
            if (ownerToCreate == null) return BadRequest(ModelState);

            var owner = _ownerRepository.GetOwners().Where(o => o.LastName.Trim().ToUpper() == ownerToCreate.LastName.Trim().ToUpper()).FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "owner already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerToCreate);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto ownerToUpdate)
        {
            if (ownerToUpdate == null) return BadRequest(ModelState);

            if (!_ownerRepository.isOwnerExists(ownerId)) return BadRequest(ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerToUpdate);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.isOwnerExists(ownerId)) return BadRequest(ModelState);
            var owner = _ownerRepository.GetOwner(ownerId);

            if (!_ownerRepository.DeleteOwner(owner))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }
    }
}
