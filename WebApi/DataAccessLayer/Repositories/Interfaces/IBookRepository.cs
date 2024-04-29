using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces
{
    public interface IBookRepository
    {
        IQueryable<BookModel> GetAllBooksQueryable();
        Task<BookModel> GetBookByIdAsync(int bookId);
        Task<IEnumerable<ReviewModel>> GetReviewsByBookId(int bookId);
        Task AddBookAsync(BookModel book);
        Task UpdateBookAsync(BookModel book);
        Task<bool> DeleteBookAsync(int bookId);
    }
}
