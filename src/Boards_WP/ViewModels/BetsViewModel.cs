using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

namespace Boards_WP.ViewModels
{
    public partial class BetsViewModel : ObservableObject
    {
        private readonly IBetsService _betsService;
        private readonly UserSession _userSession;
        private readonly INavigationService _navigationService;

        private readonly HashSet<int> _claimedBetIDs = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HomeTabOpacity))]
        [NotifyPropertyChangedFor(nameof(UserBetsTabOpacity))]
        private bool _isHomeTabSelected = true;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(OngoingSubTabOpacity))]
        [NotifyPropertyChangedFor(nameof(ExpiredSubTabOpacity))]
        private bool _isOngoingSubTabSelected = true;

        public ObservableCollection<BetItemViewModel> FilteredBets { get; set; } = new();
        public ObservableCollection<UserBetItemViewModel> OngoingUserBets { get; set; } = new();
        public ObservableCollection<UserBetItemViewModel> ExpiredUserBets { get; set; } = new();

        public double HomeTabOpacity => IsHomeTabSelected ? 1.0 : 0.6;
        public double UserBetsTabOpacity => IsHomeTabSelected ? 0.6 : 1.0;
        public double OngoingSubTabOpacity => IsOngoingSubTabSelected ? 1.0 : 0.6;
        public double ExpiredSubTabOpacity => IsOngoingSubTabSelected ? 0.6 : 1.0;

        public ICommand CreateBetCommand { get; set; }

        public BetsViewModel(IBetsService betsService, INavigationService navigationService)
        {
            _betsService = betsService;
            _navigationService = navigationService;
            _userSession = App.Services?.GetService<UserSession>();
            LoadHomeBets();
        }

        private void OnPayoutClaimed(int updatedTokens)
        {
            TokenEvents.NotifyTokensUpdated(updatedTokens);
        }

        private void OpenBetPlacement(Bet bet, BetVote vote, decimal odd)
        {
            var payload = new BetPlacementNavigationData
            {
                SelectedBet = bet,
                SelectedVote = vote,
                SelectedOdd = odd
            };

            _navigationService.NavigateTo(typeof(Views.Pages.PlaceBetView), payload);
        }

        public void Initialize(string? keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                ShowHome();
                return;
            }

            IsHomeTabSelected = true;
            LoadBetsByKeywords(keywords);
        }

        [RelayCommand]
        private void ShowHome()
        {
            IsHomeTabSelected = true;
            LoadHomeBets();
        }

        [RelayCommand]
        private void ShowUserBets()
        {
            IsHomeTabSelected = false;
            IsOngoingSubTabSelected = true;
            LoadCurrentUserBets();
        }

        [RelayCommand]
        private void ShowOngoingSubTab()
        {
            IsOngoingSubTabSelected = true;
        }

        [RelayCommand]
        private void ShowExpiredSubTab()
        {
            IsOngoingSubTabSelected = false;
        }

        private void LoadHomeBets()
        {
            FilteredBets.Clear();
            OngoingUserBets.Clear();
            ExpiredUserBets.Clear();

            var currentUserId = _userSession?.CurrentUser?.UserID ?? 0;
            var allBets = _betsService.GetAllBets();

            foreach (var bet in allBets)
            {
                var oddsUserId = currentUserId == 0 ? 1 : currentUserId;
                var (yesOdd, noOdd) = _betsService.CalculateBetOdds(bet.BetID, oddsUserId);

                FilteredBets.Add(new BetItemViewModel(
                    bet, yesOdd, noOdd,
                    (vote, odd) => OpenBetPlacement(bet, vote, odd)));
            }
        }

        private void LoadBetsByKeywords(string keywords)
        {
            FilteredBets.Clear();
            OngoingUserBets.Clear();
            ExpiredUserBets.Clear();

            var currentUserId = _userSession?.CurrentUser?.UserID ?? 0;
            var bets = _betsService.SearchBetsByKeywords(keywords);

            foreach (var bet in bets)
            {
                var oddsUserId = currentUserId == 0 ? 1 : currentUserId;
                var (yesOdd, noOdd) = _betsService.CalculateBetOdds(bet.BetID, oddsUserId);

                FilteredBets.Add(new BetItemViewModel(
                    bet, yesOdd, noOdd,
                    (vote, odd) => OpenBetPlacement(bet, vote, odd)));
            }
        }

        private void LoadCurrentUserBets()
        {
            FilteredBets.Clear();
            OngoingUserBets.Clear();
            ExpiredUserBets.Clear();

            var currentUserId = _userSession?.CurrentUser?.UserID ?? 0;
            if (currentUserId == 0) return;

            var ongoingBets = _betsService.GetOngoingPlacedBetsOfUser(currentUserId);
            foreach (var bet in ongoingBets)
                OngoingUserBets.Add(new UserBetItemViewModel(bet, _betsService, currentUserId, _claimedBetIDs, OnPayoutClaimed));

            var expiredBets = _betsService.GetExpiredPlacedBetsOfUser(currentUserId);
            foreach (var bet in expiredBets)
                ExpiredUserBets.Add(new UserBetItemViewModel(bet, _betsService, currentUserId, _claimedBetIDs, OnPayoutClaimed));
        }
    }
}