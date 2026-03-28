using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;

using Boards_WP.Data.Models;

using System;

namespace Boards_WP.Views
{
    public sealed partial class PostPreviewForm : UserControl
    {
        public static readonly DependencyProperty PostDataProperty =
            DependencyProperty.Register("PostData", typeof(Post), typeof(PostPreviewForm), new PropertyMetadata(null));

        // we declare an object called "PostData" of type Post, which we will use when the user interacts with one of the posts in preview form
        // we use this object to ensure if for example, the user likes a post, the score won't just change on screen, but will actually modify the score of the post
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

        private void OnPostClicked(object sender, PointerRoutedEventArgs e)
        {
            if (PostData == null) return;

            // We cast App.Current to our specific App class to access the public m_window
            if (App.Current is App myApp && myApp.m_window is MainWindow mainWindow)
            {
                // We use the helper method you wrote in MainWindow
                mainWindow.NavigateToPage(typeof(Pages.FullPostView), this.PostData);
            }
        }

        // Helper to find the Frame without protection level errors
        private T FindChildElementByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T element && (element as FrameworkElement)?.Name == name) return element;
                var result = FindChildElementByName<T>(child, name);
                if (result != null) return result;
            }
            return null;
        }

        private void Upvote_Click(object sender, RoutedEventArgs e)
        {
            // FIX: For standard Button clicks, we don't need e.Handled. 
            // Buttons automatically 'swallow' the click so the Grid behind doesn't see it.
            if (PostData == null) return;
            PostData.Score++;
            ScoreLabel.Text = PostData.Score.ToString();
        }

        private void Downvote_Click(object sender, RoutedEventArgs e)
        {
            if (PostData == null) return;
            PostData.Score--;
            ScoreLabel.Text = PostData.Score.ToString();
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // Change cursor to Hand
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // Change cursor back to default (Arrow)
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }


    }
}