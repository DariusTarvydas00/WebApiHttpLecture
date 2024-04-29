using WebApi.Controllers;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer;
using System.Security.Cryptography;
using System.Text;
using WebApi.DataAccessLayer.Repositories.Interfaces;

namespace WebApi.ServiceLayer;

public class UserService : IUserService
{
    private readonly MainDbContext _userContext;

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public UserService(MainDbContext userContext)
    {
        _userContext = userContext;
    }

    public UserModel GetById(int id)
    {
        return _userContext.Users.Find(id);
    }

    public IEnumerable<UserModel> GetAll()
    {
        return _userContext.Users.ToList();
    }

    public UserModel Create(RegisterRequestDto model)
    {
        if (_userContext.Users.Any(u => u.Username == model.UserEmail))
            throw new ArgumentException("User already exists");

        var user = CreateUser(model.UserEmail, model.Password, model.Roles);
        _userContext.Add(user);
        _userContext.SaveChanges();
        return user;
    }

    public void Update(int id, RegisterRequestDto model)
    {
        var user = _userContext.Users.Find(id);

        if (user == null)
            throw new ArgumentException("User not found");

        user.Username = model.UserEmail;
        user.PasswordHash = null; 
        user.PasswordSalt = null;
        user.Role = model.Roles != null && model.Roles.Any() ? string.Join(",", model.Roles) : "User";

        _userContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = _userContext.Users.Find(id);

        if (user == null)
            throw new ArgumentException("User not found");

        _userContext.Users.Remove(user);
        _userContext.SaveChanges();
    }

    private UserModel CreateUser(string username, string password, string[] roles)
    {
        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        UserModel user = new UserModel
        {
            Username = username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = roles != null && roles.Any() ? string.Join(",", roles) : "User"
        };
        return user;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}