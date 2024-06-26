using WebApi.DataAccessLayer.Models;

namespace WebApi.ServiceLayer.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User?>> GetAll();
    Task<User?> GetById(int id);
    Task<User?> GetByUserName(string username);
    Task Update(int id,string username,string password, string email, string role);
    Task Delete(int id);
    Task SignUp(string username,string password, string email, string role);
    Task<User?> LogIn(string username, string password);
}