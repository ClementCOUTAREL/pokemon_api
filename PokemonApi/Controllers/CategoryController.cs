using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interface;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categories);
        }

        [HttpGet("{catId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public IActionResult GetCategory(int catId)
        {

            if (!_categoryRepository.isCategoryExists(catId))
                return BadRequest();

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(catId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(category);
        }

        [HttpGet("{catId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int catId)
        {

            if (!_categoryRepository.isCategoryExists(catId))
                return BadRequest();

            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonByCategory(catId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory(
            [FromBody] CategoryDto categoryToCreate)
        {

            if(categoryToCreate == null) return BadRequest(ModelState);

            var category = _categoryRepository
                .GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();
        
            if(category != null)
            {
                ModelState.AddModelError("", "category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryToCreate);

            if(!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something wrong happened while saving");
                return StatusCode(500, ModelState);
            };

            return Ok("Successfully created");
        }
    }
}
