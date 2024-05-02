using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }


    [HttpGet("{isbn}")]
    public async Task<ActionResult<ReviewDto>> GetReviewByBookId(string isbn)
    {
        try
        {
            var review = await _reviewService.GetReviewsByBookIdAsync(isbn);
            if (review == null)
            {
                return NotFound($"No reviews found for the book with ID {isbn}.");
            }
            return Ok(review);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to get review: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ReviewDto>> AddReview([FromBody] ReviewDto reviewDTO)
    {
        try
        {
            var newReview = await _reviewService.AddReviewAsync(reviewDTO);
            return Ok(newReview);
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to add review: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var success = await _reviewService.DeleteReviewAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

}