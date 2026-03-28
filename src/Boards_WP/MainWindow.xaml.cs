using Microsoft.UI.Xaml;

using System;

namespace Boards_WP
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // 1. Tell the App class about this window so we can find it later
            if (App.Current is App myApp)
            {
                myApp.m_window = this;
            }

            // 2. Load the Feed by default when the app opens
            // Make sure the namespace matches where you put FeedView
            ContentFrame.Navigate(typeof(Views.Pages.FeedView));
        }

        // 3. A public helper so other pages can trigger navigation easily
        public void NavigateToPage(Type pageType, object parameter = null)
        {
            ContentFrame.Navigate(pageType, parameter);
        }
    }
}