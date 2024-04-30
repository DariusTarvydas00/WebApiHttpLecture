using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces
{
    public interface IBookRepository
    {
        public IQueryable<Book> GetAllBooksQueryable();
        public Task<Book> GetBookByIdAsync(int bookId);
        public Task<IEnumerable<Review>> GetReviewsByBookId(int bookId);
        public Task AddBookAsync(Book book);
        public Task UpdateBookAsync(Book book);
        public Task<bool> DeleteBookAsync(int bookId);
    }
}
