using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Rss;

namespace RSSCargo.BLL.Services;

public class RssFeedService : IRssFeedService
{
    private readonly IUserFeedService _userFeedService;

    public RssFeedService(IUserFeedService userFeedService)
    {
        _userFeedService = userFeedService;
    }

    public IEnumerable<RssFeed> GetUserFeeds(int userId)
    {
        var userFeeds = _userFeedService.GetUserFeeds(userId);

        return userFeeds.Select(userFeed => new RssFeed(userFeed.Id, userFeed.RssFeed));
    }

    public RssFeed GetUserFeed(int userId, int feedId)
    {
        var userFeeds = _userFeedService.GetUserFeeds(userId);
        var feed = userFeeds.First(userFeed => userFeed.Id == feedId);

        return new RssFeed(feed.Id, feed.RssFeed);
    }

    public bool ValidateFeed(string feedUrl)
    {
        try
        {
            _ = new RssFeed(0, feedUrl);
        }
        catch
        {
            return false;
        }

        return true;
    }
}