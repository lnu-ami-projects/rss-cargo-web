using RSSCargo.BLL.Services.Rss;

namespace RSSCargo.PL.Models;

public class FeedsViewModel
{
    public IEnumerable<RssFeed> UserFeeds { get; init; } = new List<RssFeed>();
}