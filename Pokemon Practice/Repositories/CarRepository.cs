using CarManagment.Helper;
using CarManagment.Interface;
using CarManagment.Models;
using System.Diagnostics.Metrics;

namespace CarManagment.Repositories
{
    public class CarRepository : ICarRepository
    {
        private  MyContext _context { get; }


        public CarRepository(MyContext context)
        {
            _context = context;
        }

        public ICollection<Car> GetCars()
        {
            return _context.Cars.OrderBy(e=>e.CarId).ToList();
        }

        public Car GetCar(int id)
        {
            return _context.Cars.FirstOrDefault(e => e.CarId == id);
        }

        public Car GetCar(string name)
        {
            return _context.Cars.FirstOrDefault(e => e.Make == name);
        }

        public bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        public decimal GetCarReview(int id)
        {
            var reviews = _context.Reviews.Where(e => e.Car.CarId == id);
            return (decimal)reviews.Sum(r => r.Rating)/ reviews.Count();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateCar(int OwnerId, int CategoryId, Car car)
        {
            var ownerEntity = _context.Owners.FirstOrDefault(e => e.OwnerId == OwnerId);
            var categoryEntity = _context.Categories.FirstOrDefault(e => e.CategoryId == CategoryId);
            Ownership ownership = new Ownership();
            ownership.Car = car;
            ownership.Owner = ownerEntity;
            _context.Add(car);
            CategoryJoin ctg = new CategoryJoin();
            ctg.Car = car;
            ctg.Category = categoryEntity;
            _context.Add(ctg);
            _context.Add(car);
            return Save();

        }

        public bool UpdateCar(int OwnerId, int CategoryId, Car car)
        {
           

            
            _context.Update(car);

            
            return Save();
        }


        public bool DeleteCar(Car car) {
            var referencingReviews = _context.Reviews.Where(review => review.Car.CarId == car.CarId);


            foreach (var review in referencingReviews)
            {
                _context.Remove(review);
            }
            _context.Remove(car);
            return Save();
        }


    }
}
