using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Input;

using Boards_WP.Data.Models;
using Boards_WP.ViewModels;

namespace Boards_WP.Views
{
    public sealed partial class CommentView : UserControl
    {
        public CommentViewModel ViewModel { get; set; }

        // this is the "input socket" and allows a parent page, like FullPostView, to pass a Comment object into this control
        // when a new object is plugged into the socket, the OnCommentDataChanged function will run
        public static readonly DependencyProperty CommentDataProperty =
            DependencyProperty.Register("CommentData", typeof(Comment), typeof(CommentView),
                new PropertyMetadata(null, OnCommentDataChanged));

        public Comment CommentData
        {
            get => (Comment)GetValue(CommentDataProperty);
            set => SetValue(CommentDataProperty, value);
        }

        public CommentView()
        {
            this.InitializeComponent();
        }

        private static void OnCommentDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CommentView control && e.NewValue is Comment comment)
            {
                control.ViewModel = new CommentViewModel(comment);
                control.Bindings.Update();
            }
        }

        
        private void PostReply_InternalClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsReplyAreaVisible = false;
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }
    }
}