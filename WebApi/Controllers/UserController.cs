using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserInterface.JwtLayer;
using WebApi.DataAccessLayer;
using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer;

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
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public UserController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public ActionResult<IEnumerable<UserModel>> Get()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<UserModel> Get(int id)
    {
        var user = _userService.GetById(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult<UserModel> Post(RegisterRequestDto model)
    {
        var user = _userService.Create(model);
        return Ok(user);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Put(int id, RegisterRequestDto model)
    {
        try
        {
            _userService.Update(id, model);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        try
        {
            _userService.Delete(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
