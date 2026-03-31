using Boards_WP.Data.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System;

namespace Boards_WP.Views.Pages
{
    public sealed partial class CreatePostView : Page
    {
        private Community _originCommunity;

        public CreatePostView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Capture which community we are posting to
            if (e.Parameter is Community com)
            {
                _originCommunity = com;
            }
        }

        private void UploadPost_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleInput.Text)) return;

            // Create the post object locally
            var newPost = new Post
            {
                PostID = 999, // Temporary ID
                Title = TitleInput.Text,
                Description = DescriptionInput.Text,
                ParentCommunity = _originCommunity,
                Owner = new User { Username = "@Me" },
                Score = 0,
                CommentsNumber = 0,
                CreationTime = DateTime.Now
            };

            // Navigate back and send the Post as the parameter
            this.Frame.Navigate(typeof(CommunityView), newPost);
        }
    }
}