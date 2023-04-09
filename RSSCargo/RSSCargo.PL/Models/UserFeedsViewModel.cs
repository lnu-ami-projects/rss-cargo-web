using RSSCargo.BLL.Services.Rss;

namespace RSSCargo.PL.Models;

public class UserFeedsViewModel
{
    public RssFeed? UserFeed { get; init; }
    public IEnumerable<RssFeedItem>? UserFeedsItems { get; init; }
}
