using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataAccessLayer.Models;
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
    [HttpPost]
    public IActionResult AddReview([FromBody] Review review)
    {
        try
        {
            return Ok(_reviewService.CreateNewReview(review));
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to add review: {ex.Message}");
        }

    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]

    public ActionResult RemoveReview(int id)
    {
        return Ok(_reviewService.RemoveReview(id));

    }
}