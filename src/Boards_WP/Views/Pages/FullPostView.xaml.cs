using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Boards_WP.Data.Models;

namespace Boards_WP.Views.Pages
{
    public sealed partial class FullPostView : Page
    {
        public FullPostView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Post selectedPost)
            {
                FullPostHeader.PostData = selectedPost;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Check if there is a page to go back to
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            else
            {
                // Fallback: manually navigate to FeedView if history is empty
                this.Frame.Navigate(typeof(FeedView));
            }
        }
    }
}