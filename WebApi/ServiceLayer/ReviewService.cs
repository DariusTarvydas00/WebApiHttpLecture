using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.ServiceLayer.DTOs;

namespace WebApi.ServiceLayer;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public ReviewModel CreateNewReview(ReviewModel review)
    {
        var newreview = _reviewRepository.CreateReview(review);
        return newreview;
    }
    public List<ReviewModel> GetAllReviewsByBookId(int bookId)
    {
        var reviews = _reviewRepository.GetReviewsByBookId(bookId);
        return reviews;
    }

    public ReviewModel RemoveReview(int reviewId)
    {
        return _reviewRepository.RemoveReview(reviewId);
    }


}