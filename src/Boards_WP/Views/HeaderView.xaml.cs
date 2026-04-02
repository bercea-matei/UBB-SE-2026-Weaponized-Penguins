using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Boards_WP.Data.Models;

namespace Boards_WP.Views
{
    public sealed partial class HeaderView : UserControl
    {
        private List<string> _allCommunities = new List<string>
        {
            "Gaming", "Programming", "Art", "Music", "Computer Science", "UBB", "Weaponized Penguins"
        };

        public ObservableCollection<string> FilteredResults { get; set; } = new();

        public HeaderView()
        {
            this.InitializeComponent();
            ResultsList.ItemsSource = FilteredResults;
        }

        private void CommunitySearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string query = sender.Text.ToLower();
                if (string.IsNullOrWhiteSpace(query))
                {
                    ResultsPopup.IsOpen = false;
                }
                else
                {
                    var matches = _allCommunities.Where(c => c.ToLower().Contains(query)).ToList();
                    FilteredResults.Clear();
                    foreach (var m in matches) FilteredResults.Add(m);
                    ResultsPopup.IsOpen = FilteredResults.Count > 0;
                }
            }
        }

        private void ResultsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedName = e.ClickedItem as string;
            if (selectedName != null)
            {
                ResultsPopup.IsOpen = false;
                CommunitySearchBox.Text = string.Empty;

                var selectedCommunity = new Community
                {
                    Name = selectedName,
                    Description = $"This is the official {selectedName} community.",
                    MembersNumber = 123,
                    Admin = new User { Username = "@System" }
                };

                NavigateToPage(typeof(Pages.CommunityView), selectedCommunity);
            }
        }

        private void CommunitySearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string query = sender.Text.ToLower().Trim();
            ResultsPopup.IsOpen = false;

            // SECRET TRIGGER: /weaponizedpenguins
            if (query == "/weaponizedpenguins")
            {
                TokenDisplay.Visibility = Visibility.Visible;
                sender.Text = string.Empty;
                NavigateToPage(typeof(Pages.BetsView), null);
            }
        }

        // Helper method to handle navigation logic safely
        private void NavigateToPage(Type pageType, object parameter)
        {
            if (App.Current is App myApp && myApp.m_window != null)
            {
                var rootFrame = myApp.m_window.Content as Frame;
                if (rootFrame == null && myApp.m_window.Content is FrameworkElement fe)
                {
                    rootFrame = fe.FindName("ContentFrame") as Frame;
                }

                if (rootFrame != null)
                {
                    rootFrame.Navigate(pageType, parameter);
                }
            }
        }
    }
}