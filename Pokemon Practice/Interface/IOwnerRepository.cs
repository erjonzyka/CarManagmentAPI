using CarManagment.Models;

namespace CarManagment.Interface
{
    public interface IOwnerRepository
    {
        ICollection <Owner> GetOwners ();
        Owner GetOwner (int id);
        ICollection<Owner> GetOwnersOfCar (int CarId);
        ICollection<Car> GetCarsOfOwner(int OwnerId);
        bool OwnerExists(int OwnerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}
