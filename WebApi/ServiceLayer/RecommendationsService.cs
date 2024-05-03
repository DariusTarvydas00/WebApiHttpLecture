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
            var goodUserBooks = allUserReviews.Where(r => r.Rating >= 8).OrderByDescending(r => r.Rating).Take(3).Select(r => r.Book);
            var usersThatReviewedGoodBooks = _reviewService.GetAllReviews().Where(r => goodUserBooks.Contains(r.Book) && r.Rating >= 8).Select(r => r.UserId).Distinct().ToList();

            var unratedBooks = _bookService.GetAllBooksQueryableAsync().Where(b => b.Reviews.Any(i => i.UserId != userId)).ToList();
            var unratedBooksFiltered = unratedBooks.Where(b => b.Reviews.Any(r => usersThatReviewedGoodBooks.Contains(r.UserId))).ToList();

            recommendations.RecommendedByCosineDistance = await GetRecommendationsByCosineDistanceAsync(goodUserBooks, unratedBooksFiltered);

            return recommendations;
        }

        private async Task<List<ResponseBookDto>> GetRecommendationsByCosineDistanceAsync(IEnumerable<Book> goodUserBooks, IEnumerable<Book> otherBooks)
        {
            Dictionary<string, double> bookDistances = new Dictionary<string, double>();

            Cosine cosine = new Cosine();

            int userCount = (await _userService.GetAll()).Count();
            

            foreach (var ratedBook in goodUserBooks)
            {
                int loggingCounter = 0;
                foreach (var unratedBook in otherBooks)
                {

                    //this is first version of building vectors
                    //double[] ratedBookVector = (await _reviewService.GetReviewsByBookIdAsync(ratedBook.ISBN)).Select(r => (double)r.Rating).ToArray();
                    //double[] unratedBookVector = (await _reviewService.GetReviewsByBookIdAsync(unratedBook.ISBN)).Select(r => (double)r.Rating).ToArray();


                    //this is just some example to show how the vectors should look like
                    //double[] item1 = {user1rating, user2rating, user3rating}; (order is important)
                    //double[] item2 = {user1rating, user2rating, user3rating}; (order is important)


                    //this is second version of building vectors
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

                    //this is third version of building vectors, which builds them by running raw sql
                    //var ratedBookVector = _bookService.GetBookVector(ratedBook.ISBN);
                    //var unratedBookVector = _bookService.GetBookVector(unratedBook.ISBN);

                    loggingCounter = loggingCounter + 1;
                    _logger.LogInformation("rated: " + ratedBook.ISBN + "   unrated: " + unratedBook.ISBN + "   counter: " + loggingCounter);

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
