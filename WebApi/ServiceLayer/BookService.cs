using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;

namespace WebApi.ServiceLayer
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(string? title, string? author, string? keyword, bool? sortByRatingAscending, bool? sortByYearAscending, bool? sortByReviewsAscending)
        {
            var query = _bookRepository.GetAllBooksQueryable();


            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(b => EF.Functions.Like(b.Title, $"%{title}%"));
            }
            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(b => EF.Functions.Like(b.Author, $"%{author}%"));
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(b => EF.Functions.Like(b.Title, $"%{keyword}%") || EF.Functions.Like(b.Author, $"%{keyword}%"));
            }


            var books = await query.Include(b => b.Reviews).ToListAsync();


            var bookDtos = books.Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublicationYear = book.PublicationYear,
                AverageRating = book.Reviews.Any() ? book.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = book.Reviews.Count
            });


            if (sortByYearAscending.HasValue)
            {
                bookDtos = sortByYearAscending.Value ? bookDtos.OrderBy(b => b.PublicationYear) : bookDtos.OrderByDescending(b => b.PublicationYear);
            }
            if (sortByReviewsAscending.HasValue)
            {
                bookDtos = sortByReviewsAscending.Value ? bookDtos.OrderBy(b => b.ReviewCount) : bookDtos.OrderByDescending(b => b.ReviewCount);
            }
            if (sortByRatingAscending.HasValue)
            {
                bookDtos = sortByRatingAscending.Value ? bookDtos.OrderBy(b => b.AverageRating) : bookDtos.OrderByDescending(b => b.AverageRating);
            }

            return bookDtos.ToList();
        }


        public async Task<BookDto> GetBookByIdAsync(int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            return book != null ? _mapper.Map<BookDto>(book) : null;
        }

        //public async Task<IEnumerable<ReviewDto>> GetReviewsByBookIdAsync(int bookId)
        //{
        //    var reviews = await _bookRepository.GetReviewsByBookId(bookId);
        //    return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        //}

        public async Task<BookDto> AddBookAsync(AddBookDto addBookDto)
        {
            var book = _mapper.Map<Book>(addBookDto);
            await _bookRepository.AddBookAsync(book);
            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }

        public async Task<BookDto> UpdateBookAsync(int bookId, UpdateBookDto updateBookDto)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null) return null;
            _mapper.Map(updateBookDto, book);
            await _bookRepository.UpdateBookAsync(book);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            return await _bookRepository.DeleteBookAsync(bookId);
        }
    }
}
