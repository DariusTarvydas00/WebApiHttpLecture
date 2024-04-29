using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer.DTOs
{
    public class RecommendationDto
    {
        public List<BookModel> RecommendedByCosineDistance { get; set; } = new List<BookModel>();

        public List<BookModel> RecommendedByMatrixFactorization { get; set; } = new List<BookModel>();

        public List<BookModel> RecommendedByFieldAwareMatrixF { get; set; } = new List<BookModel>();
    }
}
