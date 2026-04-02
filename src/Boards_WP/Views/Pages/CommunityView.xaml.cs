using Boards_WP.Data.Services.Interfaces;
using Boards_WP.ViewModels;
using Boards_WP.Views.Pages;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Boards_WP.Views.Pages
{
    public sealed partial class CommunityView : Page
    {
        public CommunityViewModel ViewModel { get; }

        public CommunityView() 
        {
            this.InitializeComponent();
            var postsService = App.Services.GetRequiredService<IPostsService>();

            ViewModel = new CommunityViewModel(
                postsService: postsService,
                navigateToCreatePost: community => Frame.Navigate(typeof(CreatePostView), community)
            );
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.ApplyNavigationParameter(e.Parameter);
        }
    }
}