using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;

namespace WebApiTestProject.ControllerTests
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> _mockService;
        private readonly BookController _controller;

        public BookControllerTest()
        {
            _mockService = new Mock<IBookService>();
            _controller = new BookController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkObjectResult_WithAListOfBooks()
        {
            // Arrange
            var bookDtos = new List<BookDto>
        {
            new BookDto { Id = 1, Title = "Book 1", Author = "Author 1", PublicationYear = 2021 },
            new BookDto { Id = 2, Title = "Book 2", Author = "Author 2", PublicationYear = 2022 }
        };

            _mockService.Setup(s => s.GetAllBooksAsync(null, null, null, null, null, null))
                        .ReturnsAsync(bookDtos);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<BookDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _mockService.Verify(s => s.GetAllBooksAsync(null, null, null, null, null, null), Times.Once);
        }
        [Fact]
        public async Task GetBookById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var controller = new BookController(mockService.Object);
            mockService.Setup(s => s.GetBookByIdAsync(It.IsAny<int>()))
                       .ReturnsAsync((BookDto)null);

            // Act
            var result = await controller.GetBookById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkObjectResult_WithBook()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var controller = new BookController(mockService.Object);
            var bookDto = new BookDto { Id = 1, Title = "Existing Book", Author = "Existing Author", PublicationYear = 2020 };

            mockService.Setup(s => s.GetBookByIdAsync(1))
                       .ReturnsAsync(bookDto);

            // Act
            var result = await controller.GetBookById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<BookDto>(okResult.Value);
            Assert.Equal(bookDto.Id, returnValue.Id);
        }

        [Fact]
        public async Task AddBook_ReturnsCreatedAtAction_WithNewBook()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var controller = new BookController(mockService.Object);
            var addBookDto = new AddBookDto { Title = "New Book", Author = "New Author", PublicationYear = 2021 };
            var bookDto = new BookDto { Id = 1, Title = "New Book", Author = "New Author", PublicationYear = 2021 };

            mockService.Setup(s => s.AddBookAsync(addBookDto)).ReturnsAsync(bookDto);

            // Act
            var result = await controller.AddBook(addBookDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<BookDto>(actionResult.Value);
            Assert.Equal("GetBookById", actionResult.ActionName);
            Assert.Equal(bookDto.Id, returnValue.Id);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContent_WhenBookIsUpdated()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var controller = new BookController(mockService.Object);
            var updateBookDto = new UpdateBookDto { Title = "Updated Title", Author = "Updated Author", PublicationYear = 2022 };
            var bookDto = new BookDto { Id = 1, Title = "Updated Title", Author = "Updated Author", PublicationYear = 2022 };

            mockService.Setup(s => s.UpdateBookAsync(1, updateBookDto)).ReturnsAsync(bookDto);

            // Act
            var result = await controller.UpdateBook(1, updateBookDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task UpdateBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var controller = new BookController(mockService.Object);
            var updateBookDto = new UpdateBookDto { Title = "Non-existent Title", Author = "Non-existent Author", PublicationYear = 2025 };

            mockService.Setup(s => s.UpdateBookAsync(1, updateBookDto)).ReturnsAsync((BookDto)null);

            // Act
            var result = await controller.UpdateBook(1, updateBookDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
