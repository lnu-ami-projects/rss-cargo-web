using RSSCargo.BLL.Services.Rss;

namespace RSSCargo.PL.Models;

public class FeedsViewModel
{
    public FeedsViewModel(IEnumerable<RssFeed> userFeeds)
    {
        UserFeeds = userFeeds;
    }

    public RssFeed? UserFeed { get; set; }
    public IEnumerable<RssFeed> UserFeeds { get; set; }
}
