/*using Xunit;

using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories;
using Boards_WP.Data.Services;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Boards_WP.Tests
{
    public class PostsServiceTests
    {
        // UPDATE THIS to your actual local SQL connection string
        private const string ConnectionString = "Data Source=DESKTOP\\SQLEXPRESS;Initial Catalog=Communities;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        private readonly PostsService _postsService;
        private readonly UserSession _userSession;
        private readonly PostsRepository _realPostsRepo;

        public PostsServiceTests()
        {
            // Ensure DB is clean and seeded before every test
            SeedDatabase();

            // 1. Setup the User Session
            _userSession = new UserSession
            {
                CurrentUser = new User { UserID = 1, Username = "TagTester" }
            };

            // 2. Instantiate the REAL repositories that are currently implemented
            _realPostsRepo = new PostsRepository(ConnectionString);
            var realUsersRepo = new UsersRepository(ConnectionString);
            var realTagsRepo = new TagsRepository(ConnectionString);
            var realMoodRepo = new UsersMoodRepository(ConnectionString);

            // 3. Instantiate the Service, injecting the Dummy for the missing CommunitiesService
            _postsService = new PostsService(
                _realPostsRepo,
                realUsersRepo,
                realTagsRepo,
                _userSession,
                realMoodRepo,
                new DummyCommunitiesService() // Using the Dummy here
            );
        }

        // ==========================================
        // 1. CRUD & AUTHORIZATION TESTS
        // ==========================================

        [Fact]
        public void AddPost_Should_Validate_And_Insert_Successfully()
        {
            // Arrange
            var newPost = new Post
            {
                Owner = new User { UserID = 1 },
                ParentCommunity = new Community { CommunityID = 1 }, // Required to pass validation & SQL FK
                Title = "Full Integration Test",
                Description = "Testing actual inserts with available real dependencies.",
                Score = 0,
                CommentsNumber = 0,
                CreationTime = DateTime.Now,
                Image = new byte[10],
                Tags = new List<Tag> { new Tag { TagID = 1 } }
            };

            // Act 
            _postsService.AddPost(newPost);

            // Assert
            var allPosts = _realPostsRepo.GetPostsByCommunityIDs(new[] { 1 });
            var savedPost = allPosts.FirstOrDefault(p => p.Title == "Full Integration Test");

            Assert.NotNull(savedPost);
            Assert.Equal(1, savedPost.ParentCommunity.CommunityID);
            Assert.Equal(1, savedPost.Owner.UserID);
        }

        [Fact]
        public void DeletePost_AsOwner_Should_Delete_Successfully()
        {
            // Arrange
            InsertTestPostIntoDb(100, 1, 1); // PostID 100, Owner 1, Comm 1

            // Act
            _postsService.DeletePost(100);

            // Assert
            var deletedPost = _realPostsRepo.GetPostByPostID(100);
            Assert.Null(deletedPost);
        }

        [Fact]
        public void DeletePost_AsNonOwner_Should_Throw_Unauthorized()
        {
            // Arrange 
            InsertTestPostIntoDb(101, 2, 1); // Owned by User 2

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() => _postsService.DeletePost(101));

            // Verify the post was NOT deleted
            var stillExists = _realPostsRepo.GetPostByPostID(101);
            Assert.NotNull(stillExists);
        }

        // ==========================================
        // 2. THEME COLOR TESTS (Pure Logic)
        // ==========================================

        [Fact]
        public void DetermineThemeForASinglePost_WithTags_ReturnsCorrectMappedColor()
        {
            // Arrange 
            var post = new Post
            {
                Tags = new List<Tag>
                {
                    new Tag { CategoryBelongingTo = new Category { CategoryID = 1 } }
                }
            };

            // Act
            var result = _postsService.DetermineThemeForASinglePost(post);

            // Assert
            Assert.Equal(ThemeColor.Pink, result);
        }

        [Fact]
        public void DetermineFeedThemeColorByLastLikes_WithRecentLikes_ReturnsDominantColor()
        {
            // Arrange 
            var likedPosts = new List<Post>
            {
                new Post { Tags = new List<Tag> { new Tag { CategoryBelongingTo = new Category { CategoryID = 4 } } } },
                new Post { Tags = new List<Tag> { new Tag { CategoryBelongingTo = new Category { CategoryID = 4 } } } },
                new Post { Tags = new List<Tag> { new Tag { CategoryBelongingTo = new Category { CategoryID = 1 } } } }
            };

            var fieldInfo = typeof(PostsService).GetField("_lastLikesOfCurrentUser", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(_postsService, likedPosts);

            // Act
            var result = _postsService.DetermineFeedThemeColorByLastLikes();

            // Assert
            Assert.Equal(ThemeColor.Orange, result);
        }

        // ==========================================
        // DATABASE SEEDING HELPERS
        // ==========================================

        private void SeedDatabase()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            // Clean up existing test data to prevent primary key collisions
            ExecuteSql(connection, "DELETE FROM PostTags; DELETE FROM Posts;");

            // Seed Users
            ExecuteSql(connection,
                "IF NOT EXISTS (SELECT 1 FROM Users WHERE userID = 1) " +
                "BEGIN SET IDENTITY_INSERT Users ON; " +
                "INSERT INTO Users (userID, username, email, passwordHash) VALUES (1, 'TagTester', 'tag@test.com', 'hash'); " +
                "SET IDENTITY_INSERT Users OFF; END");

            ExecuteSql(connection,
                "IF NOT EXISTS (SELECT 1 FROM Users WHERE userID = 2) " +
                "BEGIN SET IDENTITY_INSERT Users ON; " +
                "INSERT INTO Users (userID, username, email, passwordHash) VALUES (2, 'OtherUser', 'other@test.com', 'hash'); " +
                "SET IDENTITY_INSERT Users OFF; END");

            // Seed Community (Required for SQL Foreign Key, even if service isn't implemented)
            ExecuteSql(connection,
                "IF NOT EXISTS (SELECT 1 FROM Communities WHERE communityID = 1) " +
                "BEGIN SET IDENTITY_INSERT Communities ON; " +
                "INSERT INTO Communities (communityID, name, description, adminID) VALUES (1, 'TechComm', 'Desc', 1); " +
                "SET IDENTITY_INSERT Communities OFF; END");

            // Seed Categories
            ExecuteSql(connection,
                "IF NOT EXISTS (SELECT 1 FROM Categories WHERE categoryID = 1) " +
                "BEGIN SET IDENTITY_INSERT Categories ON; " +
                "INSERT INTO Categories (categoryID, categoryName, categoryColor) VALUES (1, 'Programming', '#178600'); " +
                "SET IDENTITY_INSERT Categories OFF; END");

            // Seed Tags
            ExecuteSql(connection,
                "IF NOT EXISTS (SELECT 1 FROM Tags WHERE tagID = 1) " +
                "BEGIN SET IDENTITY_INSERT Tags ON; " +
                "INSERT INTO Tags (tagID, tagCategoryID, tagName) VALUES (1, 1, 'C#'); " +
                "SET IDENTITY_INSERT Tags OFF; END");
        }

        private void InsertTestPostIntoDb(int postId, int ownerId, int communityId)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            ExecuteSql(connection,
                $"SET IDENTITY_INSERT Posts ON; " +
                $"INSERT INTO Posts (postID, ownerID, communityID, title, description, score, commentsNumber, creationTime) " +
                $"VALUES ({postId}, {ownerId}, {communityId}, 'Test', 'Desc', 0, 0, GETDATE()); " +
                $"SET IDENTITY_INSERT Posts OFF;");
        }

        private void ExecuteSql(SqlConnection connection, string sql)
        {
            using var cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
    }

    // ==========================================
    // DUMMY DEPENDENCIES
    // ==========================================
    public class DummyCommunitiesService : ICommunitiesService
    {
        public void AddCommunity(Community AddedCommunity)
        {
            throw new NotImplementedException();
        }

        public void AddUser(int CommunityID, int UserID)
        {
            throw new NotImplementedException();
        }

        public bool CheckOwner(int CommunityID, int UserID)
        {
            throw new NotImplementedException();
        }

        public ThemeColor DetermineCommunityThemeColor(int CommunityID)
        {
            throw new NotImplementedException();
        }

        public List<Community> GetCommunitiesUserIsPartOf(int UserID)
        {
            throw new NotImplementedException();
        }

        // Just returns a blank community so the service doesn't crash if it tries to read from it
        public Community GetCommunityByID(int id) => new Community { CommunityID = id };

        public bool IsPartOfCommunity(int UserID, int CommunityID)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(int CommunityID, int UserID)
        {
            throw new NotImplementedException();
        }

        public List<Community> searchCommunities(string Match)
        {
            throw new NotImplementedException();
        }

        public void UpdateCommunityInfo(int CommunityID, string Description, byte[] NewCommunityPicture, byte[] NewBanner)
        {
            throw new NotImplementedException();
        }
    }
}*/