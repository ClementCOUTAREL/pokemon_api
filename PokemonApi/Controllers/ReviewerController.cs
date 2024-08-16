using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto.Review;
using PokemonApi.Dto.Reviewer;
using PokemonApi.Interface;
using PokemonApi.Models;
using PokemonApi.Repository;
using PokemonApi.Shared.Filters;
using PokemonApi.Shared.Validation.Reviewer;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        async public Task<IActionResult> GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(await _reviewerRepository.GetReviewers());

            return Ok(reviewers);

        }

        [HttpGet("{reviewerId}")]
        [ServiceFilter(typeof(ReviewerExistsValidationFilter))]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        async public Task<IActionResult> GetReviewers(int reviewerId)
        {
            var reviewer = _mapper.Map<ReviewerDto>(await _reviewerRepository.GetReviewerById(reviewerId));

            return Ok(reviewer);

        }

        [HttpGet("{reviewerId}/reviews")]
        [ServiceFilter(typeof(ReviewerExistsValidationFilter))]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        async public Task<IActionResult> GetReviewsOfReviewer(int reviewerId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(await _reviewerRepository.GetReviewsByReviewer(reviewerId));

            return Ok(reviews);

        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        async public Task<IActionResult> CreateReviewer([FromBody] ReviewerDto reviewerToCreate)
        {
            if(reviewerToCreate == null) return BadRequest(ModelState);

            var reviewers = await _reviewerRepository.GetReviewers();

            var reviewer = reviewers.Where(r => r.LastName.Trim().ToUpper() ==  reviewerToCreate.LastName.Trim().ToUpper()).FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "reviewer already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerToCreate);

            if(!await _reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ServiceFilter(typeof(ReviewerExistsValidationFilter))]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
       async public Task<IActionResult> UpdateReviewer(int reviewerId,  [FromBody] ReviewerDto reviewerToUpdate)
        {

            if (reviewerToUpdate == null) return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerToUpdate);

            if(!await _reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode((int)HttpStatusCode.InternalServerError,ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ServiceFilter(typeof(ReviewerExistsValidationFilter))]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        async public Task<IActionResult> DeletePokemon(int reviewerId)
        {
            var reviewer = await _reviewerRepository.GetReviewerById(reviewerId);

            if (!await _reviewerRepository.DeleteReviewer(reviewer))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }

    }
}
