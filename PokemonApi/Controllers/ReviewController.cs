﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.Dto.Review;
using PokemonApi.Interface;
using PokemonApi.Models;
using PokemonApi.Shared.Filters;
using PokemonApi.Shared.Validation.Pokemon;
using PokemonApi.Shared.Validation.Review;
using System.Net;

namespace PokemonApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
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
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        async public Task<IActionResult> GetReviews()
        {
            var reviews = _mapper.Map<List<Review>>(await _reviewRepository.GetReviews());

            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ServiceFilter(typeof(ReviewExistsValidationAttribute))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Review))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        async public Task<IActionResult> GetReview(int reviewId)
        { 

            var review = _mapper.Map<Review>(await _reviewRepository.GetReviewById(reviewId));

            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ServiceFilter(typeof(PokemonExistsValidationAttribute))]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        async public Task<IActionResult> GetReviewsOfAPokemon(int pokeId)
        {
            var reviews = _mapper.Map<List<Review>>(await _reviewRepository.GetReviewsOfAPokemon(pokeId));

            return Ok(reviews);
        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        async public Task<IActionResult> CreateReview(
            [FromQuery] int reviewerId,
            [FromQuery] int pokeId,
            [FromBody] CreateReviewRequest reviewToCreate)
        { 
            if(reviewToCreate == null) return BadRequest(ModelState);

            if (await _pokemonRepository.IsPokemonExists(pokeId)) return NotFound("Pokemon not found");
            if (await _reviewerRepository.isReviewerExists(reviewerId)) return NotFound("Reviewer not found");
            
            var reviews = await _reviewRepository.GetReviews();
            var review = reviews.Where(r => r.Title.Trim().ToUpper() == reviewToCreate.Title.Trim().ToUpper()).FirstOrDefault();

            if(review != null)
            {
                ModelState.AddModelError("", "review already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }

            var reviewMap = _mapper.Map<Review>(reviewToCreate);

            reviewMap.Pokemon = await _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = await _reviewerRepository.GetReviewerById(reviewerId);

            if(!await _reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "An error occured while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created");

        }

        [HttpPut("{reviewId}")]
        [ServiceFilter(typeof(ReviewExistsValidationAttribute))]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        async public Task<IActionResult> UpdateReview(
            int reviewId,
            [FromQuery] int pokeId,
            [FromQuery] int reviewerId,
            [FromBody] UpdateReviewRequest reviewToUpdate)
        {
            if( reviewToUpdate == null) return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewToUpdate);

            reviewMap.Pokemon = await _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = await _reviewerRepository.GetReviewerById(reviewerId);

            if(!await _reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "An error occured while updating");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ServiceFilter(typeof(ReviewExistsValidationAttribute))]
        [ServiceFilter(typeof(ModelValidationAttributeFilter))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        async public Task<IActionResult> DeleteReview(int reviewId)
        {
            var review = await _reviewRepository.GetReviewById(reviewId);

            if (!await _reviewRepository.DeleteReview(review))
            {
                ModelState.AddModelError("", "An error occured while deleting");
                return StatusCode((int)HttpStatusCode.InternalServerError, ModelState);
            }

            return NoContent();
        }
    }
}
