using RSSCargo.BLL.Services;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories;
using RSSCargo.DAL.Repositories.Contracts;
using Moq;

namespace RSSCargo.Tests;

public class UserCargoServiceTests
{
    private readonly UserCargoService _userCargoService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserCargoServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userCargoService = new UserCargoService(_userRepositoryMock.Object);
    }

    [Fact]
    public void GetUserCargos_ReturnsCargosOfUser()
    {
        var userId = 1;
        var rssCargoFirst = 111;
        var rssCargoSecond = 222;
        var cargoFirst = new UserCargo
        {
            UserId = userId,
            CargoId = rssCargoFirst
        };
        var cargoSecond = new UserCargo
        {
            UserId = userId,
            CargoId = rssCargoSecond
        };
        _userRepositoryMock.Setup(repo => repo.GetUserCargos(userId)).Returns(new List<UserCargo> { cargoFirst, cargoSecond });

        var result = _userCargoService.GetUserCargos(userId);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void SubscribeUserCargo_AddsCargoWithFeedsToUser()
    {
        var userId = 1;
        var cargoId = 111;

        _userCargoService.SubscribeUserCargo(userId, cargoId);

        _userRepositoryMock.Verify(repo => repo.SubscribeUserCargo(userId, cargoId), Times.Once());
    }

    [Fact]
    public void UnsubscribeUserCargo_RemovesCargoWithFeedsFromUser()
    {
        var userId = 1;
        var cargoId = 111;

        _userCargoService.UnsubscribeUserCargo(userId, cargoId);

        _userRepositoryMock.Verify(repo => repo.UnsubscribeUserCargo(userId, cargoId), Times.Once());
    }
}
