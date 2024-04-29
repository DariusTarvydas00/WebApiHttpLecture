using WebApi.DataAccessLayer.Models;

namespace WebApi.DataAccessLayer.Repositories.Interfaces;

public interface IUserRepository
{
    UserModel GetById(int id);
    UserModel GetByUsername(string username);
    IEnumerable<UserModel> GetAll();
    UserModel Add(UserModel user);
    void Update(UserModel user);
    void Delete(UserModel user);
    void SaveChanges();
}