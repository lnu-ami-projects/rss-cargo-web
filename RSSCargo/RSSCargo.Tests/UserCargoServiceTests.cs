using RSSCargo.BLL.Services;
using RSSCargo.DAL.Models;
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

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 0)]
    public void GetUserCargos_ReturnsCargosOfUser(int expectedUserId, int expectedCount)
    {
        var userCargos = GetCargosOfUser();
        var userId = userCargos[0].UserId;
        _userRepositoryMock.Setup(repo => repo.GetUserCargos(userId)).Returns(GetCargosOfUser());

        var result = _userCargoService.GetUserCargos(expectedUserId);

        Assert.Equal(expectedCount, result.Count());
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
        var userCargos = GetCargosOfUser();
        var userId = userCargos[0].UserId;
        var cargoId = userCargos[0].CargoId;
        var cargoToBeRemoved = userCargos[0];

        _userRepositoryMock.Setup(repo => repo.GetUserCargos(userId)).Returns(userCargos);
        _userCargoService.UnsubscribeUserCargo(userId, cargoId);

        _userRepositoryMock.Verify(repo => repo.UnsubscribeUserCargo(cargoToBeRemoved), Times.Once());
    }

    [Fact]
    public void UnsubscribeUserCargo_ExceptionWhileRemovingCargoWithFeedsFromUser()
    {
        var userCargos = GetCargosOfUser();
        var userId = userCargos[0].UserId;
        var incorrectUserId = 123;
        var cargoId = userCargos[0].CargoId;

        _userRepositoryMock.Setup(repo => repo.GetUserCargos(userId)).Returns(userCargos);

        Assert.Throws<InvalidOperationException>(() => _userCargoService.UnsubscribeUserCargo(incorrectUserId, cargoId));
    }

    private List<UserCargo> GetCargosOfUser()
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

        return new List<UserCargo> { cargoFirst, cargoSecond };
    }
}
