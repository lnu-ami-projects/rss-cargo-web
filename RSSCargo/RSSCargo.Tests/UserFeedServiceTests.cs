using RSSCargo.BLL.Services;
using RSSCargo.DAL.Models;
using RSSCargo.DAL.Repositories;
using RSSCargo.DAL.Repositories.Contracts;
using Moq;

namespace RSSCargo.Tests;

public class UserFeedServiceTests
{
    private readonly UserFeedService _userFeedService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserFeedServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userFeedService = new UserFeedService(_userRepositoryMock.Object);
    }

    [Fact]
    public void GetUserFeeds_ReturnsFeedsOfUser()
    {
        var userId = 1;
        var rssFeedFirst = "feed1";
        var rssFeedSecond = "feed2";
        var feedFirst = new UserFeed
        {
            UserId = userId,
            RssFeed = rssFeedFirst
        };
        var feedSecond = new UserFeed
        {
            UserId = userId,
            RssFeed = rssFeedSecond
        };
        _userRepositoryMock.Setup(repo => repo.GetUserFeeds(userId)).Returns(new List<UserFeed> { feedFirst, feedSecond });

        var result = _userFeedService.GetUserFeeds(userId);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void AddUserFeed_AddsFeedToUser()
    {
        var userId = 1;
        var rssFeed = "feed1";

        _userFeedService.AddUserFeed(userId, rssFeed);

        _userRepositoryMock.Verify(repo => repo.AddUserFeed(userId, rssFeed), Times.Once());
    }

    [Fact]
    public void RemoveUserFeed_RemovesFeedFromUser()
    {
        var userId = 1;
        var rssFeed = "feed1";
        

        _userFeedService.RemoveUserFeed(userId, rssFeed);

        _userRepositoryMock.Verify(repo => repo.RemoveUserFeed(repo.GetUserFeeds(userId).First(uf=> uf.RssFeed == rssFeed)), Times.Once());
    }
}
