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

        public PostPreviewForm()
        {
            this.InitializeComponent();
        }

        // when PostData is received, the ViewModel is created (the bridge)
        private static void OnPostDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PostPreviewForm control && e.NewValue is Post post)
            {
                control.ViewModel = new PostPreviewViewModel(post);
                control.Bindings.Update(); // UI displays the new ViewModel
            }
        }

        
        private void OnPostClicked(object sender, PointerRoutedEventArgs e)
        {
            ViewModel?.OpenPostCommand.Execute(null);
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }

        
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
    }
}