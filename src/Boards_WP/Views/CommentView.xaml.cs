using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Boards_WP.Data.Models;

namespace Boards_WP.Views
{
    public sealed partial class CommentView : UserControl
    {
        public static readonly DependencyProperty CommentDataProperty =
            DependencyProperty.Register("CommentData", typeof(Comment), typeof(CommentView),
                new PropertyMetadata(null, OnDataChanged));

        public Comment CommentData
        {
            get => (Comment)GetValue(CommentDataProperty);
            set => SetValue(CommentDataProperty, value);
        }

        public CommentView()
        {
            this.InitializeComponent();
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CommentView)d;
            if (e.NewValue is Comment comment)
            {
                // adding the new line, so moving the comment a bit to the right based on its indendation
                control.Margin = new Thickness(comment.Indentation * 35, 0, 0, 12);

                if (comment.Indentation > 0)
                {
                    control.ThreadLine.Visibility = Visibility.Visible;
                }
                else
                {
                    control.ThreadLine.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Upvote_Click(object sender, RoutedEventArgs e)
        {
            if (CommentData == null) return;

            if (CommentData.UserCurrentVote == VoteType.Like) // if the user already liked the comment, and they press the up-arrow again, we cancel their like
            {
                CommentData.Score--;
                CommentData.UserCurrentVote = VoteType.None;
            }
            else if (CommentData.UserCurrentVote == VoteType.Dislike)  // if the user changed their mind, and went from dislike to like, we increment the post by 2
            {
                CommentData.Score += 2;
                CommentData.UserCurrentVote = VoteType.Like;
            }
            else // if it is the first time they interact with the post, we simply increment the score by 1
            {
                CommentData.Score++;
                CommentData.UserCurrentVote = VoteType.Like;
            }

            ScoreLabel.Text = CommentData.Score.ToString();
        }

        private void Downvote_Click(object sender, RoutedEventArgs e)
        {
            if (CommentData == null) return;

            if (CommentData.UserCurrentVote == VoteType.Dislike)
            {
                CommentData.Score++;
                CommentData.UserCurrentVote = VoteType.None;
            }
            else if (CommentData.UserCurrentVote == VoteType.Like)
            {
                CommentData.Score -= 2;
                CommentData.UserCurrentVote = VoteType.Dislike;
            }
            else
            {
                CommentData.Score--;
                CommentData.UserCurrentVote = VoteType.Dislike;
            }

            ScoreLabel.Text = CommentData.Score.ToString();
        }
    }
}