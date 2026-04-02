using System;

using Boards_WP.Data.Models;
using Boards_WP.Data.Services;
using Boards_WP.Data.Services.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace Boards_WP.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUsersService _usersService;
        private readonly UserSession _userSession;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        public LoginViewModel(IUsersService usersService, UserSession userSession, INavigationService navigationService)
        {
            _usersService = usersService;
            _userSession = userSession;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private void Login()
        {
            try
            {
                var user = _usersService.Login(Email, Password);
                if (user != null)
                {
                    _userSession.CurrentUser = user;

                    // 1. Show the main UI
                    var mainVM = App.Services.GetRequiredService<MainViewModel>();
                    mainVM.IsLoggedIn = true;

                    // 2. Find the MainWindow to switch frames
                    if (((App)App.Current).m_window is MainWindow mainWindow)
                    {
                        // Move the navigation service from the "Full Window" frame 
                        // to the "Middle Section" frame
                        _navigationService.Initialize(mainWindow.ContentFrame);
                        _navigationService.NavigateTo(typeof(Views.Pages.FeedView));

                        // Hide the login frame entirely
                        mainWindow.LoginFrame.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex) { ErrorMessage = ex.Message; }
        }
    }
}