using Microsoft.EntityFrameworkCore;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        public ReviewRepository(MyContext context) {
            _context = context;
        }

        private MyContext _context { get; }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(e=> e.ReviewId).ToList();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.FirstOrDefault(e => e.ReviewId == id);
        }

        public ICollection<Review> GetReviewsOfCar(int id)
        {
            return _context.Reviews.Include(e => e.Car).Where(e=> e.Car.CarId == id).ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateReview(int reviewerId, Review review)
        {
            var reviewerEntity = _context.Reviewers.FirstOrDefault(e => e.ReviewerId == reviewerId);
            review.Reviewer = reviewerEntity;
            _context.Add(review);
            return Save();
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }

        public bool DeleteReview(Review review) {
            _context.Remove(review);
            return Save();
        }
    }
}
