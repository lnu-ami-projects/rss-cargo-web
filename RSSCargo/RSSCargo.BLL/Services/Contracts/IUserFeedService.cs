using Microsoft.AspNetCore.Http;

namespace RSSCargo.BLL.Services.Contracts;

using DAL.Models;

public interface IUserFeedService
{
    public IEnumerable<UserFeed> GetUserFeeds(int userId);
    public void AddUserFeed(int userId, string rssFeed);
    public void RemoveUserFeed(int userId, string rssFeed);
}
