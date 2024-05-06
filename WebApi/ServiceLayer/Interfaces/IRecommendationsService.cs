using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;

namespace WebApi.ServiceLayer.Interfaces
{
    public interface IRecommendationsService
    {
        public Task<List<Book>> GetRecommendations(string username);
    }
}
