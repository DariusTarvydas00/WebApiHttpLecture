using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer.DTOs
{
    public class RecommendationDto
    {
        public List<ResponseBookDto> RecommendedByCosineDistance { get; set; } = new List<ResponseBookDto>();

        public List<ResponseBookDto> RecommendedByMatrixFactorization { get; set; } = new List<ResponseBookDto>();

        public List<ResponseBookDto> RecommendedByFieldAwareMatrixF { get; set; } = new List<ResponseBookDto>();
    }
}
