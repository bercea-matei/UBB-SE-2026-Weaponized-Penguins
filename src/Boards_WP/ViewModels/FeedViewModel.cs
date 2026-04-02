using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services;

namespace Boards_WP.ViewModels;

public partial class FeedViewModel : ObservableObject
{
    private readonly IPostsService _postsService;
    private readonly UserSession _userSession;

    [ObservableProperty]
    private bool _isHome = true;
    public ObservableCollection<Post> Posts { get; } = new();

    public FeedViewModel(IPostsService postsService, UserSession userSession)
    {
        _postsService = postsService;
        _userSession = userSession;
    }

    public void LoadFeed()
    {
        if (_isHome)
            LoadHome();
        else
            LoadDiscovery();
    }
    public void LoadHome()
    {
        
        Posts.Clear();

        // Fetch data from service
        var userId = _userSession.CurrentUser?.UserID ?? 0;
        var data = _postsService.GetPostsForHomePage(userId);

        // IMPORTANT: Add each post to the ObservableCollection
        foreach (var post in data)
        {
            Posts.Add(post);
        }
    }

    public void LoadDiscovery()
    {

        Posts.Clear();

        // Fetch data from service
        var userId = _userSession.CurrentUser?.UserID ?? 0;
        var data = _postsService.GetPostsForDiscoveryPage(userId);

        // IMPORTANT: Add each post to the ObservableCollection
        foreach (var post in data)
        {
            Posts.Add(post);
        }
    }
}