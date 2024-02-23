using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CarManagment.Dto;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        public OwnerController(IOwnerRepository owner, IMapper mapper, ICountryRepository country)
        {
            _owner = owner;
            _mapper = mapper;
            _country = country;
        }

        public IOwnerRepository _owner { get; }
        public IMapper _mapper { get; }
        public ICountryRepository _country { get; }

        [HttpGet("all/owners")]
        public IActionResult GetOwners() {
            var allOwners = _mapper.Map<List<OwnerDto>>(_owner.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(allOwners);
        }

        [HttpGet("get/owner/{id}")]
        public IActionResult GetOwner(int id) {
            if (!_owner.OwnerExists(id))
            {
                return NotFound();
            }
            var requestedOwner = _mapper.Map<OwnerDto>(_owner.GetOwner(id));
            if (!ModelState.IsValid)
            {
                return BadRequest();   
            }
            return Ok(requestedOwner);
        }

        [HttpGet("car/get/owners/{id}")]
        public IActionResult GetOwnersOfCar(int id) {
            var requestedCarOw = _mapper.Map<List<OwnerDto>>(_owner.GetOwnersOfCar(id));
            if(requestedCarOw.Count()<=0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(requestedCarOw);
        }

        [HttpGet("owner/get/cars/{id}")]
        public IActionResult GetCarsOfOwner(int id)
        {
            var requestedCarOw = _mapper.Map<List<CarDto>>(_owner.GetCarsOfOwner(id));
            if (requestedCarOw.Count() <= 0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(requestedCarOw);
        }

        [HttpPost]
        public IActionResult CreateOwner([FromQuery]int CountryId, [FromBody] OwnerDto owner)
        {
            if (owner == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existing = _owner.GetOwners().Where(e => e.Name.ToLower() == owner.Name.ToLower());

            if (existing.Any())
            {
                ModelState.AddModelError("", "Owner already exists!");
                return StatusCode(422, ModelState);
            }

            var objMap = _mapper.Map<Owner>(owner);

            objMap.Country = _country.GetCountry(CountryId);

            if (!_owner.CreateOwner(objMap))
            {
                ModelState.AddModelError("", "Error saving changes!");
                return StatusCode(500, ModelState);
            }

            return Ok("Owner succesfully created!");

        }


        [HttpPut("update/owner/{id}")]
        public IActionResult UpdateOwner(int id, [FromBody]OwnerDto updatedOwner)
        {
            if (updatedOwner == null || !ModelState.IsValid || updatedOwner.OwnerId != id)
            {
                return BadRequest(ModelState);
            }
            else if (!_owner.OwnerExists(id))
            {
                return NotFound();
            }
            var objMap = _mapper.Map<Owner>(updatedOwner);
            if (!_owner.UpdateOwner(objMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated successfully!");
        }

        [HttpDelete("destroy/owner/{id}")]
        public IActionResult DeleteOwner(int id)
        {
            if (!_owner.OwnerExists(id))
            {
                return NotFound();
            }
            var toDelete = _owner.GetOwner(id);
            if (!_owner.DeleteOwner(toDelete))
            {
                ModelState.AddModelError("", "Error while saving delete!");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted successfully!");
        }


    }

    
}
