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
    //[HttpPost]
    //public IActionResult AddReview([FromBody] Review review)
    //{
    //    try
    //    {
    //        return Ok(_reviewService.CreateNewReview(review));
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest($"Failed to add review: {ex.Message}");
    //    }

    //}
    //[HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]

    //public ActionResult RemoveReview(int id)
    //{
    //    return Ok(_reviewService.RemoveReview(id));

    //}

    [HttpPost]
    public async Task<ActionResult<ReviewDto>> AddReview([FromBody] ReviewDto reviewDTO)
    {
        var newReview = await _reviewService.AddReviewAsync(reviewDTO);
        return Ok(newReview);
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