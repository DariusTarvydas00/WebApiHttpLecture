using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.DTOs;

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

        public double[] GetBookVector(string isbn)
        {
            //var book = new SqlParameter("ISBN", isbn);
            FormattableString query = 
                $"""
                SELECT Users.Id, ISNULL(Reviews.Rating, 0) AS Rating 
                FROM Users 
                LEFT JOIN Reviews 
                ON Users.Id = Reviews.UserId AND Reviews.BookISBN = {isbn}
                """;

            //var userRatingsOnBook = _context.Users.FromSqlRaw(query, book);
            //var parameters = new object[] { book };

            //var parameters = new object[] { new SqlParameter("ISBN", isbn) }; // Use SqlParameter for SQL Server

            var queryResult = _context.Database.SqlQuery<QueryResultItem>(query);

            return queryResult.Select(r => (double)r.Rating).ToArray();
        }

        public async Task<Book> GetBookByIdAsync(string isbn)
        {
            return await _context.Books.Include(b => b.Reviews).FirstOrDefaultAsync(b => b.ISBN == isbn);
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

        public async Task<bool> DeleteBookAsync(string isbn)
        {
            var book = await _context.Books.FindAsync(isbn);
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
