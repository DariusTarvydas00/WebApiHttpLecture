using Microsoft.AspNetCore.Mvc;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
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
        public async Task<ActionResult<ResponseBookDto>> AddBook([FromBody] RequestBookDto requestBookDto)
        {
            var newBook = await _bookService.AddBookAsync(requestBookDto);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.ISBN }, newBook);
        }

        [HttpPut("{isbn}")]
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
