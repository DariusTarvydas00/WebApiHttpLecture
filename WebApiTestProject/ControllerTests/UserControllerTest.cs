using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.Interfaces;
using WebApi.ServiceLayer.JwtLayer;

namespace WebApiTestProject.ControllerTests;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IJwtService> _mockJwtService;
    private readonly Fixture _fixture;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockJwtService = new Mock<IJwtService>();
        _controller = new UserController(_mockUserService.Object, _mockJwtService.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAll_ReturnsOkObjectResult()
    {
        // Arrange
        var users = _fixture.CreateMany<User>();
        _mockUserService.Setup(service => service.GetAll()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
        Assert.Equal(users, model);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkObjectResult()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockUserService.Setup(service => service.GetById(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetById(user.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<User>(okResult.Value);
        Assert.Equal(user, model);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        int invalidId = -1;
        _mockUserService.Setup(service => service.GetById(invalidId)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.GetById(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    [Fact]
    public async Task Create_WithValidModel_ReturnsCreatedAtActionResult()
    {
        // Arrange
        // var user = _fixture.Create<UserRegisterDto>();
        // _mockUserService.Setup(service => service.SignUp(It.IsAny<User>().Returns(Task.CompletedTask));
        //
        // // Act
        // var result = await _controller.SignUp(user);
        //
        // // Assert
        // var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        // Assert.Equal(nameof(UserController.GetById), createdAtActionResult.ActionName);
        // Assert.Equal(user.Username, createdAtActionResult.RouteValues["UserName"]);
        // Assert.Equal(user, createdAtActionResult.Value);
    }

    [Fact]
    public async Task Update_WithValidModel_ReturnsNoContentResult()
    {
        // Arrange
        // var user = _fixture.Create<User>();
        // _mockUserService.Setup(service => service.Update(It.IsAny<User>())).Returns(Task.CompletedTask);
        //
        // // Act
        // var result = await _controller.Update(user);
        //
        // // Assert
        // Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_WithInvalidModel_ReturnsBadRequestResult()
    {
        // // Arrange
        // var user = _fixture.Create<User>();
        // _mockUserService.Setup(service => service.Update(It.IsAny<User>())).Throws(new Exception("Error"));
        //
        // // Act
        // var result = await _controller.Update(user);
        //
        // // Assert
        // var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        // Assert.Equal("Error", badRequestResult.Value);
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsNoContentResult()
    {
        // Arrange
        int userId = 1;
        _mockUserService.Setup(service => service.Delete(userId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(userId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_WithInvalidId_ReturnsBadRequestResult()
    {
        // Arrange
        int invalidId = -1;
        _mockUserService.Setup(service => service.Delete(invalidId)).Throws(new Exception("Error"));

        // Act
        var result = await _controller.Delete(invalidId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error", badRequestResult.Value);
    }

    [Fact]
    public async Task SignUp_WithValidModel_ReturnsOkObjectResult()
    {
        // Arrange
        var userRegisterDto = _fixture.Create<UserRegisterDto>();
        _mockUserService.Setup(service => service.SignUp(userRegisterDto.Username, userRegisterDto.Password, userRegisterDto.Email, userRegisterDto.Role)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SignUp(userRegisterDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User created successfully", okResult.Value);
    }

    [Fact]
    public async Task SignUp_WithInvalidModel_ReturnsBadRequestResult()
    {
        // Arrange
        _controller.ModelState.AddModelError("Username", "Required");
        var userRegisterDto = _fixture.Create<UserRegisterDto>();

        // Act
        var result = await _controller.SignUp(userRegisterDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username and password are required fields.", badRequestResult.Value);
    }

    [Fact]
    public async Task LogIn_WithValidCredentials_ReturnsOkObjectResult()
    {
        // Arrange
        var userDto = _fixture.Create<UserDto>();
        var user = _fixture.Create<User>();
        _mockUserService.Setup(service => service.LogIn(userDto.Username, userDto.Password)).ReturnsAsync(user);
        _mockJwtService.Setup(service => service.GetJWT(user.Username, user.Role)).Returns("token");

        // Act
        var result = await _controller.LogIn(userDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("token", okResult.Value);
    }

    [Fact]
    public async Task LogIn_WithInvalidCredentials_ReturnsBadRequestResult()
    {
        // Arrange
        var userDto = _fixture.Create<UserDto>();
        _mockUserService.Setup(service => service.LogIn(userDto.Username, userDto.Password)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.LogIn(userDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid username or password.", badRequestResult.Value);
    }

    [Fact]
    public async Task LogIn_ThrowsException_ReturnsBadRequestResult()
    {
        // Arrange
        var userDto = _fixture.Create<UserDto>();
        _mockUserService.Setup(service => service.LogIn(userDto.Username, userDto.Password)).Throws(new Exception("Error"));

        // Act
        var result = await _controller.LogIn(userDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Error", badRequestResult.Value);
    }
}

    
