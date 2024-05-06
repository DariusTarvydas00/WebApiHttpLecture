using Accord.Math.Distances;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Numerics;
using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;


namespace WebApi.ServiceLayer
{
    public class RecommendationsService : IRecommendationsService
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IReviewService _reviewService;
        private readonly ILogger<RecommendationsService> _logger;
        private List<User> _allUsers;
        private List<Review> _allReviews;


        public RecommendationsService(IUserService userService, IBookService bookService, IReviewService reviewService, ILogger<RecommendationsService> logger)
        {
            _userService = userService;
            _bookService = bookService;
            _reviewService = reviewService;
            _logger = logger;
        }

        public async Task InitializeDataAsync()
        {
            _allUsers = _userService.GetAll().Result.ToList();
            _allReviews = _reviewService.GetAllReviews();
        }
        public async Task<List<Book>> GetRecommendations(string username)
        {
            //RecommendationDto recommendations = new RecommendationDto();
            int userId = (await _userService.GetByUserName(username)).Id;
            var allUserReviews = await _reviewService.GetReviewsByUserIdEagerAsync(userId);
            var goodUserBooks = allUserReviews.Where(r => r.Rating >= 8).OrderByDescending(r => r.Rating).Take(5).Select(r => r.Book);
            var unratedBooks = _bookService.GetAllBooksQueryableAsync().Where(b => !b.Reviews.Any(r => r.UserId == userId)).ToList();

            var recommendations = await GetRecommendationsByCosineDistanceAsync(goodUserBooks, unratedBooks);

            return recommendations;
        }

        public async Task<List<Book>> GetRecommendationsByCosineDistanceAsync(IEnumerable<Book> goodUserBooks, IEnumerable<Book> unratedBooks)
        {
            await InitializeDataAsync();
            Cosine cosine = new Cosine();
            ConcurrentDictionary<string, double> bookDistances = new ConcurrentDictionary<string, double>();
            List<Task> tasks = new List<Task>();
            foreach (var ratedBook in goodUserBooks)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var ratedBookVector = await GetBookVector(ratedBook.ISBN);
                    foreach (var unratedBook in unratedBooks)
                    {
                        var unratedBookVector = await GetBookVector(unratedBook.ISBN);
                        double distance = Math.Round(cosine.Distance(ratedBookVector, unratedBookVector), 9);
                        bookDistances.AddOrUpdate(unratedBook.ISBN, distance, (key, oldVal) => Math.Min(oldVal, distance));
                    }
                }));
            }
            await Task.WhenAll(tasks);

            var recommendedBooks = bookDistances.OrderBy(b => b.Value)
                                                .Take(5)
                                                .Select(b => unratedBooks.FirstOrDefault(x => x.ISBN == b.Key))
                                                .ToList();

            return recommendedBooks;
        }
        public async Task<double[]> GetBookVector(string isbn)
        {
            double[] bookVector = new double[_allUsers.Count];
            var reviews = _allReviews.Where(r => r.BookISBN == isbn).ToList();

            foreach (var review in reviews)
            {
                int index = _allUsers.FindIndex(u => u.Id == review.UserId);
                if (index != -1)
                {
                    bookVector[index] = review.Rating;
                }
            }

            return bookVector;
        }
    }

}

