using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer.DTOs
{
    public class RecommendationDto
    {
        public List<BookDto> RecommendedByCosineDistance { get; set; } = new List<BookDto>();

        public List<BookDto> RecommendedByMatrixFactorization { get; set; } = new List<BookDto>();

        public List<BookDto> RecommendedByFieldAwareMatrixF { get; set; } = new List<BookDto>();
    }
}
