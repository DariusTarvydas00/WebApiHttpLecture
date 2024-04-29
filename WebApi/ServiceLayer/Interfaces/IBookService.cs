using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using WebApi.ServiceLayer.DTOs;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace WebApi.ServiceLayer
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(string? title, string? author, string? keyword, bool? sortByRatingAscending, bool? sortByYearAscending, bool? sortByReviewsAscending);

        Task<BookDto> GetBookByIdAsync(int bookId);
        Task<IEnumerable<ReviewDto>> GetReviewsByBookId(int bookId);
        Task<BookDto> AddBookAsync(AddBookDto addBookDto);
        Task<BookDto> UpdateBookAsync(int bookId, UpdateBookDto updateBookDto);
        Task<bool> DeleteBookAsync(int bookId);
    }
}
