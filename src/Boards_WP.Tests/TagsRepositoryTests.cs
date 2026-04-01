/*using Xunit;

using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories;

using Microsoft.Data.SqlClient;

using System;
using System.Linq;

namespace Boards_WP.Tests;

public class TagsRepositoryTests
{
    // UPDATE THIS to your actual local SQL connection string
    private const string ConnectionString = "Data Source=DESKTOP\\SQLEXPRESS;Initial Catalog=Communities;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

    public TagsRepositoryTests()
    {
        // Ensure we have at least one Category to attach Tags to, 
        // preventing Foreign Key constraint errors.
        SeedCategory();
    }

    [Fact]
    public void GetAllCategories_Should_Return_Seeded_Categories()
    {
        // Arrange
        var repo = new TagsRepository(ConnectionString);

        // Act
        var categories = repo.GetAllCategories();

        // Assert
        Assert.NotNull(categories);
        Assert.NotEmpty(categories);

        // Verify the category we seeded in the constructor exists in the results
        var seededCategory = categories.FirstOrDefault(c => c.CategoryID == 1);
        Assert.NotNull(seededCategory);
        Assert.Equal("Programming", seededCategory.CategoryName);
        Assert.Equal("#178600", seededCategory.ColorHex);
    }

    [Fact]
    public void AddTag_Should_Insert_Tag_Successfully()
    {
        // Arrange
        var repo = new TagsRepository(ConnectionString);

        // We append a random Guid so the test can be run multiple times 
        // without hitting the "UNIQUE" constraint on tagName if you add one later.
        string uniqueTagName = "TestTag_" + Guid.NewGuid().ToString().Substring(0, 5);

        var newTag = new Tag
        {
            TagName = uniqueTagName,
            CategoryBelongingTo = new Category { CategoryID = 1 } // Matches our seeded category
        };

        // Act
        // If this throws a SqlException, the insert failed.
        repo.AddTag(newTag);

        // Assert
        // Since your TagsRepository doesn't have a GetTag method, 
        // we verify the insert by checking the database directly.
        bool tagExists = false;
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using var command = new SqlCommand("SELECT COUNT(1) FROM Tags WHERE tagName = @Name", connection);
            command.Parameters.AddWithValue("@Name", uniqueTagName);

            // ExecuteScalar returns the first column of the first row (the count)
            tagExists = (int)command.ExecuteScalar() > 0;
        }

        Assert.True(tagExists, "The tag was not found in the database after calling AddTag.");
    }

    /// <summary>
    /// Seeds the Categories table so the Foreign Key constraint for Tags is satisfied.
    /// </summary>
    private void SeedCategory()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        const string sql = @"
            IF NOT EXISTS (SELECT 1 FROM Categories WHERE categoryID = 1) 
            BEGIN 
                SET IDENTITY_INSERT Categories ON; 
                INSERT INTO Categories (categoryID, categoryName, categoryColor) 
                VALUES (1, 'Programming', '#178600'); 
                SET IDENTITY_INSERT Categories OFF; 
            END";

        using var cmd = new SqlCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }
}*/