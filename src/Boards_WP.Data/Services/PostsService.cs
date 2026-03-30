using System;
using System.Collections.Generic;
using System.Text;

namespace Boards_WP.Data.Services;

public class PostsService : IPostsService
{
    private readonly IPostsRepository _postsRepo;
    private readonly IUsersRepository _usersRepo;
    private readonly ITagsRepository _tagsRepo;
    //community service
    private readonly UserSession _userSession;

    private List<Post> _lastLikesOfCurrentUser= new List<Post>();  

    public PostsService(IPostsRepository postsRepo, IUsersRepository usersRepo, ITagsRepository _tagsRepo, UserSession userSession)
    {
        _postsRepo = postsRepo;
        _usersRepo = usersRepo;
        _tagsRepo = _tagsRepo;
        _userSession = userSession;
    }


    public void AddPost(Post post)
    {
        //todo : validate post
        if (string.IsNullOrWhiteSpace(post.Title))
            throw new ArgumentException("Post title cannot be empty.");

        _postsRepo.AddPost(post);
    }


    public Post GetPostByPostID(int postId)
    {
        return _postsRepo.GetPostByPostID(postId);
    }

    public void DeletePost(int postId)
    {
        _postsRepo.DeletePost(postId);
    }

    public void IncreaseScore(int postId)
    {
        
        int userId = _userSession.CurrentUser.UserID;
        VoteType currentVote = _postsRepo.GetUserVoteForPost(userId, postId);

        if (currentVote == VoteType.Like)
        {
            
            _postsRepo.SetUserVoteForPost(userId, postId, VoteType.None);
            _postsRepo.DecreaseScore(postId);
        }
        else if (currentVote == VoteType.Dislike)
        {
            
            _postsRepo.SetUserVoteForPost(userId, postId, VoteType.Like);

            _postsRepo.IncreaseScore(postId);
            _postsRepo.IncreaseScore(postId);
        }
        else
        { 
            _postsRepo.SetUserVoteForPost(userId, postId, VoteType.Like);
            _postsRepo.IncreaseScore(postId);
        }
    }

    public void DecreaseScore(int postId)
    {
        int userId = _userSession.CurrentUser.UserID;
        VoteType currentVote = _postsRepo.GetUserVoteForPost(userId, postId);

        if (currentVote == VoteType.Dislike)
        {
            
            _postsRepo.SetUserVoteForPost(userId, postId, VoteType.None);

            _postsRepo.IncreaseScore(postId);
        }
        else if (currentVote == VoteType.Like)
        {

            _postsRepo.SetUserVoteForPost(userId, postId, VoteType.Dislike);

            _postsRepo.DecreaseScore(postId);
            _postsRepo.DecreaseScore(postId);
        }
        else
        {
            _postsRepo.SetUserVoteForPost(userId, postId, VoteType.Dislike);
            _postsRepo.DecreaseScore(postId);
        }
    }

    public List<Post> GetPostsByCommunityID(int communityId)
    {
        return _postsRepo.GetPostsByCommunityIDs(new[] { communityId });
    }

    public List<Post> GetPostsForHomePage(int userId)
    {
        List<Community> communities = new List<Community>();//= _communitiesRepo.GetCommunitiesUserIsPartOf(userId);

        int[] communityIds = communities.Select(c => c.CommunityID).ToArray();

        return _postsRepo.GetPostsByCommunityIDs(communityIds);
    }

    public List<Post> GetPostsForDiscoveryPage(int userId)
    {
        List<Community> communities = new List<Community>();//= _communitiesRepo.GetCommunitiesUserIsPartOf(userId);

        int[] communityIds = communities.Select(c => c.CommunityID).ToArray();

        return _postsRepo.GetPostExceptCommunityIDs(communityIds);
    }

    public ThemeColor DetermineFeedThemeColorByLastLikes()
    {
        throw new NotImplementedException();
        //todo
    }

    public ThemeColor DetermineThemeForASinglePost(Post post)
    {
        throw new NotImplementedException();
        //todo
    }


    public void UpdateUserPreferences(int userID, Post p, bool hasCommented)
    {
        throw new NotImplementedException();
    }
}
