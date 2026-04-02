using System;

using Boards_WP.ViewModels;
using Boards_WP.Data.Services; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace Boards_WP
{
    public sealed partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel;
        public MainViewModel MainViewModel => _mainViewModel;

        public MainWindow()
        {
            this.InitializeComponent();
            _mainViewModel = App.Services.GetRequiredService<MainViewModel>();
            _mainViewModel.IsLoggedIn = false;

            if (App.Current is App myApp)
            {
                myApp.m_window = this;
            }
            var navigationService = App.Services.GetRequiredService<INavigationService>();
            navigationService.Initialize(LoginFrame);
            navigationService.NavigateTo(typeof(Views.Pages.LoginView));
        }

        public void NavigateToPage(Type pageType, object parameter = null)
        {
            // We use ContentFrame here because this method is typically called 
            // for feed navigation once the user is already inside the app.
            ContentFrame.Navigate(pageType, parameter);
        }
    }
}