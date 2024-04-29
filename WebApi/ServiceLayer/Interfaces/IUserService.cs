using WebApi.Controllers;
using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer;

public interface IUserService
{
    UserModel GetById(int id);
    IEnumerable<UserModel> GetAll();
    UserModel GetByUsername(string username);
    IEnumerable<ReviewModel> GetUserReviews(int userId);
    UserModel Create(RegisterRequestDto model);
    void Update(int id, RegisterRequestDto model);
    void Delete(int id);
}