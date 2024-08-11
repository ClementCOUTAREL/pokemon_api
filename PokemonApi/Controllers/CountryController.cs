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
    public class CountryController : Controller
    {

        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(countries);
        }

        [HttpGet("owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerFromCountry(int countryId)
        {

            if (!_countryRepository.isCountryExists(countryId))
                return BadRequest();

            var owners = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnerFromCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry(CountryDto countryToCreate)
        {
            if (countryToCreate == null) return BadRequest(ModelState);

            var country = _countryRepository.GetCountries().Where(c => c.Name == countryToCreate.Name).FirstOrDefault();

            if(country != null)
            {
                ModelState.AddModelError("", "country already exists");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryToCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryToUpdate)
        {
            if (countryToUpdate == null) return BadRequest(ModelState);

            if (!_countryRepository.isCountryExists(countryId)) return NotFound();

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryToUpdate);

            if(!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode(StatusCodes.Status500InternalServerError,ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.isCountryExists(countryId)) return BadRequest(ModelState);
            var country = _countryRepository.GetCountry(countryId);

            if(!_countryRepository.DeleteCountry(country))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError,ModelState);
            }

            return NoContent();
        }
    }
}
