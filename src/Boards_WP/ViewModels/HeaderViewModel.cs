using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services.Interfaces;

namespace Boards_WP.ViewModels
{
    public partial class HeaderViewModel : ObservableObject
    {
        private readonly ICommunitiesService _communitiesService;
        private readonly INavigationService _navigationService;

        // The Source Generator will create the 'UserTokens' property from this field.
        // This allows 'ViewModel.UserTokens += 5' to work in your View.
        [ObservableProperty]
        private int _userTokens;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Community> _searchResults = new();

        [ObservableProperty]
        private bool _noResultsToggle;

        public HeaderViewModel(ICommunitiesService communitiesService, INavigationService navigationService)
        {
            _communitiesService = communitiesService;
            _navigationService = navigationService;

            // Initialize with a default or fetched value rather than hardcoding 1250 directly in the field
            // In a real app, you might fetch this from a UserSessionService
            UserTokens = 1250;
        }

        /// <summary>
        /// Triggered automatically by the CommunityToolkit when SearchText changes.
        /// </summary>
        partial void OnSearchTextChanged(string searchedValue)
        {
            if (string.IsNullOrWhiteSpace(searchedValue) || searchedValue.Length < 2)
            {
                SearchResults.Clear();
                NoResultsToggle = false;
                return;
            }

            // Perform the search via the service
            var matches = _communitiesService.searchCommunities(searchedValue);

            SearchResults.Clear();
            foreach (var community in matches)
            {
                SearchResults.Add(community);
            }

            NoResultsToggle = (SearchResults.Count == 0);
        }

        [RelayCommand]
        public void SelectCommunity(Community selected)
        {
            if (selected == null) return;

            SearchText = string.Empty;
            SearchResults.Clear();

            // Use the injected navigation service to keep logic out of the View-Behind
            _navigationService.NavigateTo(typeof(Views.Pages.CommunityView), selected);
        }
    }
}