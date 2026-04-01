using Microsoft.UI.Xaml.Controls;

using Boards_WP.Data.Models;

using System.Collections.Generic;
using System;
using Microsoft.UI.Xaml.Navigation;
using Boards_WP.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Boards_WP.Views.Pages
{
    
    
    public sealed partial class FeedView : Page
    {
        public FeedViewModel? ViewModel { get; set; }

        public FeedView()
        {

            ViewModel = App.Services?.GetService<FeedViewModel>();
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LoadFeed();
        }
    }
}