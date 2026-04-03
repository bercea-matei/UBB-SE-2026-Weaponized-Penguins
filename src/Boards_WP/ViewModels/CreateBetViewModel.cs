using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Boards_WP.ViewModels
{
    public partial class CreateBetViewModel : ObservableObject
    {
        private readonly IBetsService _betsService;

        public CreateBetViewModel(IBetsService betsService)
        {
            _betsService = betsService;

            Communities = new ObservableCollection<string>
            {
                "UBB",
                "Weaponized Penguins",
                "Programming"
            };

            SelectedCommunity = Communities[0];
            EndDate = DateTime.Now;
            EndTime = DateTime.Now.TimeOfDay;
        }


        [ObservableProperty]
        private string expression;

        [ObservableProperty]
        private string selectedCommunity;

        [ObservableProperty]
        private bool isPost = true;

        [ObservableProperty]
        private DateTimeOffset endDate;

        [ObservableProperty]
        private TimeSpan endTime;


        public ObservableCollection<string> Communities { get; }

        [ObservableProperty]
        private string errorMessage;


        [RelayCommand]
        private void CreateBet()
        {
            try
            {
                int currentUserId = 1; // replace later

                int communityId = SelectedCommunity switch
                {
                    "UBB" => 1,
                    "Weaponized Penguins" => 2,
                    "Programming" => 3,
                    _ => 1
                };

                Bet newBet = new Bet
                {
                    Expression = Expression,
                    StartingTime = DateTime.Now,
                    EndingTime = EndDate.DateTime.Date + EndTime,
                    Type = IsPost ? BetType.Post : BetType.Comment,
                    YesAmount = 0,
                    NoAmount = 0,
                    BetCommunity = new Community { CommunityID = communityId }
                };

                _betsService.ValidateCreateBet(currentUserId, newBet);
                _betsService.CreateBet(newBet, currentUserId);

                ErrorMessage = null;

               
                BetCreated?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public event Action BetCreated;

        [RelayCommand]
        private void Cancel()
        {
            BetCancelled?.Invoke();
        }

        public event Action BetCancelled;
    }
}