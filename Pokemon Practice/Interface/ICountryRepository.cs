using CarManagment.Models;

namespace CarManagment.Interface
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        Country GetCountryByOwner(int OwnerId);
        ICollection<Owner> GetOwnersByCountry(int countryid);
        bool CreateCountry (Country country);
        bool UpdateCountry (Country country);
        bool DeleteCountry (Country country);
        bool CountryExists(int id);
        bool Save();
    }
}
