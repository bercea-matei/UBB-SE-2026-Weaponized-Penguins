using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace Boards_WP
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            if (App.Current is App myApp)
            {
                myApp.m_window = this;
            }

            var navigationService = App.Services.GetRequiredService<INavigationService>();
            navigationService.Initialize(ContentFrame);
            navigationService.NavigateTo(typeof(Views.Pages.FeedView));
        }


        // pageType: can be any page (FeedPage, FullViewPage) and represents the next page the frame needs to load
        // parameter: is the data that needs to be displayed on this new page (and can be optional, which why we use "null")
        public void NavigateToPage(Type pageType, object parameter = null)
        {
            ContentFrame.Navigate(pageType, parameter);
        }
    }
}