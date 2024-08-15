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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();

            return Ok(categories);
        }

        [HttpGet("{catId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [TypeFilter(typeof(CheckCategoryExistsAttribute))]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetCategory(int catId)
        {
            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(catId));

            return Ok(category);
        }

        [HttpGet("{catId}/pokemon")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [TypeFilter(typeof(CheckCategoryExistsAttribute))]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int catId)
        {

            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategory(catId));

            return Ok(pokemons);
        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory(
            [FromBody] CreateCategoryRequest categoryToCreate)
        {
            var category = _categoryRepository
                .GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "category already exists");
                return StatusCode(422, ModelState);
            }
            
            var categoryMap = _mapper.Map<Category>(categoryToCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
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
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryToUpdate)
        {
            if (categoryToUpdate == null) return BadRequest(ModelState);

            if (_categoryRepository.GetCategory(categoryId) == null) return NotFound(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryToUpdate);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{catId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [TypeFilter(typeof(CheckCategoryExistsAttribute))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult DeleteCategory(int catId)
        {
            var category = _categoryRepository.GetCategory(catId);

            if(!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }

    }

}
