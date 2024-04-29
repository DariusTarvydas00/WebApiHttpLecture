using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces;

public interface IReviewRepository
{
    ReviewModel CreateReview(ReviewModel review);
    List<ReviewModel> GetAllReviews();
    ReviewModel RemoveReview(int reviewToDeleteId);
    List<ReviewModel> GetReviewsByBookId(int bookId);


}