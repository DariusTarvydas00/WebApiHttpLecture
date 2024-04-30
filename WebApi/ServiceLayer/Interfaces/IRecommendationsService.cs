using WebApi.ServiceLayer.DTOs;

namespace WebApi.ServiceLayer.Interfaces
{
    public interface IRecommendationsService
    {
        public RecommendationDto GetRecommendations(string username);
    }
}
