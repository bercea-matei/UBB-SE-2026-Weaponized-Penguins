using Boards_WP.Data.Models;
using Boards_WP.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Boards_WP.Views.Pages
{
    public sealed partial class FullPostView : Page
    {
        public FullPostViewModel ViewModel { get; set; }

        public FullPostView()
        {
            ViewModel = App.Current.Services?.GetService<FullPostViewModel>();
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Post selectedPost)
            {
                ViewModel.LoadPost(selectedPost);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            else
            {
                this.Frame.Navigate(typeof(FeedView));
            }
        }
    }
}