using System;
using System.Collections.Generic;
using System.Text;

using Boards_WP.ViewModels;
using Boards_WP.Views.Pages;
using Boards_WP;

using CommunityToolkit.Mvvm.Input;

public partial class CommunityBarViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ICommunitiesService _communitiesService;
    private readonly FeedViewModel _feedViewModel;
    private readonly UserSession _userSession = App.GetService<UserSession>();

    public ObservableCollection<Community> Communities { get; } = new();

    public CommunityBarViewModel(
        INavigationService navigationService,
        ICommunitiesService communitiesService,
        FeedViewModel feedViewModel)
    {
        _navigationService = navigationService;
        _communitiesService = communitiesService;
        _feedViewModel = feedViewModel;

        LoadCommunities();
    }

    public void LoadCommunities()
    {
        Communities.Clear();
        if (_userSession?.CurrentUser == null) return;

        foreach (Community community in _communitiesService.GetCommunitiesUserIsPartOf(_userSession.CurrentUser.UserID))
            Communities.Add(community);
    }

    [RelayCommand]
    private void NavigateHome()
    {
        _feedViewModel.LoadHome(); 
        _navigationService.NavigateTo(typeof(FeedView));
    }

    [RelayCommand]
    private void NavigateDiscovery()
    {
        _feedViewModel.LoadDiscovery();
        _navigationService.NavigateTo(typeof(FeedView));
    }

    [RelayCommand]
    private void NavigateCreateCommunity()
    {
        _navigationService.NavigateTo(typeof(CreateCommunityView));
    }

    [RelayCommand]
    private void NavigateToCommunity(Community community)
    {
        if (community != null)
        {
            _navigationService.NavigateTo(typeof(CommunityView), community);
        }
    }
}
