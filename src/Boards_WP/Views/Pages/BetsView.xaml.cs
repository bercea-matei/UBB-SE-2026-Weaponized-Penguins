using Boards_WP.ViewModels;

using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Boards_WP.Views.Pages
{
    public sealed partial class BetsView : Page
    {
        public BetsViewModel ViewModel { get; }

        public BetsView()
        {
            ViewModel = App.GetService<BetsViewModel>();
            ViewModel.CreateBetCommand = new RelayCommand(NavigateToCreateBet);
            this.InitializeComponent();
        }

        private void NavigateToCreateBet()
        {
            this.Frame.Navigate(typeof(CreateBetView));
        }

    }
}