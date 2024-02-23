using CarManagment.Models;

namespace CarManagment.Interface
{
    public interface ICarRepository
    {
        ICollection <Car> GetCars ();
        Car GetCar (int id);
        Car GetCar (string name);
        bool CarExists (int id);
        decimal GetCarReview (int id);
        bool CreateCar(int OwnerId, int CategoryId, Car car);
        bool UpdateCar (int OwnerId,int CategoryId, Car car);
        bool DeleteCar (Car car);
        bool Save();
        

    }
}
