using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSCargo.BLL.Services.Rss;

public class RssFeed
{
    public RssFeed(int id, string url)
    {
        Id = id;

        using var reader = XmlReader.Create(url);
        var feed = SyndicationFeed.Load(reader);

        Title = SyndicationGetter.GetValueOrEmpty(feed.Title);
        Description = SyndicationGetter.GetValueOrEmpty(feed.Description);
        LastUpdatedTime = SyndicationGetter.GetValueOrEmpty(feed.LastUpdatedTime);
        Authors = SyndicationGetter.GetValueOrEmpty(feed.Authors);

        Items = feed.Items.Select(i => new RssFeedItem(i)).ToArray();
    }

    public int Id { get; }
    public string Title { get; }

    public string Description { get; }

    public string LastUpdatedTime { get; }

    public string[] Authors { get; }

    public RssFeedItem[] Items { get; }
}