using System.Collections.ObjectModel;
using System.Windows.Input;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Navigation;

namespace Boards_WP.ViewModels
{
    public partial class BetsViewModel : ObservableObject
    {
        private readonly IBetsService _betsService;
        private readonly UserSession _userSession;

        [ObservableProperty]
        private bool _isHomeTabSelected = true;

        public ObservableCollection<BetItemViewModel> FilteredBets { get; set; } = new();

        public double HomeTabOpacity => IsHomeTabSelected ? 1.0 : 0.6;
        public double UserBetsTabOpacity => IsHomeTabSelected ? 0.6 : 1.0;

        public ICommand CreateBetCommand { get; set; }

        public BetsViewModel(IBetsService betsService)
        {
            _betsService = betsService;
            _userSession = App.Services?.GetService<UserSession>();
            LoadHomeBets();
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

        partial void OnIsHomeTabSelectedChanged(bool value)
        {
            OnPropertyChanged(nameof(HomeTabOpacity));
            OnPropertyChanged(nameof(UserBetsTabOpacity));
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
            LoadCurrentUserBets();
        }

        private void LoadHomeBets()
        {
            FilteredBets.Clear();
            var allBets = _betsService.GetAllBets();

            foreach (var bet in allBets)
            {
                var (yesOdd, noOdd) = _betsService.CalculateBetOdds(bet.BetID, _userSession.CurrentUser.UserID);

                FilteredBets.Add(new BetItemViewModel(bet, yesOdd, noOdd));
            }
        }

        private void LoadBetsByKeywords(string keywords)
        {
            FilteredBets.Clear();

            var currentUserId = _userSession?.CurrentUser?.UserID ?? 0;
            var bets = _betsService.SearchBetsByKeywords(keywords);
            foreach (var bet in bets)
            {
                var oddsUserId = currentUserId == 0 ? 1 : currentUserId;
                var (yesOdd, noOdd) = _betsService.CalculateBetOdds(bet.BetID, oddsUserId);
                FilteredBets.Add(new BetItemViewModel(bet, yesOdd, noOdd));
            }
        }

        private void LoadCurrentUserBets()
        {
            FilteredBets.Clear();

            var currentUserId = _userSession?.CurrentUser?.UserID ?? 0;
            if (currentUserId == 0)
            {
                return;
            }

            var userBets = _betsService.GetBetsOfUser(currentUserId);
            foreach (var bet in userBets)
            {
                var (yesOdd, noOdd) = _betsService.CalculateBetOdds(bet.BetID, currentUserId);
                FilteredBets.Add(new BetItemViewModel(bet, yesOdd, noOdd));
            }
        }
    }
}