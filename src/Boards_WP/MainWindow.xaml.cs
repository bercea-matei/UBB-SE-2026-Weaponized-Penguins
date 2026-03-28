using Microsoft.UI.Xaml;

using System;
using System.Collections.Generic;

using Boards_WP.Data.Models;

namespace Boards_WP
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var posts = new List<Post>
            {
                new Post {
                    Title = "How to make your CV better",
                    OwnerName = "@AlekBindiu",
                    CommunityName = "Computer Science",
                    Score = 50,
                    CommentsNumber = 30,
                    CreationTime = new DateTime(2026, 3, 11),
                    Description = "Text ..................................."
                },
                new Post {
                    Title = ".NET Internship",
                    OwnerName = "@Matei",
                    CommunityName = "UBB",
                    Score = 37,
                    CommentsNumber = 9,
                    CreationTime = new DateTime(2026, 3, 7),
                    Description = "Text ..................................."
                },
                new Post {
                    Title = "My First Meme!",
                    OwnerName = "@AlexandraBochis",
                    CommunityName = "UBB",
                    Score = 17,
                    CommentsNumber = 5,
                    CreationTime = new DateTime(2026, 2, 28),
                    Description = "Text ..................................."
                }
            };

            FeedList.ItemsSource = posts;
        }
    }
}