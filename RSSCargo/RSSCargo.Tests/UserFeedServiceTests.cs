using RSSCargo.BLL.Services;
using RSSCargo.DAL.Models;
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

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 0)]
    public void GetUserFeeds_ReturnsFeedsOfUser(int expectedUserId, int expectedCount)
    {
        var userFeeds = GetFeedsOfUser();
        var userId = userFeeds[0].UserId;
        _userRepositoryMock.Setup(repo => repo.GetUserFeeds(userId)).Returns(userFeeds);

        var result = _userFeedService.GetUserFeeds(expectedUserId);

        Assert.Equal(expectedCount, result.Count());
    }

    [Fact]
    public void AddUserFeed_AddsFeedToUser()
    {
        const int userId = 1;
        const string rssFeed = "feed1";

        _userFeedService.AddUserFeed(userId, rssFeed);

        _userRepositoryMock.Verify(repo => repo.AddUserFeed(userId, rssFeed), Times.Once());
    }

    [Fact]
    public void RemoveUserFeed_RemovesFeedFromUser()
    {
        var userFeeds = GetFeedsOfUser();
        var userId = userFeeds[0].UserId;
        var feedRss = userFeeds[0].RssFeed;
        var feedToBeRemoved = userFeeds[0];

        _userRepositoryMock.Setup(repo => repo.GetUserFeeds(userId)).Returns(userFeeds);
        _userFeedService.RemoveUserFeed(userId, feedRss);

        _userRepositoryMock.Verify(repo => repo.RemoveUserFeed(feedToBeRemoved), Times.Once());
    }

    [Fact]
    public void RemoveUserFeed_ExceptionWhileRemovingFeedFromUser()
    {
        var userFeeds = GetFeedsOfUser();
        var userId = userFeeds[0].UserId;
        const int incorrectUserId = 123;
        var feedRss = userFeeds[0].RssFeed;

        _userRepositoryMock.Setup(repo => repo.GetUserFeeds(userId)).Returns(userFeeds);

        Assert.Throws<InvalidOperationException>(() => _userFeedService.RemoveUserFeed(incorrectUserId, feedRss));
    }

    private static List<UserFeed> GetFeedsOfUser()
    {
        const int userId = 1;
        const string rssFeedFirst = "feed1";
        const string rssFeedSecond = "feed2";
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

        return new List<UserFeed> { feedFirst, feedSecond };
    }
}
