using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer;

public interface IUserService
{
    ICollection<ReviewModel> GetReviews(string username);
}