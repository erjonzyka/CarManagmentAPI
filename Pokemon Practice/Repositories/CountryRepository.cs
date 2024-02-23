using Microsoft.EntityFrameworkCore;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Repositories
{
   
    public class CountryRepository : ICountryRepository
    {
        public MyContext _context { get; }
        public CountryRepository(MyContext context)
        {
            _context = context;
        }



        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(e=> e.CountryId).ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.FirstOrDefault(e => e.CountryId == id);
        }

        public ICollection<Owner> GetOwnersByCountry(int countryid)
        {
            return _context.Owners
                    .Include(e => e.Country)
                    .Where(e => e.CountryId == countryid)
                    .ToList();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Countries
                    .Include(e => e.Owners)
                    .FirstOrDefault(e => e.Owners.Any(owner => owner.OwnerId == ownerId));
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }

        public bool DeleteCountry(Country country) {
            var referencingOwners = _context.Owners.Where(o => o.CountryId == country.CountryId);
            foreach (var owner in referencingOwners)
            {
                owner.CountryId = null; 
                _context.Update(owner);
            }
            _context.Remove(country);
            return Save();
        }

        public bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.CountryId == id);
        }
    }
}
