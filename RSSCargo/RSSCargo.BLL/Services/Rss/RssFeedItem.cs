using System.ServiceModel.Syndication;

namespace RSSCargo.BLL.Services.Rss;

public class RssFeedItem
{
    public RssFeedItem(SyndicationItem item, string feedTitle)
    {
        Title = SyndicationGetter.GetValueOrEmpty(item.Title);
        PublishDate = SyndicationGetter.GetValueOrEmpty(item.PublishDate);
        Links = SyndicationGetter.GetValueOrEmpty(item.Links);
        Summary = SyndicationGetter.GetValueOrEmpty(item.Summary);
        Authors = SyndicationGetter.GetValueOrEmpty(item.Authors);
        FeedTitle = feedTitle;
    }

    public string Title { get; }

    public string FeedTitle { get; }

    public string PublishDate { get; }
    
    public IEnumerable<string> Authors { get; }

    public Tuple<string, string>[] Links { get; } // Title & Link

    public string Summary { get; }
}