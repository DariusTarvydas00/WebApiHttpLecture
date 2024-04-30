using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces;

public interface IReviewRepository
{
    public Review CreateReview(Review review);
    public Task<Review> CreateReviewAsync(Review review);
    public List<Review> GetAllReviews();
    public IQueryable<Review> GetReviews();
    public Review RemoveReview(int reviewToDeleteId);
    public Task<bool> DeleteReviewAsync(int reviewToDeleteId);
    public List<Review> GetReviewsByBookId(int bookId);
    public Task<IEnumerable<Review>> GetReviewsByBookIdAsync(int bookId);
    public List<Review> GetReviewsByUser(int userId);
    public Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
}