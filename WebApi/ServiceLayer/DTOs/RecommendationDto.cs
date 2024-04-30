using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer.DTOs
{
    public class RecommendationDto
    {
        public List<Book> RecommendedByCosineDistance { get; set; } = new List<Book>();

        public List<Book> RecommendedByMatrixFactorization { get; set; } = new List<Book>();

        public List<Book> RecommendedByFieldAwareMatrixF { get; set; } = new List<Book>();
    }
}
