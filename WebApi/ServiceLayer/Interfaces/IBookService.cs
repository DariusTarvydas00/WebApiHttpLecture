using WebApi.ServiceLayer.DTOs;

namespace WebApi.ServiceLayer.Interfaces
{
    public interface IBookService
    {
        public Task<IEnumerable<BookDto>> GetAllBooksAsync(string? title, string? author, string? keyword, bool? sortByRatingAscending, bool? sortByYearAscending, bool? sortByReviewsAscending);

        public Task<BookDto> GetBookByIdAsync(int bookId);
        //public Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(int bookId);
        public Task<BookDto> AddBookAsync(AddBookDto addBookDto);
        public Task<BookDto> UpdateBookAsync(int bookId, UpdateBookDto updateBookDto);
        public Task<bool> DeleteBookAsync(int bookId);
    }
}
