using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User?>> GetAll();
    Task<User?> GetById(int id);
    Task<User?> GetByUserName(string username);
    Task Create(User model);
    Task Update(User model);
    Task Delete(int id);
    Task<IEnumerable<Review>> GetReviews(string username);
    Task SignUp(string username,string password);
    Task<User?> LogIn(string username, string password);
}