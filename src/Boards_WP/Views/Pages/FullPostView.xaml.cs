using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using Boards_WP.Data.Models;

namespace Boards_WP.Views.Pages
{
    public sealed partial class FullPostView : Page
    {
        public ViewModels.FullPostViewModel ViewModel { get; }

        public FullPostView()
        {
            this.InitializeComponent();

            
            ViewModel = App.Services.GetRequiredService<ViewModels.FullPostViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Post clickedPost)
            {
                ViewModel.Initialize(clickedPost);
                this.Bindings.Update();
            }
        }

        
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
    }
}