using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;
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

    public Review CreateReview(Review review)
    {
        _mainDbContext.Reviews.Add(review);
        _mainDbContext.SaveChanges();
        return review;
    }

    public IQueryable<Review> GetAllReviews()
    {
        var reviews = _mainDbContext.Reviews;
        return reviews;
    }

    public List<Review> GetReviewsByBookId(string isbn)
    {
        var bookToReview = _mainDbContext.Reviews.FirstOrDefault(review => review.BookISBN == isbn);
        if (bookToReview == null)
        {
            return null;
        }
        var query = _mainDbContext.Reviews.AsQueryable();
        query = query.Where(r => r.BookISBN == isbn);
        return query.ToList();
    }

    public List<Review> GetReviewsByUserId(int userId)
    {
        return _mainDbContext.Reviews.Where(r => r.UserId == userId).ToList();
    }

    public Review RemoveReview(int reviewToDeleteId)
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


    //IQueryable<Review> IReviewRepository.GetReviews()
    //{
    //    return _mainDbContext.Reviews
    //        .Include(r => r.Reviews)
    //        .AsQueryable();
    //}

    public async Task<Review> CreateReviewAsync(Review review)
    {
        _mainDbContext.Reviews.Add(review);
        await _mainDbContext.SaveChangesAsync();
        return review;
    }

    public async Task<List<Review>> GetAllReviewsAsync()
    {
        return await _mainDbContext.Reviews.ToListAsync();
    }

    public async Task<List<Review>> GetReviewsByBookIdAsync(string isbn)
    {
        return await _mainDbContext.Reviews.Where(r => r.BookISBN == isbn)
                .Include(r => r.User)
                .Include(r => r.Book)
                .ToListAsync();
    }

    public async Task<List<Review>> GetReviewsByUserIdAsync(int userId)
    {
        return await _mainDbContext.Reviews.Where(r => r.UserId == userId)
                .Include(r => r.User)
                .Include(r => r.Book)
                .ToListAsync();
    }

    public async Task<bool> DeleteReviewAsync(int reviewToDeleteId)
    {
        var review = await _mainDbContext.Reviews.FindAsync(reviewToDeleteId);
        if (review != null)
        {
            _mainDbContext.Reviews.Remove(review);
            await _mainDbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}