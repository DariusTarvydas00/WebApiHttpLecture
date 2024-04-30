using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces;

public interface IReviewRepository
{
    public Review CreateReview(Review review);
    public List<Review> GetAllReviews();
    public Review RemoveReview(int reviewToDeleteId);
    public List<Review> GetReviewsByBookId(int bookId);
    public List<Review> GetReviewsByUser(int userId);
}