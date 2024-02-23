using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CarManagment.Dto;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private IReviewerRepository _reviewer { get; }
        private IMapper _mapper { get; }
        public ReviewerController(IReviewerRepository reviewer, IMapper mapper)
        {
            _reviewer = reviewer;
            _mapper = mapper;
              
        }

        [HttpGet("reviewers/all")]
        public IActionResult GetReviewers() { 
        var allRev = _mapper.Map<List<ReviewerDto>>(_reviewer.GetReviewers());
            if(allRev.Count() <= 0) { 
            return NotFound(allRev);
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(allRev);
        }

        [HttpGet("reviewer/{id}")]
        public IActionResult GetReviewer(int id)
        {
            if (!_reviewer.ReviewerExists(id))
            {
                return NotFound();
            }
            var reqRev = _mapper.Map<ReviewerDto>(_reviewer.GetReviewer(id));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(reqRev);

        }

        [HttpGet("reviewer/reviews/{id}")]
        public IActionResult GetReviewsByReviewer(int id)
        {
            var reqRev = _mapper.Map<List<ReviewDto>>(_reviewer.GetReviewsByReviewer(id));
            if(reqRev.Count() <= 0)
            {
                return NotFound();
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(reqRev);
            }
            return Ok(reqRev);
        }

        [HttpPost]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewer)
        {
            if (reviewer == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existing = _reviewer.GetReviewers().Where(e => e.FirstName.ToLower() == reviewer.LastName.ToLower() && e.FirstName.ToLower() == reviewer.LastName.ToLower());

            if (existing.Any())
            {
                ModelState.AddModelError("", "Reviewer already exists!");
                return StatusCode(422, ModelState);
            }

            var objMap = _mapper.Map<Reviewer>(reviewer);


            if (!_reviewer.CreateReviewer(objMap))
            {
                ModelState.AddModelError("", "Error saving changes!");
                return StatusCode(500, ModelState);
            }

            return Ok("Reviewer succesfully created!");
        }

        [HttpPut(("review/update/{id}"))]
        public IActionResult UpdateReviewer(int id,ReviewerDto updateReviewer)
        {
            if (!ModelState.IsValid || updateReviewer == null || updateReviewer.ReviewerId != id)
            {
                return BadRequest();
            }
            else if (!_reviewer.ReviewerExists(id))
            {
                return NotFound();
            }
            var objMap = _mapper.Map<Reviewer>(updateReviewer);
            if (!_reviewer.UpdateReviewer(objMap))
            {
                ModelState.AddModelError("","Error while saving!");
                return StatusCode(500, ModelState);
            }
            return Ok("Reviewer updated successfully!");
        }

        [HttpDelete("destroy/reviewer/{id}")]
        public IActionResult DeleteReviewer(int id)
        {
            if (!_reviewer.ReviewerExists(id))
            {
                return NotFound();
            }
            var toDelete = _reviewer.GetReviewer(id);
            if (!_reviewer.DeleteReviewer(toDelete))
            {
                ModelState.AddModelError("", "Error while saving delete!");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted successfully!");
        }
    }


}

