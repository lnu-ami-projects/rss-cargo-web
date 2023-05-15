using RSSCargo.BLL.Services;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace RSSCargo.Tests;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public void GetUserByEmail_ReturnsUser()
    {
        var userEmail = "a@email.com";
        var user = new User { Email = userEmail };
        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(userEmail)).Returns(user);

        var result = _userService.GetUserByEmail(userEmail);

        Assert.Equal(user, result);
    }

    [Fact]
    public void GetUserById_ReturnsUser()
    {
        var userId = 1;
        var user = new User { Id = userId };
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

        var result = _userService.GetUserById(userId);

        Assert.Equal(user, result);
    }

    [Fact]
    public void GetUserAuthenticated_ReturnsUser()
    {
        var userId = 1;
        var user = new User { Id = userId };
        var userClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "Bill"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }, "Cookies"));
        var httpContext = new DefaultHttpContext() { User = userClaimPrincipal };
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

        var result = _userService.GetUserAuthenticated(httpContext);

        Assert.Equal(user, result);
    }

    [Fact]
    public void CreateUser_CreatesUser()
    {
        var userEmail = "b@email.com";
        var userName = "b";
        var userPassword = "1111";

        _userService.CreateUser(userEmail, userName, userPassword);

        _userRepositoryMock.Verify(repo => repo.CreateUser(userEmail, userName, userPassword), Times.Once());
    }
}