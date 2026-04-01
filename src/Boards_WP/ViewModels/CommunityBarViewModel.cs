using System;
using System.Collections.Generic;
using System.Text;

using Boards_WP.ViewModels;
using Boards_WP.Views.Pages;

using CommunityToolkit.Mvvm.Input;

public partial class CommunityBarViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ICommunitiesService _communitiesService;
    private readonly FeedViewModel _feedViewModel; // Injected Singleton

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

    private void LoadCommunities()
    {
        // In a real app, fetch from _communitiesService
        Communities.Add(new Community { Name = "ComputerScience" });
        Communities.Add(new Community { Name = "UBB" });
        Communities.Add(new Community { Name = "Weaponized_Penguins" });
    }

    [RelayCommand]
    private void NavigateHome()
    {
        _feedViewModel.LoadHome(); // Change the state of the feed
        _navigationService.NavigateTo(typeof(FeedView));
    }

    [RelayCommand]
    private void NavigateDiscovery()
    {
        _feedViewModel.LoadDiscovery(); // Change the state of the feed
        _navigationService.NavigateTo(typeof(FeedView));
    }

    [RelayCommand]
    private void NavigateCreateCommunity()
    {
        // Instead of passing the collection, the CreateCommunity page 
        // should use the CommunitiesService to add a new one.
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
