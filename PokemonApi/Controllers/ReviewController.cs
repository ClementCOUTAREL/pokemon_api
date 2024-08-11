using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interface;
using PokemonApi.Models;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("api/[Controller]")]
    [Controller]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(
            IReviewRepository reviewRepository,
            IPokemonRepository pokemonRepository,
            IReviewerRepository reviewerRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<Review>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        { 
            if(!_reviewRepository.isReviewExists(reviewId)) return BadRequest();

            var review = _mapper.Map<Review>(_reviewRepository.GetReviewById(reviewId));

            if(!ModelState.IsValid) return BadRequest();

            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAPokemon(int pokeId)
        {
            if(!_pokemonRepository.IsPokemonExists(pokeId)) return BadRequest();

            var reviews = _mapper.Map<List<Review>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

            if (!ModelState.IsValid) return BadRequest();

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewToCreate)
        { 
            if(reviewToCreate == null) return BadRequest(ModelState);

            var review = _reviewRepository
                .GetReviews()
                .Where(r => r.Title.Trim().ToUpper() == reviewToCreate.Title.Trim().ToUpper())
                .FirstOrDefault();

            if(review != null)
            {
                ModelState.AddModelError("", "review already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewToCreate);

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewerById(reviewerId);

            if(!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateReview(int reviewId,[FromQuery] int pokeId, [FromQuery] int reviewerId, [FromBody] ReviewDto reviewToUpdate)
        {
            if( reviewToUpdate == null) return BadRequest(ModelState);
            if(!_reviewRepository.isReviewExists(reviewId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewToUpdate);

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewerById(reviewerId);

            if(!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.isReviewExists(reviewId)) return BadRequest(ModelState);
            var review = _reviewRepository.GetReviewById(reviewId);

            if (!_reviewRepository.DeleteReview(review))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }
    }
}
