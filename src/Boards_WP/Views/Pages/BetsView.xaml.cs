using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Boards_WP.Data.Models;
using System;

namespace Boards_WP.Views.Pages
{
    public sealed partial class BetsView : Page
    {
        public ObservableCollection<Bet> TestBets { get; set; } = new();

        public BetsView()
        {
            this.InitializeComponent();

            // Example 1: Post
            TestBets.Add(new Bet
            {
                Expression = "Weaponized Penguins",
                BetCommunity = new Community { Name = "UBB" },
                Type = BetType.Post,
                StartingTime = DateTime.Now,
                EndingTime = DateTime.Now.AddDays(2),
                YesAmount = 450,
                NoAmount = 120
            });

            // Example 2: Comment
            TestBets.Add(new Bet
            {
                Expression = "Compiler Error",
                BetCommunity = new Community { Name = "Programming" },
                Type = BetType.Comment,
                StartingTime = DateTime.Now.AddHours(-1),
                EndingTime = DateTime.Now.AddHours(5),
                YesAmount = 20,
                NoAmount = 1000
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = this;
        }
    }
}