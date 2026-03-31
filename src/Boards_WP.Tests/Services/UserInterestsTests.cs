namespace Boards_WP.Tests.Services;

public class UserInterestsTests
{
    private readonly PostsService _service;

    public UserInterestsTests()
    {
        //--no repos for testing the alg math -> pass null
        _service = new PostsService(null!, null!, null!, null!, null!);
    }

    private Dictionary<int, int> GetStartingScores(int count)
    {
        var scores = new Dictionary<int, int>();
        for (int i = 1; i <= count; i++) scores.Add(i, 10000 / count);
        scores[1] += 10000 % count;
        return scores;
    }

    [Theory]
    [InlineData(VoteType.Like, false)]
    [InlineData(VoteType.Dislike, true)]
    [InlineData(VoteType.None, false)]
    [InlineData(VoteType.None, true)]
    public void Algorithm_AlwaysPreserves10000Total(VoteType vote, bool commented)
    {
        int userId = 1;
        var startScores = GetStartingScores(24);
        var post = new Post
        {
            Tags = new List<Tag> {
                new Tag { CategoryBelongingTo = new Category { CategoryID = 5 } }
            }
        };

        var result = _service.UserInterestsAlgorithm(userId, post, vote, commented, startScores);

        int finalSum = result.Values.Sum();
        Assert.Equal(10000, finalSum);
        Assert.True(result.Values.All(v => v >= 0), "Logic created a negative score!");
    }
}