using Microsoft.EntityFrameworkCore;
using CarManagment.Interface;
using CarManagment.Models;

namespace CarManagment.Repositories
{
    public class ReviewerRepository : IReviewerRepository
    {
        private MyContext _context { get; }
        public ReviewerRepository(MyContext context) {
            _context = context;
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.OrderBy(e=> e.ReviewerId).ToList();
        }

        public Reviewer GetReviewer(int id)
        {
            return _context.Reviewers.FirstOrDefault(e => e.ReviewerId == id);
        }

        public ICollection<Review> GetReviewsByReviewer(int id)
        {
            return _context.Reviews.Where(e=> e.Reviewer.ReviewerId ==id).ToList(); 
        }

        public bool ReviewerExists(int id)
        {
            return _context.Reviewers.Any(e=> e.ReviewerId==id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer) { 
        _context.Remove(reviewer);
            return Save();
        }

    }
}
