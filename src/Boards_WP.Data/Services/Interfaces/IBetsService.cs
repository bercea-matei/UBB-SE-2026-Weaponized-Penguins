using System;
using System.Collections.Generic;
using Boards_WP.Data.Models;

namespace Boards_WP.Data.Services
{
    public interface IBetsService
    {
        public Boolean IsSecretKey(String Input);
        public String ExtractBetKeywords(String Input);
        public int GetUserTokenCount(int UserID);
        public List<Bet> GetAllBets();
        public List<Bet> SearchBetsByKeywords(String Keywords);
        public Bet GetBetByID(int BetID);
        public Boolean ValidateCreateBet(int UserID, Bet CreatedBet);
        public void CreateBet(int UserID, Bet CreatedBet);
        public Boolean ValidatePlaceUserBet(int UserID, int BetID, int Amount, BetVote Vote);
        public void PlaceUserBet(int UserID, int BetID, int Amount, BetVote Vote);
        public (float YesOdd, float NoOdd) CalculateBetOdds(int BetID);
        public float GetUserTokenFeeDiscount(int UserID);
        public List<Bet> GetExpiredBets();
        public BetVote CheckBetCondition(int BetID);
        public void ResolveBet(int BetID);
        public void ExecuteActionsByBetResult(int UserID, int BetID, Boolean DidWin);
        public Boolean VerifySecretWordForBetOpening(String Input, int BetID);

    }
}
