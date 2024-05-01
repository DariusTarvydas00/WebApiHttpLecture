using AutoMapper;
using Moq;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer;

namespace WebApiTestProject.ServiceLayerTests
{
    public class BookServiceTest
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BookService _service;

        public BookServiceTest()
        {
            _mockRepo = new Mock<IBookRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new BookService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBookDto_WhenBookExists()
        {
            // Arrange
            var book = new Book { ISBN = "0123456789", Title = "Sample Book", Author = "Author", PublicationYear = 2020 };
            var bookDto = new ResponseBookDto { ISBN = "0123456789", Title = "Sample Book", Author = "Author", PublicationYear = 2020 };

            _mockRepo.Setup(x => x.GetBookByIdAsync("0123456789")).ReturnsAsync(book);
            _mockMapper.Setup(x => x.Map<ResponseBookDto>(book)).Returns(bookDto);

            // Act
            var result = await _service.GetBookByIdAsync("0123456789");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Sample Book", result.Title);
            _mockRepo.Verify(x => x.GetBookByIdAsync("0123456789"), Times.Once);
        }

        //[Fact]
        //public async Task GetReviewsByBookId_ReturnsReviewDtos_WhenReviewsExist()
        //{
        //    // Arrange
        //    var reviews = new List<Review>
        //    {
        //        new Review { Id = 1, UserId = 1, BookId = 1, Rating = 5, ReviewText = "Great!" }
        //    };
        //    var reviewDtos = new List<ReviewDto>
        //    {
        //        new ReviewDto { Id = 1, UserId = "1", BookId = 1, Rating = 5, ReviewText = "Great!" }
        //    };

        //    _mockRepo.Setup(x => x.GetReviewsByBookId(1)).ReturnsAsync(reviews);
        //    _mockMapper.Setup(x => x.Map<IEnumerable<ReviewDto>>(reviews)).Returns(reviewDtos);

        //    // Act
        //    var result = await _service.GetReviewsByBookIdAsync(1);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Single(result);
        //    _mockRepo.Verify(x => x.GetReviewsByBookId(1), Times.Once);
        //}

        [Fact]
        public async Task AddBookAsync_ReturnsBookDto_WhenBookIsAdded()
        {
            // Arrange
            var requestBookDto = new RequestBookDto { Title = "New Book", Author = "New Author", PublicationYear = 2021 };
            var book = new Book { ISBN = "0123456789", Title = "New Book", Author = "New Author", PublicationYear = 2021 };
            var bookDto = new ResponseBookDto { ISBN = "0123456789", Title = "New Book", Author = "New Author", PublicationYear = 2021 };

            _mockMapper.Setup(m => m.Map<Book>(requestBookDto)).Returns(book);
            _mockMapper.Setup(m => m.Map<ResponseBookDto>(book)).Returns(bookDto);
            _mockRepo.Setup(r => r.AddBookAsync(book)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddBookAsync(requestBookDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Book", result.Title);
        }

        [Fact]
        public async Task UpdateBookAsync_ReturnsUpdatedBookDto_WhenBookExists()
        {
            // Arrange
            var book = new Book { ISBN = "0123456789", Title = "Old Title", Author = "TEST Author", PublicationYear = 2020 };
            var requestBookDto = new RequestBookDto { ISBN = "9876543210", Title = "Updated Title", Author = "TEST Author", PublicationYear = 2020 };
            var updatedBookDto = new ResponseBookDto { ISBN = "9876543210", Title = "Updated Title", Author = "TEST Author", PublicationYear = 2020 };

            _mockRepo.Setup(x => x.GetBookByIdAsync("0123456789")).ReturnsAsync(book);
            _mockRepo.Setup(x => x.UpdateBookAsync(book)).Returns(Task.CompletedTask);
            _mockMapper.Setup(x => x.Map(requestBookDto, book)).Returns(book);
            _mockMapper.Setup(x => x.Map<ResponseBookDto>(book)).Returns(updatedBookDto);

            // Act
            var result = await _service.UpdateBookAsync("0123456789", requestBookDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Title", result.Title);
            _mockRepo.Verify(x => x.UpdateBookAsync(book), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_ReturnsTrue_WhenBookDeleted()
        {
            // Arrange
            _mockRepo.Setup(x => x.DeleteBookAsync("0123456789")).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteBookAsync("0123456789");

            // Assert
            Assert.True(result);
            _mockRepo.Verify(x => x.DeleteBookAsync("0123456789"), Times.Once);
        }
    }
}