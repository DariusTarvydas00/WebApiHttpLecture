using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces;

public interface IReviewRepository
{
    public Review CreateReview(Review review);
    public List<Review> GetAllReviews();
    public List<Review> GetReviewsByBookId(string isbn);
    public List<Review> GetReviewsByUserId(int userId);
    public Review RemoveReview(int reviewToDeleteId);

    public Task<Review> CreateReviewAsync(Review review);
    public Task<List<Review>> GetAllReviewsAsync();
    public Task<List<Review>> GetReviewsByBookIdAsync(string isbn);
    public Task<List<Review>> GetReviewsByUserIdAsync(int userId);
    public Task<bool> DeleteReviewAsync(int reviewToDeleteId);
}