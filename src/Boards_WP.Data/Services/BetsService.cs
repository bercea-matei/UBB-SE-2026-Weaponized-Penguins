using System;
using System.Collections.Generic;
using System.Text;

namespace Boards_WP.Data.Services;

public class BetsService : IBetsService
{
    private readonly IBetsRepository _betsRepository;
    private readonly IUsersService _usersService;
    private readonly ICommunitiesService _communitiesService;
    private readonly ICommentsService _commentsService;

    public BetsService(IBetsRepository betsRepository, IUsersService usersService, ICommunitiesService communitiesService, ICommentsService commentsService)
    {
        _betsRepository = betsRepository;
        _usersService = usersService;
        _communitiesService = communitiesService;
        _commentsService = commentsService;
    }
    public (decimal YesOdd, decimal NoOdd) CalculateBetOdds(int BetID)
    {
        throw new NotImplementedException();
    }

    public BetVote CheckBetCondition(int BetID)
    {
        throw new NotImplementedException();
    }

    public void CreateBet(Bet CreatedBet, int CreatorID)
    {
        try
        {
            CreatedBet.BetID = _betsRepository.AddBet(CreatedBet);
            _betsRepository.UpdateUserTokens(CreatorID, _betsRepository.GetUserTokens(CreatorID).TokensNumber - 5);
        }
        catch
        {
            throw new Exception("Failed to create bet.");
        }
    }

    public void ExecuteActionsByBetResult(int UserID, int BetID)
    {
        throw new NotImplementedException();
    }

    public string ExtractBetKeywords(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        int firstSpaceIndex = input.IndexOf(' ');

        if (firstSpaceIndex == -1)
            return string.Empty; 

        return input.Substring(firstSpaceIndex + 1).Trim();
    }

    public List<Bet> GetAllBets()
    {
        try
        {
            return _betsRepository.GetAllBetsSortedByDate();

        }
        catch
        {
            throw new Exception("Failed to retrieve bets.");
        }
    }

    public Bet GetBetByID(int BetID)
    {
        try
        {
            return _betsRepository.GetBetByID(BetID);

        }
        catch
        {
            throw new Exception("Failed to retrieve bet.");
        }
    }

    public List<Bet> GetExpiredBetsOfUser(int UserID)
    {
        List<Bet> ExpiredBets = new List<Bet>();

        List<UsersBets> AllBetsOfUser = _betsRepository.GetUserBetsByUser(UserID);
        foreach (UsersBets bet in AllBetsOfUser)
        {
            if(bet.SelectedBet.EndingTime < DateTime.Now)
                ExpiredBets.Add(bet.SelectedBet);
        }

        return ExpiredBets;
    }

    public int GetUserTokenCount(int UserID)
    {
        try
        {
            UsersTokens UserTokensObj = _betsRepository.GetUserTokens(UserID);
            return UserTokensObj.TokensNumber;
        }
        catch
        {
            throw new Exception("Failed to retrieve user token count.");
        }
    }

    public float GetUserTokenFeeDiscount(int UserID)
    {
        throw new NotImplementedException();
    }

    public bool IsSecretKey(string Input)
    {
        return Input.StartsWith("/weaponizedpenguins");
    }

    public void PlaceUserBet(int UserID, int BetID, int Amount, BetVote Vote)
    {
        try
        {
            UsersBets PlacedBet = new UsersBets
            {
                BettingUser = _usersService.GetUserByID(UserID),
                SelectedBet = _betsRepository.GetBetByID(BetID),
                Amount = Amount,
                Vote = Vote,
                Odd = Vote == BetVote.YES ? CalculateBetOdds(BetID).YesOdd : CalculateBetOdds(BetID).NoOdd
            };

            _betsRepository.AddUserBet(PlacedBet);
        }
        catch
        {
            throw new Exception("Failed to place user bet.");
        }
    }

    public void ResolveBet(int BetID)
    {
        throw new NotImplementedException();
    }

    public List<Bet> SearchBetsByKeywords(string Keywords)
    {
        try
        {
            return _betsRepository.GetBetsByKeywords(Keywords);
        }
        catch
        {
            throw new Exception("Failed to search bets by keywords.");
        }
    }

    public bool ValidateCreateBet(int UserID, Bet CreatedBet)
    {
        throw new NotImplementedException();
    }

    public bool ValidatePlaceUserBet(int UserID, int BetID, int Amount, BetVote Vote)
    {
        throw new NotImplementedException();
    }
}
