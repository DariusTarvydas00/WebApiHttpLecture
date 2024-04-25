using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserInterface.JwtLayer;
using WebApi.DataAccessLayer;
using WebApi.DataAccessLayer.Models;

namespace WebApi.Controllers;

public class LoginResponseDto
{
    public string Token { get; set; }
}

public class RegisterRequestDto
{
    public string UserEmail { get; set; }
    public string Password { get; set; }
    public string[] Roles { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly MainDbContext _userContext;
    private readonly IJwtService _jwtService;

    public UserController(MainDbContext userContext, IJwtService jwtService)
    {
        _userContext = userContext;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("SignUp")]
    public ActionResult<UserModel> SignUp(RegisterRequestDto model)
    {
        if (_userContext.Users.Any(u => u.Username == model.UserEmail))
            return Conflict("User already exists");

        var user = CreateUser(model.UserEmail, model.Password, model.Roles);
        _userContext.Add(user);
        _userContext.SaveChanges();
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("LogIn")]
    public ActionResult<string> Login(LoginModel model)
    {
        var user = _userContext.Users.FirstOrDefault(x => x.Username == model.Username);

        if (user == null || !VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Unauthorized("Invalid email or password");
        }

        var token = _jwtService.GetJWT(user.Username, user.Role);
        return Ok(new LoginResponseDto { Token = token });
    }

    [HttpGet("GetUser")]
    [Authorize(Roles = "Admin")]
    public ActionResult<UserModel> GetUser(string username)
    {
        var user = _userContext.Users.FirstOrDefault(x => x.Username == username);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
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
