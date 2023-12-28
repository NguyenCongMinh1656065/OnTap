using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ôn_tập.Dto;
using Ôn_tập.Interfaces;
using Ôn_tập.Models;

namespace Ôn_tập.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]     
    public class ReviewController : ControllerBase
    {
        private IReviewRepository _reviewRepository;
        private IMapper _mapper;
        private IPokemonRepository _pokemonRepository;
        private IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository , IMapper mapper , IPokemonRepository pokemonRepository , IReviewerRepository reviewerRepository) 
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }
        [HttpGet("pokemon/{pokeId}")]
        public IActionResult GetReviewForAPokemon(int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));
            if (!ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }
        [HttpGet("reviewId")]
        public IActionResult GetPokemon(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpPost]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery]int pokeId, [FromBody]ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);
            var reviews = _reviewRepository.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.Trim().ToUpper())
                .FirstOrDefault();
            if (reviews == null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            } 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong wile create");
                return StatusCode(422, ModelState);
            }    
            return Ok("Successfully create");
        }
        [HttpPut("{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody]ReviewDto updateReview)
        {
            if (updateReview == null) 
                return BadRequest(ModelState); 
            if (reviewId != updateReview.Id) 
                return BadRequest(ModelState);
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(updateReview);
            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
    }
}
