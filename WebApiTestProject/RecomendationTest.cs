using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.Interfaces;
using WebApi.ServiceLayer;
using WebApi.ServiceLayer.DTOs;
using AutoMapper;

namespace WebApiTestProject
{
    public class RecomendationTest
    {
        private readonly Mock<IUserService> _mockUserService = new Mock<IUserService>();
        private readonly Mock<IBookService> _mockBookService = new Mock<IBookService>();
        private readonly Mock<IReviewService> _mockReviewService = new Mock<IReviewService>();
        private readonly Mock<ILogger<RecommendationsService>> _mockLogger = new Mock<ILogger<RecommendationsService>>();

        [Fact]
        public async Task GetRecommendations_ReturnsAccurateRecommendationsBasedOnUserPreferences()
        {
            // Arrange
            var userService = _mockUserService.Object;
            var bookService = _mockBookService.Object;
            var reviewService = _mockReviewService.Object;
            var logger = _mockLogger.Object;

            var recommendationsService = new RecommendationsService(userService, bookService, reviewService, logger);

            string username = "user1";
            int userId = 1;

            var users = new List<User>
            {
                new User { Id = 1, Username = "user1" },
                new User { Id = 2, Username = "user2" },
                new User { Id = 3, Username = "user3" },
                new User { Id = 4, Username = "user4" }
            };

            var books = new List<Book>
            {
                new Book { ISBN = "123", Title = "Book 1" },
                new Book { ISBN = "456", Title = "Book 2" },
                new Book { ISBN = "789", Title = "Book 3" },
                new Book { ISBN = "999", Title = "Book 4" },
                new Book { ISBN = "555", Title = "Book 5" },  // patiko 1 user
                new Book { ISBN = "666", Title = "Book 6" }   // norim kad šita rekomenduotu
            };

            var reviews = new List<Review>
            {
                // User 1 ratings
                new Review { UserId = 1, BookISBN = "123", Rating = 4 },
                new Review { UserId = 1, BookISBN = "456", Rating = 5 },
                new Review { UserId = 1, BookISBN = "555", Rating = 9 },

                // User 2 ratings
                new Review { UserId = 2, BookISBN = "123", Rating = 5 },
                new Review { UserId = 2, BookISBN = "456", Rating = 3 },
                new Review { UserId = 2, BookISBN = "789", Rating = 8 },
                new Review { UserId = 2, BookISBN = "666", Rating = 9 },  // panašus rating

                // User 3 ratings
                new Review { UserId = 3, BookISBN = "789", Rating = 2 },
                new Review { UserId = 3, BookISBN = "999", Rating = 1 },
                new Review { UserId = 3, BookISBN = "555", Rating = 8 },
                new Review { UserId = 3, BookISBN = "666", Rating = 8 },  // panašus rating

                // User 4 ratings
                new Review { UserId = 4, BookISBN = "123", Rating = 5 },
                new Review { UserId = 4, BookISBN = "999", Rating = 7 },
                new Review { UserId = 4, BookISBN = "555", Rating = 7 },
                new Review { UserId = 4, BookISBN = "666", Rating = 7 }   // panašus rating
            };

            //mappinu
            foreach (var review in reviews)
            {
                var book = books.FirstOrDefault(b => b.ISBN == review.BookISBN);
                if (book != null)
                {
                    review.Book = book;
                    book.Reviews.Add(review);
                }
            }
            //paruošiam setuppus
            _mockUserService.Setup(s => s.GetAll()).ReturnsAsync(users);
            _mockReviewService.Setup(s => s.GetAllReviews()).Returns(reviews);
            _mockReviewService.Setup(s => s.GetAllReviewsByBookId(It.IsAny<string>())).Returns((string isbn) => reviews.Where(r => r.BookISBN == isbn).ToList());

            _mockUserService.Setup(s => s.GetByUserName(username)).ReturnsAsync(new User { Id = userId });
            _mockReviewService.Setup(s => s.GetReviewsByUserIdEagerAsync(userId)).ReturnsAsync(reviews.Where(r => r.UserId == userId).ToList());
            _mockBookService.Setup(s => s.GetAllBooksQueryableAsync()).Returns(books.AsQueryable());
            _mockBookService.Setup(s => s.GetBookByIdAsync(It.IsAny<string>())).Returns((string isbn) =>
            {
                var book = books.FirstOrDefault(b => b.ISBN == isbn);
                if (book == null)
                    return Task.FromResult<ResponseBookDto>(null);
                return Task.FromResult(new ResponseBookDto { ISBN = book.ISBN, Title = book.Title });
            });


            // Act
            var result = await recommendationsService.GetRecommendations(username);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.Equal("Book 6", result[0].Title);
        }
    }
}