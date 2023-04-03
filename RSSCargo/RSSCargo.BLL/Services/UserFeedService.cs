using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

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
        _repository.RemoveUserFeed(userId, rssFeed);
    }
}
