using System;
using System.Collections.Generic;
using System.Text;

namespace Boards_WP.Data.Services;

public class BetsService : IBetsService
{
    private readonly IBetsRepository _betsRepository;
    private readonly IUsersService _usersService;
    private readonly ICommentsService _commentsService;
    private readonly IPostsService _postsService;
    public BetsService(IBetsRepository betsRepository, IUsersService usersService, ICommentsService commentsService, IPostsService postsService)
    {
        _betsRepository = betsRepository;
        _usersService = usersService;
        _commentsService = commentsService;
        _postsService = postsService;
    }
    public (decimal YesOdd, decimal NoOdd) CalculateBetOdds(int BetID, int UserID)
    {
        try
        {
            Bet bet = _betsRepository.GetBetByID(BetID);

            decimal Vyes = bet.YesAmount;
            decimal Vno = bet.NoAmount;

            const decimal S = 1000m;
            const decimal M = 0.05m;

            decimal Pyes = (Vyes + S) / (Vyes + Vno + (2 * S));
            decimal Pno = (Vno + S) / (Vyes + Vno + (2 * S));

            decimal M_dynamic_yes = M + Math.Max(0, Pyes - 0.50m) * 0.2m;
            decimal M_dynamic_no = M + Math.Max(0, Pno - 0.50m) * 0.2m;

            decimal discount = GetUserTokenFeeDiscount(UserID);

            decimal M_final_yes = M_dynamic_yes * (1 - discount);
            decimal M_final_no = M_dynamic_no * (1 - discount);

            decimal YesOdd = 1 / (Pyes * (1 + M_final_yes));
            decimal NoOdd = 1 / (Pno * (1 + M_final_no));

            return (YesOdd, NoOdd);
        }
        catch
        {
            throw new Exception("Failed to calculate bet odds.");
        }
    }

    private List<Post> GetPostsFromBetInterval(Bet b)
    {
        List<Post> PostsFromInterval = new List<Post>();

        List<Post> AllPosts = _postsService.GetPostsByCommunityID(b.BetCommunity.CommunityID);

        foreach(Post post in AllPosts)
        {
            if (post.CreationTime >= b.StartingTime && post.CreationTime <= b.EndingTime)
                PostsFromInterval.Add(post);
        }

        return PostsFromInterval;

    }

    public BetVote CheckBetCondition(int BetID)
    {
        try
        {
            Bet BetToCheck = _betsRepository.GetBetByID(BetID);
            String Expression = BetToCheck.Expression;

            
            List<Post> PostsFromBetInterval = GetPostsFromBetInterval(BetToCheck);

            if(BetToCheck.Type == BetType.Post)
            {
                foreach (Post p in PostsFromBetInterval)
                    if (p.Title.Contains(Expression) || p.Description.Contains(Expression))
                        return BetVote.YES;
            }
            else
            {
                foreach (Post p in PostsFromBetInterval)
                {

                   List<Comment> CommentsOfPost = _commentsService.GetCommentsByPost(p.PostID, p.Owner.UserID); //ID of owner because for this thing I don't care about which user is asking for the comments, I just want all the comments of the post
                   foreach (Comment c in CommentsOfPost)
                        if (c.Description.Contains(Expression))
                            return BetVote.YES;
                }
            }

            return BetVote.NO;
        }
        catch
        {
            throw new Exception("Failed to check bet.");
        }
        
    }

    public void CreateBet(Bet CreatedBet, int CreatorID)
    {
        try
        {
            CreatedBet.BetID = _betsRepository.AddBet(CreatedBet);
            _betsRepository.UpdateUserTokens(CreatorID, _betsRepository.GetUserTokens(CreatorID).TokensNumber - 5);
        }
        catch (SqlException ex)
        {
            System.Diagnostics.Debug.WriteLine($"SQL Error: {ex.Message}");
        }
    }

    public void ExecuteActionsByBetResult(int UserID, int BetID)
    {
        try
        {
            if (didUserWinBet(UserID, BetID) == true)
            {
                UsersBets BetPlacedByUser = _betsRepository.GetUserBetByID(UserID, BetID);

                int Winnings = (int)Math.Ceiling(BetPlacedByUser.Amount * (BetPlacedByUser.Vote == BetVote.YES ? CalculateBetOdds(BetID, UserID).YesOdd : CalculateBetOdds(BetID, UserID).NoOdd));
                _betsRepository.UpdateUserTokens(UserID, _betsRepository.GetUserTokens(UserID).TokensNumber + Winnings);
            }
        }
        catch
        {
            throw new Exception("Failed to execute actions by bet result.");
        }
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

    public decimal GetUserTokenFeeDiscount(int UserID)
    {
        try
        {
            UsersTokens userTokens = _betsRepository.GetUserTokens(UserID);

            decimal Usold = userTokens.TokensNumber;
            const decimal Umax = 10000m;

            decimal discount = Math.Min(0.50m, (Usold / Umax) * 0.50m);

            return discount;
        }
        catch
        {
            throw new Exception("Failed to calculate user discount.");
        }
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
                Odd = Vote == BetVote.YES ? CalculateBetOdds(BetID, UserID).YesOdd : CalculateBetOdds(BetID, UserID).NoOdd
            };

            _betsRepository.AddUserBet(PlacedBet);
        }
        catch
        {
            throw new Exception("Failed to place user bet.");
        }
    }

    public Boolean didUserWinBet(int UserID, int BetID)
    {
        try
        {
            UsersBets UserBet = _betsRepository.GetUserBetByID(UserID, BetID);
            BetVote BetResult = CheckBetCondition(BetID);
            return UserBet.Vote == BetResult;
        }
        catch
        {
            throw new Exception("Failed to check if user won bet.");
        }
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
        try
        {
            int UserTokens = _betsRepository.GetUserTokens(UserID).TokensNumber;
            if (UserTokens < 5)
                throw new Exception("User does not have enough tokens to create a bet.");
            if (CreatedBet.StartingTime >= CreatedBet.EndingTime)
                throw new Exception("Bet starting time must be before ending time.");
            if (CreatedBet.Expression.Length < 3)
                throw new Exception("Bet expression must have at least 3 characters.");
            if (CreatedBet.Expression.Length > 50)
                throw new Exception("Bet expression must have at most 50 characters.");
            return true;
        }
        catch
        {
            throw new Exception("Failed to validate create bet.");
        }
    }

    public bool ValidatePlaceUserBet(int UserID, int Amount)
    {
        try
        {
            int UserTokens = _betsRepository.GetUserTokens(UserID).TokensNumber;

            if (Amount <= 0)
                throw new Exception("Bet amount must be greater than 0.");
            if (UserTokens < Amount)
                throw new Exception("User does not have enough tokens to place this bet.");
            if(Amount > 1000)
                throw new Exception("Bet amount must be less than or equal to 1000 tokens.");

            return true;

        }
        catch
        {
            throw new Exception("Failed to validate place user bet.");
        }
    }
}
