using System;

namespace Boards_WP.Data.Models
{
    public class UsersBets
    {
        public User BettingUser { get; set; }
        public Bet SelectedBet { get; set; }
        public int Amount { get; set; }
        public float Odd { get; set; }
        public BetVote Vote { get; set; }

    }
}
