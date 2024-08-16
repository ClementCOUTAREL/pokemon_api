using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto.Country;
using PokemonApi.Dto.Owner;
using PokemonApi.Interface;
using PokemonApi.Models;
using PokemonApi.Shared.Filters;
using PokemonApi.Shared.Validation.Country;
using PokemonApi.Shared.Validation.Owner;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {

        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

        async public Task<IActionResult> GetCountries()
        {
            var countries = _mapper.Map<List<GetCountryRequest>>(await _countryRepository.GetCountries());

            return Ok(countries);
        }

        [HttpGet("owners/{ownerId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(OwnerExistValidationAttribute))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Country))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        async public Task<IActionResult> GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<GetCountryRequest>(await _countryRepository.GetCountryByOwner(ownerId));

            return Ok(country);
        }

        [HttpGet("{countryId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(CountryExistsValidationAttribute))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        async public Task<IActionResult> GetOwnerFromCountry(int countryId)
        {

            var owners = _mapper.Map<List<GetOwnerRequest>>(await _countryRepository.GetOwnerFromCountry(countryId));

            return Ok(owners);
        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        async public Task<IActionResult> CreateCountry(CreateCountryRequest countryToCreate)
        {
            if (countryToCreate == null) return BadRequest(ModelState);

            var countries = await _countryRepository.GetCountries();
            var country = countries.Where(c => c.Name == countryToCreate.Name).FirstOrDefault();

            if(country != null)
            {
                ModelState.AddModelError("", "country already exists");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryToCreate);

            if (!await _countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(CountryExistsValidationAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        async public Task<IActionResult> UpdateCountry(int countryId, [FromBody] UpdateCountryRequest countryToUpdate)
        {
            if (countryToUpdate == null) return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryToUpdate);

            if(!await _countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode(StatusCodes.Status500InternalServerError,ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(CountryExistsValidationAttribute))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        async public Task<IActionResult> DeleteCountry(int countryId)
        {
            var country = await _countryRepository.GetCountry(countryId);

            if(!await _countryRepository.DeleteCountry(country))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError,ModelState);
            }

            return NoContent();
        }
    }
}
