using Boards_WP.Data.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;

namespace Boards_WP.Views
{
    public sealed partial class PostPreviewForm : UserControl
    {
        // Define PostData as a DependencyProperty so the ListView can "push" data into it
        public static readonly DependencyProperty PostDataProperty =
            DependencyProperty.Register("PostData", typeof(Post), typeof(PostPreviewForm), new PropertyMetadata(null));

        public Post PostData
        {
            get => (Post)GetValue(PostDataProperty);
            set => SetValue(PostDataProperty, value);
        }

        public PostPreviewForm()
        {
            this.InitializeComponent();
        }

        public string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        private void Upvote_Click(object sender, RoutedEventArgs e)
        {
            if (PostData == null) return;
            PostData.Score++;
            // Manually update the label because simple classes don't notify the UI of changes automatically
            ScoreLabel.Text = PostData.Score.ToString();
        }

        private void Downvote_Click(object sender, RoutedEventArgs e)
        {
            if (PostData == null) return;
            PostData.Score--;
            ScoreLabel.Text = PostData.Score.ToString();
        }
    }
}