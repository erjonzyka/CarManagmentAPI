using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CarManagment.Dto;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CountryController : Controller
    {
        private ICountryRepository _country { get; }
        public IMapper _mapper { get; }

        public CountryController(ICountryRepository country, IMapper mapper)
        {
            _country = country;
            _mapper = mapper;
        }

        [HttpGet("allcountries")]
        public IActionResult GetCountries()
        {
            var allCountries = _mapper.Map<List<CountryDto>>(_country.GetCountries());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(allCountries);
        }

        [HttpGet("getcountry/{id}")]
        public IActionResult GetCountry(int id)
        {
            var reqCountry = _mapper.Map<CountryDto>(_country.GetCountry(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reqCountry);
        }

        [HttpGet("get/owners/{id}")]
        public IActionResult GetCountryByOwner(int id)
        {
            var getCountry = _mapper.Map<CountryDto>(_country.GetCountryByOwner(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(getCountry);
        }

        [HttpGet("get/owners/byc/{id}")]
        public IActionResult GetOwnersByCountry(int id)
        {
            var getOwners = _mapper.Map<List<OwnerDto>>(_country.GetOwnersByCountry(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(getOwners);
        }

        [HttpPost]
        public IActionResult CreateCountry([FromBody] CountryDto country)
        {
            if (country == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCat = _country.GetCountries().Where(e => e.Name.ToLower() == country.Name.ToLower());

            if (existingCat.Any())
            {
                ModelState.AddModelError("", "Country already exists!");
                return StatusCode(422, ModelState);
            }

            var categoryMap = _mapper.Map<Country>(country);

            if (!_country.CreateCountry(categoryMap))
            {
                ModelState.AddModelError("", "Error saving changes!");
                return StatusCode(500, ModelState);
            }

            return Ok("Country succesfully created!");
        }

        [HttpPut("country/update/{countryId}")]
        public IActionResult UpdateCountry(int countryId, CountryDto updateCountry)
        {
            if(!ModelState.IsValid || updateCountry == null || updateCountry.CountryId != countryId)
            {
                return BadRequest(ModelState);
            }
            if (!_country.CountryExists(countryId)) ;
            var objMap = _mapper.Map<Country>(updateCountry);
            if (!_country.UpdateCountry(objMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated Successfully");

        }

        [HttpDelete("destroy/country/{id}")]
        public IActionResult DeleteCountry(int id)
        {
            if (!_country.CountryExists(id))
            {
                return NotFound();
            }
            var toDelete = _country.GetCountry(id);
            if (!_country.DeleteCountry(toDelete))
            {
                ModelState.AddModelError("", "Error while saving delete!");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted successfully!");
        }
    }
}
