using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer;
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
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks(string? title = null, string? author = null, string? keyword = null, bool? sortByRatingAscending = null, bool? sortByYearAscending = null, bool? sortByReviewsAscending = null)
        {
            var books = await _bookService.GetAllBooksAsync(title, author, keyword, sortByRatingAscending, sortByYearAscending, sortByReviewsAscending);
            return Ok(books);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByBookId(int id)
        {
            var reviews = await _bookService.GetReviewsByBookId(id);
            if (reviews == null || !reviews.Any())
            {
                return NotFound($"No reviews found for the book with ID {id}.");
            }
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> AddBook([FromBody] AddBookDto addBookDto)
        {
            var newBook = await _bookService.AddBookAsync(addBookDto);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
        {

            var updatedBook = await _bookService.UpdateBookAsync(id, updateBookDto);
            if (updatedBook == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var success = await _bookService.DeleteBookAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
