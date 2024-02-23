using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CarManagment.Dto;
using CarManagment.Helper;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : Controller
    {
        private ICarRepository _car { get; }
        private ICategoryRepository _category { get; }
        private IOwnerRepository _owner { get; }
        private IMapper _mapper { get; }
        public CarController(ICarRepository car, IMapper mapper, ICategoryRepository category, IOwnerRepository owner)
        {
            _car = car;
            _mapper = mapper;
            _category = category;
            _owner = owner;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Car>))]
        public IActionResult GetCars()
        {
            var cars = _mapper.Map<List<CarDto>>(_car.GetCars());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(cars);
        }

        [HttpGet("car/byid/{id}")]
        public IActionResult GetCar(int id)
        {
            if (!_car.CarExists(id))
            {
                return NotFound();
            }
            var requestedCar = _mapper.Map<CarDto>(_car.GetCar(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(requestedCar);
            }
            return Ok(requestedCar);
        }

        [HttpGet("car/byname/{name}")]
        public IActionResult GetCar(string name)
        {
            var requestedCar = _mapper.Map<CarDto>(_car.GetCar(name));
            if (!ModelState.IsValid)
            {
                return BadRequest(requestedCar);
            }
            return Ok(requestedCar);
        }

        [HttpGet("car/getreview/{id}")]
        public decimal GetCarReview(int id)
        {
            return _car.GetCarReview(id);
        }

        [HttpPost]
        public IActionResult CreateCar([FromQuery] int OwnerId, [FromQuery] int CategoryId, [FromBody] CarDto car)
        {
            if (car == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existing = _car.GetCars().Where(e => e.Make.ToLower() == car.Make.ToLower());
            if (!_category.CategoryExists(CategoryId) || !_owner.OwnerExists(OwnerId))
            {
                return NotFound("Owner or category found");
            }
            if (existing.Any())
            {
                ModelState.AddModelError("", "Car already exists!");
                return StatusCode(422, ModelState);
            }

            var objMap = _mapper.Map<Car>(car);


            if (!_car.CreateCar(OwnerId, CategoryId, objMap))
            {
                ModelState.AddModelError("", "Error saving changes!");
                return StatusCode(500, ModelState);
            }

            return Ok("Car succesfully created!");
        }

        [HttpPut("car/update/{id}")]
        public IActionResult UpdateCar(int id, [FromQuery] int OwnerId, [FromQuery] int CategoryId, [FromBody] CarDto updateCar)
        {
            if (!ModelState.IsValid || updateCar == null || updateCar.CarId != id)
            {
                return BadRequest(ModelState);
            }
            if (!_car.CarExists(id)) ;
            var objMap = _mapper.Map<Car>(updateCar);
            if (!_car.UpdateCar(OwnerId,CategoryId,objMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Car updated Successfully");
        }

        [HttpDelete("destroy/car/{id}")]
        public IActionResult DeleteCar(int id)
        {
            if (!_car.CarExists(id))
            {
                return NotFound();
            }
            var toDelete = _car.GetCar(id);
            if (!_car.DeleteCar(toDelete))
            {
                ModelState.AddModelError("", "Error while saving delete!");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted successfully!");
        }
    }
}
