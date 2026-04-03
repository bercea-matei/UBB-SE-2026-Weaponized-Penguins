using System;

using Boards_WP.Data.Models;

namespace Boards_WP.ViewModels
{
    public class UserBetItemViewModel
    {
        public Bet BetData { get; }
        public int BetAmount { get; }
        public BetVote Vote { get; }
        public decimal LockedOdd { get; }

        public string LockedOddText => $"{LockedOdd:F2}x";
        public string PotentialWinText => $"{Math.Ceiling(BetAmount * LockedOdd):0}";

        public UserBetItemViewModel(UsersBets userBet)
        {
            BetData = userBet.SelectedBet;
            BetAmount = userBet.Amount;
            Vote = userBet.Vote;
            LockedOdd = userBet.Odd;
        }
    }
}
