using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;

namespace WebApi.DataAccessLayer.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly MainDbContext _mainDbContext;
    public ReviewRepository(MainDbContext mainDbContext)
    {
        _mainDbContext = mainDbContext;
    }

    public ReviewModel CreateReview(ReviewModel review)
    {
        _mainDbContext.Reviews.Add(review);
        _mainDbContext.SaveChanges();
        return review;
    }

    public List<ReviewModel> GetAllReviews()
    {
        var reviews = _mainDbContext.Reviews.ToList();
        return reviews;
    }

    public ReviewModel RemoveReview(int reviewToDeleteId)
    {
        var review = _mainDbContext.Reviews.FirstOrDefault(r => r != null && r.Id == reviewToDeleteId);
        if (review == null)
        {
            return null;
        }
        _mainDbContext.Reviews.Remove(review);
        _mainDbContext.SaveChanges();
        return review;

    }

    public List<ReviewModel> GetReviewsByBookId(int bookId)
    {
        var bookToReview = _mainDbContext.Reviews.FirstOrDefault(review => review.BookId == bookId);
        if (bookToReview == null)
        {
            return null;
        }
        var query = _mainDbContext.Reviews.AsQueryable();
        query = query.Where(r => r.BookId == bookId);
        return query.ToList();
    }
}