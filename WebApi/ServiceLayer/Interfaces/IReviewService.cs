using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer;

public interface IReviewService
{
    ReviewModel CreateNewReview(ReviewModel review);
    List<ReviewModel> GetAllReviewsByBookId(int bookId);
    public ReviewModel RemoveReview(int reviewId);
}