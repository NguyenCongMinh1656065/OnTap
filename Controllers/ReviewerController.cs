using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ôn_tập.Dto;
using Ôn_tập.Interfaces;
using Ôn_tập.Models;

namespace Ôn_tập.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private IReviewerRepository _reviewerRepository;
        private IMapper _mapper;
        private IReviewRepository _reviewRepository;

        public ReviewerController(IReviewerRepository reviewerRepository,IMapper mapper , IReviewRepository reviewRepository) 
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }
        [HttpGet]
        public IActionResult GetReviewers() 
        {
            var reviewer = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            return Ok(reviewer);
        }
        [HttpGet("{reviewerId}")]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            var  reviews = _mapper.Map<List<ReviewDto>>(
                _reviewerRepository.GetReviewByReviewer(reviewerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpPost]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var country = _reviewerRepository.GetReviewers()
                .Where(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpDelete]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }    
            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
            }
            return NoContent();
        }
        [HttpPut]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody]ReviewerDto updateReviewer)
        {
            if (updateReviewer == null )
                return BadRequest(ModelState);
            if (reviewerId != updateReviewer.Id)
                return BadRequest(ModelState);
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var reviewerMap = _mapper.Map<Reviewer>(updateReviewer);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
