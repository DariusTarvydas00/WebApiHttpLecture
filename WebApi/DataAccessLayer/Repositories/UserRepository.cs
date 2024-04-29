using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;

namespace WebApi.DataAccessLayer.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MainDbContext _userContext;

    public UserRepository(MainDbContext userContext)
    {
        _userContext = userContext;
    }

    public UserModel GetById(int id)
    {
        return _userContext.Users.Find(id);
    }

    public UserModel GetByUsername(string username)
    {
        return _userContext.Users.FirstOrDefault(u => u.Username == username);
    }

    public IEnumerable<UserModel> GetAll()
    {
        return _userContext.Users.ToList();
    }

    public UserModel Add(UserModel user)
    {
        _userContext.Users.Add(user);
        _userContext.SaveChanges();
        return user;
    }

    public void Update(UserModel user)
    {
        _userContext.Users.Update(user);
        _userContext.SaveChanges();
    }

    public void Delete(UserModel user)
    {
        _userContext.Users.Remove(user);
        _userContext.SaveChanges();
    }

    public void SaveChanges()
    {
        _userContext.SaveChanges();
    }
}