using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.ServiceLayer.Interfaces;

namespace WebApi.ServiceLayer;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public Review CreateNewReview(Review review)
    {
        var newreview = _reviewRepository.CreateReview(review);
        return newreview;
    }
    public List<Review> GetAllReviewsByBookId(int bookId)
    {
        var reviews = _reviewRepository.GetReviewsByBookId(bookId);
        return reviews;
    }

    public Review RemoveReview(int reviewId)
    {
        return _reviewRepository.RemoveReview(reviewId);
    }

    public async Task<List<Review>> GetReviewsByUser(int userId)
    {
        return await Task.FromResult(_reviewRepository.GetReviewsByUser(userId));
    }

}