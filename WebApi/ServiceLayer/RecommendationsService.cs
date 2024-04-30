using Accord.Math.Distances;
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

        public RecommendationsService(IUserService userService, IBookService bookService, IReviewService reviewService)
        {
            _userService = userService;
            _bookService = bookService;
            _reviewService = reviewService;
        }

        public async Task<RecommendationDto> GetRecommendations(string username)
        {
            RecommendationDto recommendations = new RecommendationDto();
            int userId = (await _userService.GetByUserName(username)).Id;
            var allUserReviews = await _reviewService.GetReviewsByUser(userId);
            var goodUserReviews = allUserReviews.Where(r => r.Rating >= 8).OrderByDescending(r => r.Rating).Take(5);
            var emptyUserReviews = allUserReviews.Where(r => r.Rating == 0);

            recommendations.RecommendedByCosineDistance = await GetRecommendationsByCosineDistanceAsync(goodUserReviews, emptyUserReviews);

            return recommendations;
        }

        private async Task<List<BookDto>> GetRecommendationsByCosineDistanceAsync(IEnumerable<Review> goodUserReviews, IEnumerable<Review> emptyUserReviews)
        {
            Dictionary<int, double> bookDistances = new Dictionary<int, double>();

            Cosine cosine = new Cosine();

            foreach (var ratedBook in goodUserReviews)
            {
                foreach (var unratedBook in emptyUserReviews)
                {
                    double[] ratedBookVector = (await _bookService.GetReviewsByBookIdAsync(ratedBook.BookId)).Select(r => (double)r.Rating).ToArray();
                    double[] unratedBookVector = (await _bookService.GetReviewsByBookIdAsync(unratedBook.BookId)).Select(r => (double)r.Rating).ToArray();
                    double distance = Double.Round(cosine.Distance(ratedBookVector, unratedBookVector), 9);
                    if (bookDistances.Count < 5)
                    {
                        bookDistances.Add(unratedBook.BookId, distance);
                    }
                    else
                    {
                        KeyValuePair<int, double> worstRecommendationSoFar = bookDistances.OrderBy(b => b.Value).Last();
                        if (distance < worstRecommendationSoFar.Value)
                        {
                            bookDistances.Remove(worstRecommendationSoFar.Key);
                            bookDistances.Add(unratedBook.BookId, distance);
                        }
                    }
                }
            }

            List<BookDto> recommendedBooks = new List<BookDto>();
            foreach (var bookDistance in bookDistances)
            {
                recommendedBooks.Add(await _bookService.GetBookByIdAsync(bookDistance.Key));
            }

            return recommendedBooks;
        }

    }
}
