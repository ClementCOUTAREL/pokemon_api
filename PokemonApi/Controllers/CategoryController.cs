using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto.Category;
using PokemonApi.Dto.Pokemon;
using PokemonApi.Interface;
using PokemonApi.Models;
using PokemonApi.Shared.Filters;
using PokemonApi.Shared.Validation.Category;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        async public Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [TypeFilter(typeof(CategoryExistsAttribute))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(Category))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        async public Task<IActionResult> GetCategory(int categoryId)
        {
            var category = _mapper.Map<GetCategoryRequest>(await _categoryRepository.GetCategory(categoryId));

            return Ok(category);
        }

        [HttpGet("{categoryId}/pokemon")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [TypeFilter(typeof(CategoryExistsAttribute))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        async public Task<IActionResult> GetPokemonByCategoryId(int categoryId)
        {

            var pokemons = _mapper.Map<List<GetPokemonRequest>>(await _categoryRepository.GetPokemonByCategory(categoryId));

            return Ok(pokemons);
        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        async public Task<IActionResult> CreateCategory(
            [FromBody] CreateCategoryRequest categoryToCreate)
        {
            var categories = await _categoryRepository.GetCategories();
            var category = categories
                .Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "category already exists");
                return StatusCode(422, ModelState);
            }
            
            var categoryMap = _mapper.Map<Category>(categoryToCreate);

            if (!await _categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something wrong happened while saving");
                return StatusCode(500, ModelState);
            };

            return Ok("Successfully created");
        }

        [HttpPut("{categoryId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        async public Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequest categoryToUpdate)
        {
            if (categoryToUpdate == null) return BadRequest(ModelState);


            if (await _categoryRepository.GetCategory(categoryId) == null) return NotFound(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryToUpdate);

            if (!await _categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [TypeFilter(typeof(CategoryExistsAttribute))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        async public Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _categoryRepository.GetCategory(categoryId);

            if(!await _categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }
    }
}
