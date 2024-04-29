using WebApi.ServiceLayer.DTOs;

namespace WebApi.ServiceLayer.Interfaces
{
    public interface IRecommendationsService
    {
        RecommendationDto GetRecommendations(string username);
    }
}
