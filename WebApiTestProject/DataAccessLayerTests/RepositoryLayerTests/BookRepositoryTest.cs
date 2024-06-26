﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccessLayer;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories;


namespace WebApiTestProject.DataAccessLayerTests.RepositoryLayerTests
{
    public class BookRepositoryTest : IDisposable
    {
        private readonly DbContextOptions<MainDbContext> _contextOptions;
        private MainDbContext _context;

        public BookRepositoryTest()
        {
            _contextOptions = new DbContextOptionsBuilder<MainDbContext>().Options;
                
                // .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                // .Options;

            InitializeContext();
            SeedDatabase();
        }

        private void InitializeContext()
        {
            _context = new MainDbContext(_contextOptions);
        }

        private void SeedDatabase()
        {
            _context.Books.Add(new Book { ISBN = "1", Title = "1984", Author = "George Orwell", PublicationYear = 1949 });
            _context.Books.Add(new Book { ISBN = "2", Title = "The Hobbit", Author = "J.R.R. Tolkien", PublicationYear = 1937 });
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
        [Fact]
        public async Task GetAllBooksQueryable_ReturnsAllBooks()
        {
            using (var context = new MainDbContext(_contextOptions))
            {
                var repository = new BookRepository(context);
                var books = repository.GetAllBooksQueryable();

                Assert.Equal(2, await books.CountAsync());
            }
        }
        [Fact]
        public async Task GetBookByIdAsync_ReturnsBook_WhenBookExists()
        {
            using (var context = new MainDbContext(_contextOptions))
            {
                var repository = new BookRepository(context);
                var book = await repository.GetBookByIdAsync("1");

                Assert.NotNull(book);
                Assert.Equal("1984", book.Title);
            }
        }
        [Fact]
        public async Task GetBookByIdAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            using (var context = new MainDbContext(_contextOptions))
            {
                var repository = new BookRepository(context);
                var book = await repository.GetBookByIdAsync("10");

                Assert.Null(book);
            }
        }
        [Fact]
        public async Task AddBookAsync_AddsBookCorrectly()
        {
            using (var context = new MainDbContext(_contextOptions))
            {
                var repository = new BookRepository(context);
                var book = new Book { Title = "Brave New World", Author = "Aldous Huxley", PublicationYear = 1932 };

                await repository.AddBookAsync(book);
                var addedBook = context.Books.FirstOrDefault(b => b.Title == "Brave New World");

                Assert.NotNull(addedBook);
                Assert.Equal("Aldous Huxley", addedBook.Author);
            }
        }
        [Fact]
        public async Task UpdateBookAsync_UpdatesDataCorrectly()
        {
            using (var context = new MainDbContext(_contextOptions))
            {
                var repository = new BookRepository(context);
                var book = await context.Books.FirstOrDefaultAsync(b => b.ISBN == "1");
                book.Title = "Nineteen Eighty-Four";

                await repository.UpdateBookAsync(book);

                var updatedBook = await context.Books.FindAsync(1);
                Assert.Equal("Nineteen Eighty-Four", updatedBook.Title);
            }
        }
        // [Fact]
        // public async Task DeleteBookAsync_DeletesBookCorrectly()
        // {
        //     // using (var context = new MainDbContext(_contextOptions))
        //     // {
        //     //     var repository = new BookRepository(context);
        //     //     var book = await context.Books.FirstOrDefaultAsync(b => b.Id == 1);
        //     //
        //     //     var result = await repository.DeleteBookAsync(book.Id);
        //     //
        //     //     Assert.True(result);
        //     //     Assert.Null(await context.Books.FindAsync(1));
        //     // }
        // }
    }

}