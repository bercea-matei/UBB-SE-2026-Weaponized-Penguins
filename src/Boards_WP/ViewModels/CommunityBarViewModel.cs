using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services; // Ensure this matches your Service namespace
using Boards_WP.ViewModels;
using Boards_WP.Views.Pages;

using CommunityToolkit.Mvvm.Input;

public partial class CommunityBarViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ICommunitiesService _communitiesService;
    private readonly FeedViewModel _feedViewModel;
    private readonly UserSession _userSession; // Injected to get UserID 3

    public ObservableCollection<Community> Communities { get; } = new();

    public CommunityBarViewModel(
        INavigationService navigationService,
        ICommunitiesService communitiesService,
        FeedViewModel feedViewModel,
        UserSession userSession)
    {
        _navigationService = navigationService;
        _communitiesService = communitiesService;
        _feedViewModel = feedViewModel;
        _userSession = userSession;

        LoadCommunities();
    }

    private void LoadCommunities()
    {
        try
        {
            // 1. Clear existing list
            Communities.Clear();

            // 2. Fetch from DB using your specific interface method
            // Uses UserID 3 from your hardcoded UserSession
            var dbCommunities = _communitiesService.GetCommunitiesUserIsPartOf(_userSession.CurrentUser.UserID);

            // 3. Populate the ObservableCollection
            if (dbCommunities != null)
            {
                foreach (var community in dbCommunities)
                {
                    Communities.Add(community);
                }
            }
        }
        catch (Exception ex)
        {
            // Helps you catch SQL connection issues in the Output window
            System.Diagnostics.Debug.WriteLine($"Failed to load communities from DB: {ex.Message}");
        }
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