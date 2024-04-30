using Microsoft.EntityFrameworkCore;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;

namespace WebApi.DataAccessLayer.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly MainDbContext _context;

        public BookRepository(MainDbContext context)
        {
            _context = context;
        }

        public IQueryable<Book> GetAllBooksQueryable()
        {

            return _context.Books
                .Include(b => b.Reviews)
                .AsQueryable();
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _context.Books.Include(b => b.Reviews).FirstOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByBookId(int bookId)
        {
            return await _context.Reviews.Where(r => r.BookId == bookId)
                .Include(r => r.User)
                .Include(r => r.Book)
                .ToListAsync();
        }

        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
