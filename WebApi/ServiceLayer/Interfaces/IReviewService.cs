using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;


namespace WebApi.ServiceLayer.Interfaces;

public interface IReviewService
{
    public List<Review> GetAllReviewsByBookId(string isbn);
    public Review CreateNewReview(Review review);
    public Review RemoveReview(int reviewId);
    public Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(string isbn);
    public Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId);
    public Task<List<Review>> GetReviewsByBookIdEagerAsync(string isbn);
    public Task<List<Review>> GetReviewsByUserIdEagerAsync(int userId);
    public Task<ReviewDto> AddReviewAsync(ReviewDto addReviewDto);
    public Task<bool> DeleteReviewAsync(int reviewId);

}