<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

=======
>>>>>>> main
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Boards_WP.Data.Models;

namespace Boards_WP.Views.Pages
{
    public sealed partial class FullPostView : Page
    {
<<<<<<< HEAD
      
        public ObservableCollection<Comment> PostComments { get; set; } = new ObservableCollection<Comment>();

        public FullPostView()
        {
            this.InitializeComponent();

        }

=======
        public FullPostView()
        {
            this.InitializeComponent();
        }


        // we use this function to display the preview form of the clicked post (so that when we enter full view post, the preview form gets shown at the top, above the comments)
>>>>>>> main
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // checking if the data we were given is actually an object of type Post, if yes, we display its preview form 
            if (e.Parameter is Post selectedPost)
            {
<<<<<<< HEAD
                
                FullPostHeader.PostData = selectedPost;
                var hardcodedComments = new List<Comment>
                {
                    new Comment
                    {
                        CommentID = 1,
                        Owner = new User { Username = "@FilipOszkar" },
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt",
                        Score = 15,
                        CreationTime = DateTime.Now.AddHours(-2),
                        Indentation = 0
                    },
                    new Comment
                    {
                        CommentID = 2,
                        Owner = new User { Username = "@BerceaMatei" },
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
                        Score = 8,
                        CreationTime = DateTime.Now.AddHours(-1),
                        Indentation = 1
                    },
                    new Comment
                    {
                        CommentID = 3,
                        Owner = new User { Username = "@RazvanBerbecar" },
                        Description = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        Score = 12,
                        CreationTime = DateTime.Now.AddMinutes(-30),
                        Indentation = 2
                    },
                    new Comment
                    {
                        CommentID = 4,
                        Owner = new User { Username = "@BeneIonut" },
                        Description = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo",
                        Score = 2,
                        CreationTime = DateTime.Now.AddMinutes(-10),
                        Indentation = 0
                    }
                };

              
                PostComments.Clear(); // clearing out any comments from previous posts
                foreach (var c in hardcodedComments)  // adding the comments from the current post
                {
                    PostComments.Add(c);
                }
=======
                FullPostHeader.PostData = selectedPost;
>>>>>>> main
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            if (this.Frame.CanGoBack) // take back the user to the page they were previously on
            {
                this.Frame.GoBack();
            }
            else  // in case there is no history, take the user back to the FeedView
            {
=======
            // checking if there is a page to go back to, if there is, then we go back to it
            // CanGoBack is a built-in property that remembers where the user was
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            else
            {
                // if we dont have a previous page to go back to, we tell the app to go back to the FeedView
>>>>>>> main
                this.Frame.Navigate(typeof(FeedView));
            }
        }
    }
}