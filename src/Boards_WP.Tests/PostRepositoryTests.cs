/*

using Xunit;
using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace Boards_WP.Tests;

public class PostsRepositoryTests
{
    // UPDATE THIS to your actual local SQL connection string
    private const string ConnectionString = "Data Source=DESKTOP\\SQLEXPRESS;Initial Catalog=Communities;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";


    public PostsRepositoryTests()
    {
        // This ensures the database has the required parent records
        // before any test runs, preventing Foreign Key errors.
        SeedDatabase();
    }

    [Fact]
    public void AddPost_Should_InsertData_Successfully()
    {
        // Arrange
        var repo = new PostsRepository(ConnectionString);
        var newPost = new Post
        {
            Owner = new User { UserID = 1 },
            Community = new Community { CommunityID = 1 },
            Title = "Testing Joins",
            Description = "This post should be retrievable with full user data.",
            Score = 0,
            CommentsNumber = 0,
            Image = null
            
        };

        // Act & Assert
        // If this doesn't throw a SqlException, the insert worked!
        repo.AddPost(newPost);
    }

    [Fact]
    public void GetPostByPostID_Should_Return_Populated_Objects_Via_Join()
    {
        // Arrange
        var repo = new PostsRepository(ConnectionString);

        // We first get a list to find a valid PostID to test against
        var allPosts = repo.GetPostsByCommunityIDs(new[] { 1 });
        var testPost = allPosts.FirstOrDefault();
        Assert.NotNull(testPost); // Fails if database has no posts

        // Act
        var result = repo.GetPostByPostID(testPost.PostID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testPost.PostID, result.PostID);

        // The most important asserts: Did the JOIN work?
        // If these aren't null, your MapReaderToPost successfully read the joined tables!
        Assert.False(string.IsNullOrEmpty(result.Owner.Username));
        Assert.False(string.IsNullOrEmpty(result.Owner.Email));
        Assert.False(string.IsNullOrEmpty(result.Community.Name));
    }

    [Fact]
    public void GetPostsByCommunityIDs_Should_Return_Posts_With_Joins()
    {
        // Arrange
        var repo = new PostsRepository(ConnectionString);
        int[] communityIds = { 1 };

        // Act
        var results = repo.GetPostsByCommunityIDs(communityIds);

        // Assert
        Assert.NotNull(results);
        foreach (var post in results)
        {
            Assert.Equal(1, post.Community.CommunityID);
            Assert.NotNull(post.Owner.Username); // Verifies the JOIN
        }
    }

    [Fact]
    public void IncreaseScore_Should_Increment_By_One()
    {
        // Arrange
        var repo = new PostsRepository(ConnectionString);
        var post = repo.GetPostsByCommunityIDs(new[] { 1 }).FirstOrDefault();
        Assert.NotNull(post);

        int initialScore = post.Score;

        // Act
        repo.IncreaseScore(post.PostID);

        // Assert
        var updatedPost = repo.GetPostByPostID(post.PostID);
        Assert.Equal(initialScore + 1, updatedPost.Score);
    }




    [Fact]
    public void AddPost_Should_Insert_Post_And_Tags_Successfully()
    {
        // Arrange
        var repo = new PostsRepository(ConnectionString);
        var newPost = new Post
        {
            Owner = new User { UserID = 1 },
            Community = new Community { CommunityID = 1 },
            Title = "Testing Post with Tags",
            Description = "This post should have 2 tags attached to it.",
            Score = 0,
            CommentsNumber = 0,
            Image = null,
            // Attach the Tags that we seeded in the constructor
            Tags = new List<Tag>
            {
                new Tag { TagID = 1 }, // "C#"
                new Tag { TagID = 2 }  // "SQL"
            }
        };

        // Act 
        // If this throws an exception, the SQL or Foreign Keys are wrong
        repo.AddPost(newPost);

        // Assert
        // We fetch the list just to confirm it didn't crash
        var posts = repo.GetPostsByCommunityIDs(new[] { 1 });
        Assert.NotEmpty(posts);
    }

    [Fact]
    public void GetPostByPostID_Should_Return_Post_With_Tags_And_Categories()
    {
        // Arrange
        var repo = new PostsRepository(ConnectionString);

        // Let's get the ID of the post we just inserted in the test above
        var allPosts = repo.GetPostsByCommunityIDs(new[] { 1 });
        var targetPost = allPosts.LastOrDefault(p => p.Title == "Testing Post with Tags");

        // If this is null, you need to run AddPost first (or run all tests together)
        Assert.NotNull(targetPost);

        // Act
        // This is the method that uses reader.NextResult()
        var fullPost = repo.GetPostByPostID(targetPost.PostID);

        // Assert - Main Post Data
        Assert.NotNull(fullPost);
        Assert.Equal("Testing Post with Tags", fullPost.Title);
        Assert.NotNull(fullPost.Owner.Username); // Verifies JOIN on Users
        Assert.NotNull(fullPost.Community.Name); // Verifies JOIN on Communities

        // Assert - Tags and Categories Data
        Assert.NotNull(fullPost.Tags);
        Assert.Equal(2, fullPost.Tags.Count); // We inserted 2 tags

        // Check the first tag to ensure the JOIN on Categories worked
        var firstTag = fullPost.Tags[0];
        Assert.Equal("C#", firstTag.TagName);
        Assert.Equal("#178600", firstTag.ColorHex); // Pulled from the Category table
        Assert.NotNull(firstTag.CategoryBelongingTo);
        Assert.Equal("Programming", firstTag.CategoryBelongingTo.CategoryName);
    }

    /// <summary>
    /// Seeds Users, Communities, Categories, and Tags so Foreign Keys don't break.
    /// </summary>
    private void SeedDatabase()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        // 1. Ensure User 1 exists
        ExecuteSql(connection,
            "IF NOT EXISTS (SELECT 1 FROM Users WHERE userID = 1) " +
            "BEGIN SET IDENTITY_INSERT Users ON; " +
            "INSERT INTO Users (userID, username, email, passwordHash) VALUES (1, 'TagTester', 'tag@test.com', 'hash'); " +
            "SET IDENTITY_INSERT Users OFF; END");

        // 2. Ensure Community 1 exists
        ExecuteSql(connection,
            "IF NOT EXISTS (SELECT 1 FROM Communities WHERE communityID = 1) " +
            "BEGIN SET IDENTITY_INSERT Communities ON; " +
            "INSERT INTO Communities (communityID, name, description, adminID) VALUES (1, 'TechComm', 'Desc', 1); " +
            "SET IDENTITY_INSERT Communities OFF; END");

        // 3. Ensure Category 1 exists
        ExecuteSql(connection,
            "IF NOT EXISTS (SELECT 1 FROM Categories WHERE categoryID = 1) " +
            "BEGIN SET IDENTITY_INSERT Categories ON; " +
            "INSERT INTO Categories (categoryID, categoryName, categoryColor) VALUES (1, 'Programming', '#178600'); " +
            "SET IDENTITY_INSERT Categories OFF; END");

        // 4. Ensure Tag 1 and 2 exist and belong to Category 1
        ExecuteSql(connection,
            "IF NOT EXISTS (SELECT 1 FROM Tags WHERE tagID = 1) " +
            "BEGIN SET IDENTITY_INSERT Tags ON; " +
            "INSERT INTO Tags (tagID, tagCategoryID, tagName) VALUES (1, 1, 'C#'); " +
            "SET IDENTITY_INSERT Tags OFF; END");

        ExecuteSql(connection,
            "IF NOT EXISTS (SELECT 1 FROM Tags WHERE tagID = 2) " +
            "BEGIN SET IDENTITY_INSERT Tags ON; " +
            "INSERT INTO Tags (tagID, tagCategoryID, tagName) VALUES (2, 1, 'SQL'); " +
            "SET IDENTITY_INSERT Tags OFF; END");
    }

    // Helper to keep the SeedDatabase method clean
    private void ExecuteSql(SqlConnection connection, string sql)
    {
        using var cmd = new SqlCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }
}*/