using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Boards_WP.Data.Models;

namespace Boards_WP.Views
{
    public sealed partial class HeaderView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private int _userTokens = 1250;
        public int UserTokens
        {
            get => _userTokens;
            set { if (_userTokens != value) { _userTokens = value; OnPropertyChanged(); } }
        }

        private List<string> _allCommunities = new List<string>
        {
            "Gaming", "Programming", "Art", "Music", "Computer Science", "UBB", "Weaponized Penguins"
        };

        public ObservableCollection<string> FilteredResults { get; set; } = new();

        public HeaderView()
        {
            this.InitializeComponent();
            this.DataContext = this;
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
            if (e.ClickedItem is string selectedName)
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

            if (query == "/weaponizedpenguins")
            {
                TokenDisplay.Visibility = Visibility.Visible;
                sender.Text = string.Empty;

                UserTokens += 5;

                NavigateToPage(typeof(Pages.BetsView), null);
            }
        }

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