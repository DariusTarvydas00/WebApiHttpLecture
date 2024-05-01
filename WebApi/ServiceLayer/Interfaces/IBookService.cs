using WebApi.ServiceLayer.DTOs;

namespace WebApi.ServiceLayer.Interfaces
{
    public interface IBookService
    {
        public Task<IEnumerable<ResponseBookDto>> GetAllBooksAsync(string? title, string? author, string? keyword, bool? sortByRatingAscending, bool? sortByYearAscending, bool? sortByReviewsAscending);
        public Task<ResponseBookDto> GetBookByIdAsync(string isbn);
        public Task<ResponseBookDto> AddBookAsync(RequestBookDto requestBookDto);
        public Task<ResponseBookDto> UpdateBookAsync(string isbn, RequestBookDto requestBookDto);
        public Task<bool> DeleteBookAsync(string isbn);
    }
}
