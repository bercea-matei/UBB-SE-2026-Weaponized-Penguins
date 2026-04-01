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
    public ObservableCollection<PostPreviewViewModel> PostsList { get; } = new();
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
        var userId = _userSession.CurrentUser?.UserID ?? 0;
        var data = _postsService.GetPostsForHomePage(userId); 

        PostsList.Clear();
        foreach (var post in data)
        {
            var previewViewModel = new PostPreviewViewModel(post);
            PostsList.Add(previewViewModel);
        }
    }

    public void LoadDiscovery()
    {
        var userId = _userSession.CurrentUser?.UserID ?? 0;
        var data = _postsService.GetPostsForDiscoveryPage(userId);

        PostsList.Clear();
        foreach (var post in data)
        {
            var previewViewModel = new PostPreviewViewModel(post);
            PostsList.Add(previewViewModel);
        }
    }
}