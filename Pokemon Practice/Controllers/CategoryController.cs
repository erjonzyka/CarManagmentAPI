using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CarManagment.Dto;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CategoryController : Controller
    {
        private ICategoryRepository _category { get; }
        public IMapper _mapper { get; }

        public CategoryController(ICategoryRepository category, IMapper mapper)
        {
            _category = category;
            _mapper = mapper;
        }

        [HttpGet("allcateg")]
        public IActionResult GetCategories() {
            var allCategories = _mapper.Map<List<CategoryDto>>(_category.GetCategories());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(allCategories);
        }

        [HttpGet("getcat/{id}")]
        public IActionResult GetCategory(int id)
        {
            var requestedCategory = _mapper.Map<CategoryDto>(_category.GetCategory(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(requestedCategory);
        }

        [HttpGet("cars/cat/{id}")]
        public IActionResult GetCarByCategory(int id) {
        var requestedCars = _mapper.Map<List<CarDto>>(_category.GetCarByCategory(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(requestedCars);
        }

        [HttpGet("categ/check/{id}")]
        public IActionResult CategoryExists(int id) { 
        return Ok(_category.CategoryExists(id));
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto category)
        {
            if(category == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingCat = _category.GetCategories().Where(e => e.Name.ToLower() == category.Name.ToLower());

            if (existingCat.Any()) {
                ModelState.AddModelError("", "Category already exists!");
                return StatusCode(422, ModelState);
            }

            var categoryMap = _mapper.Map<Category>(category);

            if (!_category.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Error saving changes!");
                return StatusCode(500, ModelState);
            }

            return Ok("Category succesfully created!");

        }

        [HttpPut("update/category/{id}")]
        public IActionResult UpdateCategory(int id,[FromBody] CategoryDto updatedCategory) { 
        if(updatedCategory == null || !ModelState.IsValid || updatedCategory.CategoryId != id)
            {
                return BadRequest(ModelState);
            }
        else if (!_category.CategoryExists(id))
            {
                return NotFound();
            }
        var objMap = _mapper.Map<Category>(updatedCategory);
            if (!_category.UpdateCategory(objMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated successfully!");


        }

        [HttpDelete("destroy/category/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            if (!_category.CategoryExists(id))
            {
                return NotFound();
            }
            var toDelete = _category.GetCategory(id);
            if (!_category.DeleteCategory(toDelete))
            {
                ModelState.AddModelError("", "Error while saving delete!");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted successfully!");
        }
    }
}
