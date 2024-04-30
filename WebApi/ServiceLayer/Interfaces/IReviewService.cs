using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;


namespace WebApi.ServiceLayer.Interfaces;

public interface IReviewService
{
    public Review CreateNewReview(Review review);
    public List<Review> GetAllReviewsByBookId(int bookId);
    public Review RemoveReview(int reviewId);
    public Task<List<Review>> GetReviewsByUser(int userId);
    public Task<ReviewDto> AddReviewAsync(ReviewDto addReviewDto);
    public Task<bool> DeleteReviewAsync(int reviewId);
    public Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(int bookId);
    public Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId);

}