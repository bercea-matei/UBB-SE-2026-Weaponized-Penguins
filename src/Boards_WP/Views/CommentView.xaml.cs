using System;

using Boards_WP.Data.Models;
using Boards_WP.ViewModels;
using Boards_WP.Views.Pages; 

using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace Boards_WP.Views
{
    public sealed partial class CommentView : UserControl
    {
        
        public static readonly DependencyProperty CommentDataProperty =
            DependencyProperty.Register("CommentData", typeof(Comment), typeof(CommentView), new PropertyMetadata(null));

        public Comment CommentData
        {
            get => (Comment)GetValue(CommentDataProperty);
            set => SetValue(CommentDataProperty, value);
        }

        public CommentView()
        {
            this.InitializeComponent();
        }

        
        private FullPostViewModel GetParentViewModel()
        {
            
            if (App.Current is App myApp && myApp.m_window?.Content is Frame rootFrame)
            {
                if (rootFrame.Content is FullPostView page)
                {
                    return page.ViewModel;
                }
            }
            return null;
        }

        private void Upvote_Click(object sender, RoutedEventArgs e)
        {
            if (CommentData == null) return;
            CommentData.Score++;
            this.Bindings.Update();
        }

        private void Downvote_Click(object sender, RoutedEventArgs e)
        {
            if (CommentData == null) return;
            CommentData.Score--;
            this.Bindings.Update();
        }

        
        private void DeleteComment_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = GetParentViewModel();
            if (viewModel != null && CommentData != null)
            {
                
                viewModel.PostComments.Remove(this.CommentData);
            }
        }

        
        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }
        private void Reply_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PostReply_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}