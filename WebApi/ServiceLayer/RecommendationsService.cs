using Accord.Math.Distances;
using Microsoft.Extensions.Logging;
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

        public RecommendationsService(IUserService userService, IBookService bookService, IReviewService reviewService, ILogger<RecommendationsService> logger)
        {
            _userService = userService;
            _bookService = bookService;
            _reviewService = reviewService;
            _logger = logger;
        }

        public async Task<RecommendationDto> GetRecommendations(string username)
        {
            RecommendationDto recommendations = new RecommendationDto();
            int userId = (await _userService.GetByUserName(username)).Id;
            var allUserReviews = await _reviewService.GetReviewsByUserIdEagerAsync(userId);
            var goodUserBooks = allUserReviews.Where(r => r.Rating >= 8).OrderByDescending(r => r.Rating).Take(5).Select(r => r.Book);
            var unratedBooks = _bookService.GetAllBooksQueryableAsync().Where(b => b.Reviews.Any(i => i.UserId != userId)).ToList();


            //List<Book> unratedBooks = new List<Book>();
            //var goodUserReviews = allUserReviews.Where(r => r.Rating >= 8).OrderByDescending(r => r.Rating).Take(5);
            //var emptyUserReviews = allUserReviews.Where(r => r.Rating == 0);

            recommendations.RecommendedByCosineDistance = await GetRecommendationsByCosineDistanceAsync(goodUserBooks, unratedBooks);

            return recommendations;
        }

        private async Task<List<ResponseBookDto>> GetRecommendationsByCosineDistanceAsync(IEnumerable<Book> goodUserReviews, IEnumerable<Book> emptyUserReviews)
        {
            Dictionary<string, double> bookDistances = new Dictionary<string, double>();

            Cosine cosine = new Cosine();

            int userCount = (await _userService.GetAll()).Count();

            foreach (var ratedBook in goodUserReviews)
            {
                foreach (var unratedBook in emptyUserReviews)
                {
                    //double[] ratedBookVector = (await _reviewService.GetReviewsByBookIdAsync(ratedBook.ISBN)).Select(r => (double)r.Rating).ToArray();
                    //double[] unratedBookVector = (await _reviewService.GetReviewsByBookIdAsync(unratedBook.ISBN)).Select(r => (double)r.Rating).ToArray();

                    //double[] item1 = {user1rating, user2rating, user3rating}; (order is important)
                    //double[] item2 = {user1rating, user2rating, user3rating}; (order is important)

                    //var ratedBookReviews = await _reviewService.GetReviewsByBookIdEagerAsync(ratedBook.ISBN);
                    //var unratedBookReviews = await _reviewService.GetReviewsByBookIdEagerAsync(unratedBook.ISBN);
                    
                    
                    var ratedBookReviews = ratedBook.Reviews;
                    var unratedBookReviews = unratedBook.Reviews;

                    double[] ratedBookVector = new double[userCount];
                    double[] unratedBookVector = new double[userCount];

                    for (int i = 0; i < userCount; i++)
                    {
                        var ratedReview = ratedBookReviews.FirstOrDefault(r => r.User.Id == i);
                        var unratedReview = unratedBookReviews.FirstOrDefault(r => r.User.Id == i);

                        if (ratedReview != null)
                        {
                            ratedBookVector[i] = ratedReview.Rating;
                        }
                        else
                        {
                            ratedBookVector[i] = 0;
                        }

                        if (unratedReview != null)
                        {
                            unratedBookVector[i] = unratedReview.Rating;
                        }
                        else
                        {
                            unratedBookVector[i] = 0;
                        }
                    }

                    _logger.LogInformation(unratedBook.ISBN + "\n" + string.Join(' ',ratedBookVector));

                    double distance = Double.Round(cosine.Distance(ratedBookVector, unratedBookVector), 9);
                    if (bookDistances.Count < 5)
                    {
                        bookDistances.Add(unratedBook.ISBN, distance);
                    }
                    else
                    {
                        KeyValuePair<string, double> worstRecommendationSoFar = bookDistances.OrderBy(b => b.Value).Last();
                        if (distance < worstRecommendationSoFar.Value)
                        {
                            bookDistances.Remove(worstRecommendationSoFar.Key);
                            bookDistances.Add(unratedBook.ISBN, distance);
                        }
                    }
                }
            }

            List<ResponseBookDto> recommendedBooks = new List<ResponseBookDto>();
            foreach (var bookDistance in bookDistances)
            {
                recommendedBooks.Add(await _bookService.GetBookByIdAsync(bookDistance.Key));
            }

            return recommendedBooks;
        }

    }
}
