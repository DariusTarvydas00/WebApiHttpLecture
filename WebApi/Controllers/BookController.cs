using Microsoft.AspNetCore.Mvc;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IReviewService _reviewService;

        public BookController(IBookService bookService, IReviewService reviewService)
        {
            _bookService = bookService;
            _reviewService = reviewService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseBookDto>>> GetAllBooks(string? title = null, string? author = null, string? keyword = null, bool? sortByRatingAscending = null, bool? sortByYearAscending = null, bool? sortByReviewsAscending = null)
        {
            var books = await _bookService.GetAllBooksAsync(title, author, keyword, sortByRatingAscending, sortByYearAscending, sortByReviewsAscending);
            return Ok(books);
        }


        [HttpGet("{isbn}")]
        public async Task<ActionResult<ResponseBookDto>> GetBookById(string isbn)
        {
            var book = await _bookService.GetBookByIdAsync(isbn);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        //[HttpGet("{id}/reviews")]
        //public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByBookId(int id)
        //{
        //    var reviews = await _bookService.GetReviewsByBookIdAsync(id);
        //    if (reviews == null || !reviews.Any())
        //    {
        //        return NotFound($"No reviews found for the book with ID {id}.");
        //    }
        //    return Ok(reviews);
        //    throw new System.NotImplementedException();
        //}


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseBookDto>> AddBook([FromBody] RequestBookDto requestBookDto)
        {
            var existingBook = await _bookService.GetBookByIdAsync(requestBookDto.ISBN);
            if (existingBook != null)
            {
                return Conflict("A book with the same ISBN already exists.");
            }
            var newBook = await _bookService.AddBookAsync(requestBookDto);
            return Ok(); 
                //CreatedAtAction(nameof(GetBookById), new { id = newBook.ISBN }, newBook);
        }

        [HttpPut("{isbn}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(string isbn, [FromBody] RequestBookDto requestBookDto)
        {
            var updatedBook = await _bookService.UpdateBookAsync(isbn, requestBookDto);
            if (updatedBook == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{isbn}")]
        [Authorize(Roles = "Admin")]    
        public async Task<IActionResult> DeleteBook(string isbn)
        {
            var success = await _bookService.DeleteBookAsync(isbn);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
