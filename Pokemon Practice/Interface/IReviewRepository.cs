using CarManagment.Models;

namespace CarManagment.Interface
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsOfCar(int id);
        bool ReviewExists(int id);

        bool CreateReview(int ReviewerId, Review review);
        bool DeleteReview(Review review);
        bool UpdateReview(Review review);
        bool Save();

    }
}
