using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories;
using WebApi.DataAccessLayer;

namespace WebApiTestProject.DataAccessLayerTests;

public class UserRepositoryTests
{
    private readonly UserRepository _userRepository;
    private readonly Mock<MainDbContext> _mockDbContext;
    private readonly Fixture _fixture;

    public UserRepositoryTests()
    {
        _mockDbContext = new Mock<MainDbContext>(new DbContextOptions<MainDbContext>());
        _userRepository = new UserRepository(_mockDbContext.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAll_ReturnsListOfUsers()
    {
        // Arrange
        var users = _fixture.CreateMany<User>().ToList();
        var mockDbSet = GetQueryableMockDbSet(users);
        _mockDbContext.Setup(context => context.Users).Returns(mockDbSet);

        // Act
        var result = await _userRepository.GetAll();

        // Assert
        Assert.Equal(users, result);
    }

    [Fact]
    public async Task GetById_ReturnsUserWithGivenId()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var mockDbSet = GetQueryableMockDbSet(new List<User> { user });
        _mockDbContext.Setup(context => context.Users).Returns(mockDbSet);

        // Act
        var result = await _userRepository.GetById(user.Id);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenUserNotFound()
    {
        // Arrange
        var users = new List<User>();
        var mockDbSet = GetQueryableMockDbSet(users);
        _mockDbContext.Setup(context => context.Users).Returns(mockDbSet);

        // Act
        var result = await _userRepository.GetById(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserName_ReturnsUserWithGivenUserName()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var mockDbSet = GetQueryableMockDbSet(new List<User> { user });
        _mockDbContext.Setup(context => context.Users).Returns(mockDbSet);

        // Act
        var result = await _userRepository.GetByUserName(user.Username);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetByUserName_ReturnsNull_WhenUserNotFound()
    {
        // Arrange
        var users = new List<User>();
        var mockDbSet = GetQueryableMockDbSet(users);
        _mockDbContext.Setup(context => context.Users).Returns(mockDbSet);

        // Act
        var result = await _userRepository.GetByUserName("nonexistentuser");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_AddsNewUser()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        await _userRepository.Create(user);

        // Assert
        _mockDbContext.Verify(context => context.Users.Add(user), Times.Once);
        
    }

    [Fact]
    public async Task Update_UpdatesExistingUser()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        await _userRepository.Update(user);

        // Assert
        _mockDbContext.Verify(context => context.Users.Update(user), Times.Once);
        
    }

    [Fact]
    public async Task Delete_RemovesUserWithGivenId()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var mockDbSet = GetQueryableMockDbSet(new List<User> { user });
        _mockDbContext.Setup(context => context.Users).Returns(mockDbSet);

        // Act
        await _userRepository.Delete(user.Id);

        // Assert
        _mockDbContext.Verify(context => context.Users.Remove(user), Times.Once);
    }

    private DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
    {
        var queryable = sourceList.AsQueryable();
        var mockDbSet = new Mock<DbSet<T>>();
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        mockDbSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) => queryable.FirstOrDefault(e => e.GetType().GetProperty("Id").GetValue(e).Equals(ids[0])));
        return mockDbSet.Object;
    }
}