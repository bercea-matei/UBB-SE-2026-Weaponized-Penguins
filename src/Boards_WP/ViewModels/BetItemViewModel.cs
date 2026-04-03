using System;

using Boards_WP.Data.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Boards_WP.ViewModels
{
    public partial class BetItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Bet _betData;

        [ObservableProperty]
        private string _yesOdds;

        [ObservableProperty]
        private string _noOdds;

        public string TimeLeft
        {
            get
            {
                var span = BetData.EndingTime - DateTime.Now;
                if (span.TotalSeconds <= 0) return "Expired";

                return span.Days > 0
                    ? $"{span.Days}d {span.Hours:D2}h"
                    : $"{span.Hours:D2}h {span.Minutes:D2}m";
            }
        }

        public BetItemViewModel(Bet bet, decimal yesOdd, decimal noOdd)
        {
            BetData = bet;
            YesOdds = $"{yesOdd:F2}x";
            NoOdds = $"{noOdd:F2}x";
        }
    }
}