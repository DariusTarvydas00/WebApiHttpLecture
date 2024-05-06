using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;

namespace WebApi.ServiceLayer;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public List<Review> GetAllReviewsByBookId(string isbn)
    {
        var reviews = _reviewRepository.GetReviewsByBookId(isbn);
        return reviews;
    }
    public List<Review> GetAllReviews()
    {
        var reviews = _reviewRepository.GetAllReviews();
        return reviews;
    }

    public Review CreateNewReview(Review review)
    {
        var newreview = _reviewRepository.CreateReview(review);
        return newreview;
    }

    public Review RemoveReview(int reviewId)
    {
        return _reviewRepository.RemoveReview(reviewId);
    }


    public async Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(string isbn)
    {
        var reviews = await _reviewRepository.GetReviewsByBookIdAsync(isbn);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId)
    {
        var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<List<Review>> GetReviewsByBookIdEagerAsync(string isbn)
    {
        return await _reviewRepository.GetReviewsByBookIdAsync(isbn);
    }

    public async Task<List<Review>> GetReviewsByUserIdEagerAsync(int userId)
    {
        return await _reviewRepository.GetReviewsByUserIdAsync(userId);
    }

    public async Task<ReviewDto> AddReviewAsync(ReviewDto addReviewDto)
    {
        var review = _mapper.Map<Review>(addReviewDto);
        await _reviewRepository.CreateReviewAsync(review);
        var reviewDto = _mapper.Map<ReviewDto>(review);
        return reviewDto;
    }

    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        return await _reviewRepository.DeleteReviewAsync(reviewId);
    }
}