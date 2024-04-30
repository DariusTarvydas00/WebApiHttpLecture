using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer.Interfaces;

public interface IReviewService
{
    public Review CreateNewReview(Review review);
    public List<Review> GetAllReviewsByBookId(int bookId);
    public Review RemoveReview(int reviewId);
}