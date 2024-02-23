using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CarManagment.Dto;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        public IReviewRepository _review { get; }
        public IReviewerRepository _reviewer { get; }
        public IMapper _mapper { get; }
        public ReviewController(IReviewRepository review, IReviewerRepository reviewer, IMapper mapper)
        {
            _review = review;
            _reviewer = reviewer;
            _mapper = mapper;
        }

        [HttpGet("reviews/all")]
        public IActionResult GetReviews() {
            var allReviews = _mapper.Map<List<ReviewDto>>(_review.GetReviews());
            if (allReviews.Count <= 0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(allReviews);
        }

        [HttpGet("reviews/{id}")]
        public IActionResult GetReview(int id)
        {
            var reqReview = _mapper.Map<ReviewDto>(_review.GetReview(id));
            if (!ModelState.IsValid)
            {
                return BadRequest();   
            }
            return Ok(reqReview);
        }

        [HttpGet("car/review/{id}")]
        public IActionResult GetReviewsOfCar(int id)
        {
            var revCar = _mapper.Map<List<ReviewDto>>(_review.GetReviewsOfCar(id));
            if(revCar.Count <= 0)
            {
                return NotFound(ModelState);
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(revCar);
        }

        [HttpPost]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromBody] ReviewDto review)
        {
            if (review == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewer.ReviewerExists(reviewerId))
            {
                return NotFound("Reviewer does not exist!");
            }
            var objMap = _mapper.Map<Review>(review);


            if (!_review.CreateReview(reviewerId, objMap))
            {
                ModelState.AddModelError("", "Error saving changes!");
                return StatusCode(500, ModelState);
            }

            return Ok("Review succesfully submitted!");
        }


        [HttpPut("review/update/{reviewId}")]
        public IActionResult UpdateReview(int reviewId, ReviewDto updateReview)
        {
            if (!ModelState.IsValid || updateReview == null || updateReview.ReviewId != reviewId)
            {
                return BadRequest(ModelState);
            }
            if (!_review.ReviewExists(reviewId)) ;
            var objMap = _mapper.Map<Review>(updateReview);
            if (!_review.UpdateReview(objMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Review Updated Successfully");

        }

        [HttpDelete("destroy/review/{id}")]
        public IActionResult DeleteCountry(int id)
        {
            if (!_review.ReviewExists(id))
            {
                return NotFound();
            }
            var toDelete = _review.GetReview(id);
            if (!_review.DeleteReview(toDelete))
            {
                ModelState.AddModelError("", "Error while saving delete!");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted successfully!");
        }
    }

}

