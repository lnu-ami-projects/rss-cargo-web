using RSSCargo.BLL.Services;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Rss;
using Moq;

namespace RSSCargo.Tests;

public class CargoServiceTests
{
    private readonly CargoService _cargoService;
    private readonly Mock<ICargoRepository> _cargoRepositoryMock;
    private readonly Mock<IUserCargoService> _userCargoServiceMock;

    public CargoServiceTests()
    {
        _cargoRepositoryMock = new Mock<ICargoRepository>();
        _userCargoServiceMock = new Mock<IUserCargoService>();
        _cargoService = new CargoService(_cargoRepositoryMock.Object, _userCargoServiceMock.Object);
    }

    [Fact]
    public void GetAllCargos_ReturnsAllCargos()
    {
        var cargos = GetCargos();

        _cargoRepositoryMock.Setup(repo => repo.GetAllCargos()).Returns(cargos);
        var result = _cargoService.GetAllCargos();

        Assert.Equal(cargos, result);
    }

    [Fact]
    public void GetUnsubscribedCargos_WhenThereAreSome_ReturnsCargosWhichAreNotUsedForAnyUser()
    {
        var cargos = GetCargos();
        var userCargos = GetCargosOfUser();

        _userCargoServiceMock.Setup(service => service.GetUserCargos(userCargos[0].UserId)).Returns(userCargos);
        _cargoRepositoryMock.Setup(repo => repo.GetAllCargos()).Returns(cargos);
        var result = _cargoService.GetUnsubscribedCargos(userCargos[0].UserId).ToArray();

        Assert.Equal(cargos[2], result[0]);
    }

    [Fact]
    public void GetUnsubscribedCargos_WhenThereAreNotAny_ReturnsCargosWhichAreNotUsedForAnyUser()
    {
        var cargos = GetCargos().SkipLast(1).ToList();
        var userCargos = GetCargosOfUser();

        _userCargoServiceMock.Setup(service => service.GetUserCargos(userCargos[0].UserId)).Returns(userCargos);
        _cargoRepositoryMock.Setup(repo => repo.GetAllCargos()).Returns(cargos);
        var result = _cargoService.GetUnsubscribedCargos(userCargos[0].UserId).ToArray();

        Assert.Equal(Array.Empty<Cargo>(), result);
    }

    [Fact]
    public void GetSubscribedCargos_WhenThereAreSome_ReturnsCargosWhichAreUsedForAtLeastOneUser()
    {
        var cargos = GetCargos();
        var userCargos = GetCargosOfUser();

        _userCargoServiceMock.Setup(service => service.GetUserCargos(userCargos[0].UserId)).Returns(userCargos);
        _cargoRepositoryMock.Setup(repo => repo.GetAllCargos()).Returns(cargos);
        var result = _cargoService.GetSubscribedCargos(userCargos[0].UserId).ToArray();

        Assert.Equal(cargos.SkipLast(1).ToArray(), result);
    }

    [Fact]
    public void GetSubscribedCargos_WhenThereAreNotAny_ReturnsCargosWhichAreUsedForAtLeastOneUser()
    {
        var cargos = GetCargos().Skip(2).ToList();
        var userCargos = GetCargosOfUser();

        _userCargoServiceMock.Setup(service => service.GetUserCargos(userCargos[0].UserId)).Returns(userCargos);
        _cargoRepositoryMock.Setup(repo => repo.GetAllCargos()).Returns(cargos);
        var result = _cargoService.GetSubscribedCargos(userCargos[0].UserId).ToArray();

        Assert.Equal(new Cargo[] { }, result);
    }

    [Theory]
    [InlineData(111)]
    [InlineData(222)]
    [InlineData(123)]
    public void GetCargoFeeds_ReturnsCargoFeedsFromParticularCargo(int expectedCargoId)
    {
        var cargoFeeds = GetCargoFeedsFromCargos().Where(x => x.CargoId == expectedCargoId).ToList();

        _cargoRepositoryMock.Setup(repo => repo.GetCargoFeeds(expectedCargoId)).Returns(cargoFeeds);
        var result = _cargoService.GetCargoFeeds(expectedCargoId);

        Assert.Equal(cargoFeeds, result);
    }

    [Fact]
    public void GetCargoById_ReturnsCargoById()
    {
        var cargos = GetCargos();
        var cargoId = 111;

        _cargoRepositoryMock.Setup(repo => repo.GetAllCargos()).Returns(cargos);
        var result = _cargoService.GetCargoById(cargoId);

        Assert.Equal(cargos.First(c => c.Id == cargoId), result);
    }

    [Fact]
    public void GetCargoById_IfNoCargoByIdIsFound_ExceptionIsThrown()
    {
        var cargos = GetCargos();
        var cargoId = 123;

        _cargoRepositoryMock.Setup(repo => repo.GetAllCargos()).Returns(cargos);

        Assert.Throws<InvalidOperationException>(() => _cargoService.GetCargoById(cargoId));
    }

    [Theory]
    [InlineData(111)]
    [InlineData(222)]
    [InlineData(123)]
    public void GetRssCargoFeeds_ReturnsRssFeedsFromCargoById(int expectedCargoId)
    {
        var cargoFeeds = GetCargoFeedsFromCargos().Where(x => x.CargoId == expectedCargoId);

        var enumerable = cargoFeeds as CargoFeed[] ?? cargoFeeds.ToArray();
        _cargoRepositoryMock.Setup(repo => repo.GetCargoFeeds(expectedCargoId)).Returns(enumerable);
        var result = _cargoService.GetRssCargoFeeds(expectedCargoId).Select(x => new {x.Link, x.Description, x.Title, x.Authors, x.Id, x.LastUpdatedTime}).ToList();
        var expectedResult = enumerable.Select(cF => new RssFeed(0, cF.RssFeed)).Select(x => new { x.Link, x.Description, x.Title, x.Authors, x.Id, x.LastUpdatedTime}).ToList(); 

        Assert.Equal(expectedResult, result);
    }


    private static List<Cargo> GetCargos()
    {
        var cargoFirst = new Cargo
        {
            Id = 111,
            Name = "rssCargoFirst"
        };
        var cargoSecond = new Cargo
        {
            Id = 222,
            Name = "rssCargoSecond"
        };
        var cargoThird = new Cargo
        {
            Id = 333,
            Name = "rssCargoThird"
        };

        return new List<Cargo> { cargoFirst, cargoSecond, cargoThird };
    }

    private static IEnumerable<CargoFeed> GetCargoFeedsFromCargos()
    {
        var cargoFeedFirst = new CargoFeed
        {
            CargoId = 111,
            RssFeed = "http://rss.cnn.com/rss/edition_world.rss"
        };
        var cargoFeedSecond = new CargoFeed
        {
            CargoId = 111,
            RssFeed = "http://rss.cnn.com/rss/edition_business.rss"
        };
        var cargoFeedThird = new CargoFeed
        {
            CargoId = 222,
            RssFeed = "http://rss.cnn.com/rss/edition_business.rss"
        };

        return new List<CargoFeed> { cargoFeedFirst, cargoFeedSecond, cargoFeedThird };
    }

    private static List<UserCargo> GetCargosOfUser()
    {
        const int userId = 1;
        const int rssCargoFirst = 111;
        const int rssCargoSecond = 222;
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

