namespace RSSCargo.BLL.Services;

using DAL.Models;
using RSSCargo.DAL.Repositories.Contracts;
using Contracts;

public class UserFeedService : IUserFeedService
{
    private readonly IUserRepository _repository;

    public UserFeedService(IUserRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<UserFeed> GetUserFeeds(int userId)
    {
        return _repository.GetUserFeeds(userId);
    }

    public void AddUserFeed(int userId, string rssFeed)
    {
        _repository.AddUserFeed(userId, rssFeed);
    }

    public void RemoveUserFeed(int userId, string rssFeed)
    {
        var found = _repository.GetUserFeeds(userId).First(x => x.RssFeed == rssFeed);
        _repository.RemoveUserFeed(found);
    }
}
