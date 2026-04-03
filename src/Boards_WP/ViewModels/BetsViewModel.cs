using System.Collections.ObjectModel;
using System.Windows.Input;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services;
using Boards_WP.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

namespace Boards_WP.ViewModels
{
    public partial class BetsViewModel : ObservableObject
    {
        private readonly IBetsService _betsService;
        private readonly UserSession _userSession;
        private readonly MainViewModel _mainViewModel;
        public ObservableCollection<BetItemViewModel> FilteredBets { get; set; } = new();

        public double HomeTabOpacity => 1.0;
        public double UserBetsTabOpacity => 0.6;

        public ICommand ShowHomeCommand { get; set; }
        public ICommand ShowUserBetsCommand { get; set; }
        public ICommand CreateBetCommand { get; set; }

        public BetsViewModel(IBetsService betsService)
        {
            _betsService = betsService;
            _userSession = App.Services?.GetService<UserSession>();
            LoadBets();
        }

        private void LoadBets()
        {
            FilteredBets.Clear();
            var allBets = _betsService.GetAllBets();

            foreach (var bet in allBets)
            {
                var (yesOdd, noOdd) = _betsService.CalculateBetOdds(bet.BetID, _userSession.CurrentUser.UserID);

                FilteredBets.Add(new BetItemViewModel(bet, yesOdd, noOdd));
            }
        }
    }
}