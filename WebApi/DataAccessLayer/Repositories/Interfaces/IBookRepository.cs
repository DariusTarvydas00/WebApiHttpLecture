using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces
{
    public interface IBookRepository
    {
        public IQueryable<Book> GetAllBooksQueryable();
        public double[] GetBookVector(string isbn);
        public Task<Book> GetBookByIdAsync(string isbn);
        public Task AddBookAsync(Book book);
        public Task UpdateBookAsync(Book book);
        public Task<bool> DeleteBookAsync(string isbn);
    }
}
