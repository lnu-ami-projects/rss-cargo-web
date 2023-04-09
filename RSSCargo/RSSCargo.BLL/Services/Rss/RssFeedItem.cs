using System.ServiceModel.Syndication;

namespace RSSCargo.BLL.Services.Rss;

public class RssFeedItem
{
    public RssFeedItem(SyndicationItem item)
    {
        Title = SyndicationGetter.GetValueOrEmpty(item.Title);
        PublishDate = SyndicationGetter.GetValueOrEmpty(item.PublishDate);
        Links = SyndicationGetter.GetValueOrEmpty(item.Links);
        Summary = SyndicationGetter.GetValueOrEmpty(item.Summary);
    }

    public string Title { get; }

    public string PublishDate { get; }

    public Tuple<string, string>[] Links { get; } // Title & Link

    public string Summary { get; }
}