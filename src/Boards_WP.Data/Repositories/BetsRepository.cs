using System;
using System.Collections.Generic;
using System.Text;

namespace Boards_WP.Data.Repositories;

public class BetsRepository : IBetsRepository
{
    public void AddBet(Bet b)
    {
        throw new NotImplementedException();
    }

    public void AddUserBet(UsersBets UserBet)
    {
        throw new NotImplementedException();
    }

    public void AddUserTokens(UsersTokens userToken)
    {
        throw new NotImplementedException();
    }

    public List<Bet> GetAllBetsSortedByDate()
    {
        throw new NotImplementedException();
    }

    public Bet GetBetByID(int BetID)
    {
        throw new NotImplementedException();
    }

    public List<Bet> GetBetsByKeywords(string Keywords)
    {
        throw new NotImplementedException();
    }

    public UsersBets GetUserBetByID(int UserID, int BetID)
    {
        throw new NotImplementedException();
    }

    public List<UsersBets> GetUserBetsByBet(int BetID)
    {
        throw new NotImplementedException();
    }

    public List<UsersBets> GetUserBetsByUser(int UserID)
    {
        throw new NotImplementedException();
    }

    public UsersTokens GetUserTokens(int UserID)
    {
        throw new NotImplementedException();
    }

    public void RemoveBet(int BetID)
    {
        throw new NotImplementedException();
    }

    public void UpdateBetAmounts(int BetID, int YesAmount, int NoAmount)
    {
        throw new NotImplementedException();
    }

    public void UpdateUserTokens(int UserID, int NewAmount)
    {
        throw new NotImplementedException();
    }

    public bool UserTokensExist(int UserID)
    {
        throw new NotImplementedException();
    }
}
