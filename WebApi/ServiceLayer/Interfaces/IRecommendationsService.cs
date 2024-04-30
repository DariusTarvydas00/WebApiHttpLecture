using WebApi.ServiceLayer.DTOs;

namespace WebApi.ServiceLayer.Interfaces
{
    public interface IRecommendationsService
    {
        public Task<RecommendationDto> GetRecommendations(string username);
    }
}
