using RSSCargo.BLL.Services.Rss;

namespace RSSCargo.PL.Models;

public class CargoFeedsViewModel
{
    public string CargoName { get; init; }
    public IEnumerable<RssFeed>? UserCargoFeeds { get; init; }
}