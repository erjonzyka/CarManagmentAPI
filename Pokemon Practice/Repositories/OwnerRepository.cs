using Microsoft.EntityFrameworkCore;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Repositories
{
    public class OwnerRepository :IOwnerRepository
    {
        public MyContext _context { get; }
        public OwnerRepository(MyContext context)
        {
            _context = context;
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.OrderBy(e=> e.OwnerId).ToList();
        }

        public Owner GetOwner (int id)
        {
            return _context.Owners.FirstOrDefault(e => e.OwnerId == id);
        }

        public ICollection<Owner> GetOwnersOfCar(int CarId)
        {
            return _context.Owners.Include(e=> e.Cars).Where(e=> e.Cars.Any(e=> e.CarId == CarId)).ToList();
        }

        public ICollection<Car> GetCarsOfOwner(int id)
        {
            return _context.Cars.Include(e=> e.CarOwners).Where(e=> e.CarOwners.Any(e=> e.OwnerId == id)).ToList();
        }

        public bool OwnerExists(int id) {
            return _context.Owners.Any(e => e.OwnerId == id);
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        
    }
}
