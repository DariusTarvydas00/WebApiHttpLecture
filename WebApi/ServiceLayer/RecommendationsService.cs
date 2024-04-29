using Accord.Math.Distances;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;


namespace WebApi.ServiceLayer
{
    public class RecommendationsService : IRecommendationsService
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        public RecommendationsService(IUserService userService, IBookService bookService)
        {
            _userService = userService;
            _bookService = bookService;
        }

        public RecommendationDto GetRecommendations(string username)
        {
            RecommendationDto recommendations = new RecommendationDto();
            var allUserReviews = _userService.GetReviews(username);
            var goodUserReviews = allUserReviews.Where(r => r.Rating >= 8).OrderByDescending(r => r.Rating).Take(5);
            var emptyUserReviews = allUserReviews.Where(r => r.Rating == 0);
            
            recommendations.RecommendedByCosineDistance = GetRecommendationsByCosineDistance(goodUserReviews, emptyUserReviews);

            return recommendations;
        }

        private List<BookModel> GetRecommendationsByCosineDistance(IEnumerable<ReviewModel> goodUserReviews, IEnumerable<ReviewModel> emptyUserReviews)
        {
            Dictionary<int, double> bookDistances = new Dictionary<int, double>();

            Cosine cosine = new Cosine();

            foreach (var ratedBook in goodUserReviews)
            {
                foreach (var unratedBook in emptyUserReviews)
                {
                    double[] ratedBookVector = _bookService.GetReviewsByBookId(ratedBook.BookId).Select(r => r.Rating).ToArray();
                    double[] unratedBookVector = _bookService.GetReviewsByBookId(unratedBook.BookId).Select(r => r.Rating).ToArray();
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

            List<BookModel> recommendedBooks = new List<BookModel>();
            foreach (var bookDistance in bookDistances)
            {
                recommendedBooks.Add(_bookService.GetBookById(bookDistance.Key));
            }

            return recommendedBooks;
        }

    }
}
