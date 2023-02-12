using Microsoft.EntityFrameworkCore;

using Web.Data;
using Web.Models;

namespace Web.Services
{
    
    public class ReviewService
    {
        private readonly DataContext _dataContext;
        public ReviewService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void addReview(Review review)
        {
            _dataContext.Reviews.Add(review);
            _dataContext.SaveChanges();
        }
        public List<Review> getAllReviews(int id)
        {
            return _dataContext.Reviews.Where(review => review.recipeID == id).ToList();
        }
    }
}
