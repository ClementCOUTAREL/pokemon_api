using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto;
using PokemonApi.Interface;
using PokemonApi.Models;
using PokemonApi.Repository;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("api/[Controller]")]
    [Controller]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid) return BadRequest();

            return Ok(reviewers);

        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewers(int reviewerId)
        {
            if(!_reviewerRepository.isReviewerExists(reviewerId)) return BadRequest();

            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewerById(reviewerId));

            if (!ModelState.IsValid) return BadRequest();

            return Ok(reviewer);

        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfReviewer(int reviewerId)
        {
            if (!_reviewerRepository.isReviewerExists(reviewerId)) return BadRequest();

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid) return BadRequest();

            return Ok(reviews);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerToCreate)
        {
            if(reviewerToCreate == null) return BadRequest(ModelState);

            var reviewer = _reviewerRepository
                .GetReviewers()
                .Where(r => r.LastName.Trim().ToUpper() ==  reviewerToCreate.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "reviewer already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerToCreate);

            if(!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateReviewer(int reviewerId,  [FromBody] ReviewerDto reviewerToUpdate)
        {

            if (reviewerToUpdate == null) return BadRequest(ModelState);

            if (!_reviewerRepository.isReviewerExists(reviewerId)) return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerToUpdate);

            if(!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode((int)HttpStatusCode.InternalServerError,ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult DeletePokemon(int reviewerId)
        {
            if (!_reviewerRepository.isReviewerExists(reviewerId)) return BadRequest(ModelState);
            var reviewer = _reviewerRepository.GetReviewerById(reviewerId);

            if (!_reviewerRepository.DeleteReviewer(reviewer))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }

    }
}
