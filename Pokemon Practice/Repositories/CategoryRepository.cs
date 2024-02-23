using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public MyContext _context { get; }
        public CategoryRepository(MyContext context)
        {
            _context = context;
        }


        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(e => e.CategoryId == id);
        }

        public ICollection<Car> GetCarByCategory(int id)
        {
            return _context.Cars.Include(e => e.CarCategories).Where(e => e.CarCategories.Any(cc => cc.CategoryId == id)).ToList();
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(e=> e.CategoryId == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();

        }
    }
}
