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

    public async Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(int bookId)
    {
        var reviews = await _reviewRepository.GetReviewsByBookIdAsync(bookId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByUserIdAsync(int userId)
    {
        var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
    }
}