using RSSCargo.BLL.Services.Rss;

namespace RSSCargo.BLL.Services.Contracts;

public interface IRssFeedService
{
    public IEnumerable<RssFeed> GetUserFeeds(int userId);
    public RssFeed GetUserFeed(int userId, int feedId);

    public bool ValidateFeed(string feedUrl);
}