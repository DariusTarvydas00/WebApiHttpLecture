using AutoFixture;
using Moq;
using System.Security.Cryptography;
using System.Text;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.ServiceLayer.DTOs;
using WebApi.ServiceLayer.JwtLayer;
using WebApi.ServiceLayer;

namespace WebApiTestProject.ServiceLayerTests;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IJwtService> _mockJwtService;
    private readonly Fixture _fixture;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtService = new Mock<IJwtService>();
        _userService = new UserService(_mockUserRepository.Object, _mockJwtService.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAll_ReturnsListOfUsers()
    {
        // Arrange
        var users = _fixture.CreateMany<User>().ToList();
        _mockUserRepository.Setup(repo => repo.GetAll()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAll();

        // Assert
        Assert.Equal(users, result);
    }

    [Fact]
    public async Task GetById_ReturnsUserWithGivenId()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockUserRepository.Setup(repo => repo.GetById(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetById(user.Id);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetByUserName_ReturnsUserWithGivenUserName()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockUserRepository.Setup(repo => repo.GetByUserName(user.Username)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetByUserName(user.Username);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task Create_CreatesNewUser()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        await _userService.Create(user);

        // Assert
        _mockUserRepository.Verify(repo => repo.Create(user), Times.Once);
    }

    [Fact]
    public async Task Update_UpdatesExistingUser()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        await _userService.Update(user);

        // Assert
        _mockUserRepository.Verify(repo => repo.Update(user), Times.Once);
    }

    [Fact]
    public async Task Delete_RemovesUserWithGivenId()
    {
        // Arrange
        int userId = 1;

        // Act
        await _userService.Delete(userId);

        // Assert
        _mockUserRepository.Verify(repo => repo.Delete(userId), Times.Once);
    }

    [Fact]
    public async Task SignUp_CreatesNewUser()
    {
        // Arrange
        var userRegisterDto = _fixture.Create<UserRegisterDto>();

        // Act
        await _userService.SignUp(userRegisterDto.Username, userRegisterDto.Password, userRegisterDto.Email, userRegisterDto.Role);

        // Assert
        _mockUserRepository.Verify(repo => repo.Create(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SignUp_ThrowsException_WhenUsernameAlreadyExists()
    {
        // Arrange
        var userRegisterDto = _fixture.Create<UserRegisterDto>();
        _mockUserRepository.Setup(repo => repo.GetByUserName(userRegisterDto.Username)).ReturnsAsync(_fixture.Create<User>());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.SignUp(userRegisterDto.Username, userRegisterDto.Password, userRegisterDto.Email, userRegisterDto.Role));
    }

    [Fact]
    public async Task LogIn_ReturnsUserWithValidCredentials()
    {
        // Arrange
        var userDto = _fixture.Create<UserDto>();
        var user = _fixture.Create<User>();
        var password = _fixture.Create<string>();
        var passwordSalt = Encoding.UTF8.GetBytes(password);
        var passwordHash = new HMACSHA512(passwordSalt).ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
        user.PasswordSalt = passwordSalt;
        user.PasswordHash = passwordHash;

        _mockUserRepository.Setup(repo => repo.GetByUserName(userDto.Username)).ReturnsAsync(user);

        // Act
        var result = await _userService.LogIn(userDto.Username, userDto.Password);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task LogIn_ReturnsNullWithInvalidCredentials()
    {
        // Arrange
        var userDto = _fixture.Create<UserDto>();
        var user = _fixture.Create<User>();
        var invalidPassword = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        var passwordSalt = Encoding.UTF8.GetBytes(password);
        var passwordHash = new HMACSHA512(passwordSalt).ComputeHash(Encoding.UTF8.GetBytes(password));
        user.PasswordSalt = passwordSalt;
        user.PasswordHash = passwordHash;

        _mockUserRepository.Setup(repo => repo.GetByUserName(userDto.Username)).ReturnsAsync(user);

        // Act
        var result = await _userService.LogIn(userDto.Username, invalidPassword);

        // Assert
        Assert.Null(result);
    }
}