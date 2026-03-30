using System;
using System.Collections.Generic;
using System.Text;

namespace Boards_WP.Data.Services;

public class BetsService : IBetsService
{
    public (float YesOdd, float NoOdd) CalculateBetOdds(int BetID)
    {
        throw new NotImplementedException();
    }

    public BetVote CheckBetCondition(int BetID)
    {
        throw new NotImplementedException();
    }

    public void CreateBet(int UserID, Bet CreatedBet)
    {
        throw new NotImplementedException();
    }

    public void ExecuteActionsByBetResult(int UserID, int BetID, bool DidWin)
    {
        throw new NotImplementedException();
    }

    public string ExtractBetKeywords(string Input)
    {
        throw new NotImplementedException();
    }

    public List<Bet> GetAllBets()
    {
        throw new NotImplementedException();
    }

    public Bet GetBetByID(int BetID)
    {
        throw new NotImplementedException();
    }

    public List<Bet> GetExpiredBets()
    {
        throw new NotImplementedException();
    }

    public int GetUserTokenCount(int UserID)
    {
        throw new NotImplementedException();
    }

    public float GetUserTokenFeeDiscount(int UserID)
    {
        throw new NotImplementedException();
    }

    public bool IsSecretKey(string Input)
    {
        throw new NotImplementedException();
    }

    public void PlaceUserBet(int UserID, int BetID, int Amount, BetVote Vote)
    {
        throw new NotImplementedException();
    }

    public void ResolveBet(int BetID)
    {
        throw new NotImplementedException();
    }

    public List<Bet> SearchBetsByKeywords(string Keywords)
    {
        throw new NotImplementedException();
    }

    public bool ValidateCreateBet(int UserID, Bet CreatedBet)
    {
        throw new NotImplementedException();
    }

    public bool ValidatePlaceUserBet(int UserID, int BetID, int Amount, BetVote Vote)
    {
        throw new NotImplementedException();
    }

    public bool VerifySecretWordForBetOpening(string Input, int BetID)
    {
        throw new NotImplementedException();
    }
}
