using System;
using System.Collections.Generic;
using System.Text;

using Boards_WP.Data.Models;

namespace Boards_WP.Data.Repositories.Interfaces;

public interface IPostsRepository
{
    public void AddPost(Post p);
    public void DeletePost(int postID);
    public void IncreaseScore(int postID);
    public void DecreaseScore(int postID);
    public void IncreaseCommentsNumber(int postID);
    public Post GetPostByPostID(int postID);
    public List<Post> GetPostsByCommunityIDs(int[] communityIDs);
    public List<Post> GetPostExceptCommunityIDs(int[] communityIDs);

}
