using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;

using Boards_WP.Views.Pages;

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

            ContentFrame.Navigate(typeof(FeedView));
        }

        public void NavigateToPage(Type pageType, object parameter = null)
        {
            ContentFrame.Navigate(pageType, parameter);
        }
    }
}