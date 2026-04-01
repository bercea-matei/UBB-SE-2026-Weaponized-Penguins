using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Input;

using Boards_WP.Data.Models;
using Boards_WP.ViewModels;

namespace Boards_WP.Views
{
    public sealed partial class PostPreviewForm : UserControl
    {
        public PostPreviewViewModel ViewModel { get; set; }

        public static readonly DependencyProperty PostDataProperty =
            DependencyProperty.Register("PostData", typeof(Post), typeof(PostPreviewForm),
                new PropertyMetadata(null, OnPostDataChanged));

        public Post PostData
        {
            get => (Post)GetValue(PostDataProperty);
            set => SetValue(PostDataProperty, value);
        }

        public PostPreviewForm() => this.InitializeComponent();

        private static void OnPostDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PostPreviewForm control && e.NewValue is Post post)
            {
                control.ViewModel = new PostPreviewViewModel(post);

                control.ViewModel.OnDeleteRequested = (p) =>
                {
                    control.Visibility = Visibility.Collapsed;
                };

                // FIX: Check for null to avoid CS1061 or NullReference if XAML isn't ready
                control.Bindings?.Update();
            }
        }

        private void OnPostClicked(object sender, PointerRoutedEventArgs e) => ViewModel?.OpenPostCommand.Execute(null);
        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e) => this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e) => this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
    }
}