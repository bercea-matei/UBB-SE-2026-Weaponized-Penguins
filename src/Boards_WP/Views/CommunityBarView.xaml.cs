using Boards_WP.ViewModels; // Adjust to your actual VM namespace

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Boards_WP.Views
{
    public sealed partial class CommunityBarView : UserControl
    {
        // Reference to our ViewModel
        public CommunityBarViewModel ViewModel { get; }

        public CommunityBarView()
        {
            this.InitializeComponent();

            // Get the ViewModel from the DI container
            ViewModel = App.Services.GetRequiredService<CommunityBarViewModel>();

            // Set the DataContext so XAML can bind to it
            this.DataContext = ViewModel;
        }

        private void CommunityListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Simply call the command in the ViewModel
            if (e.ClickedItem is Data.Models.Community selected)
            {
                ViewModel.NavigateToCommunityCommand.Execute(selected);
            }
        }

        // We can now remove the complex FindChildFrame and manual navigation logic 
        // for Home and Discover because the ViewModel handles them via RelayCommands!

        private void HomeNavigation_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.NavigateHomeCommand.Execute(null);
        }

        private void DiscoverNavigation_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.NavigateDiscoveryCommand.Execute(null);
        }

        private void StartCommunity_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.NavigateCreateCommunityCommand.Execute(null);
        }
    }
}