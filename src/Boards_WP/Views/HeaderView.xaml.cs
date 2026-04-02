using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;

using Boards_WP.ViewModels;
using Boards_WP.Data.Models;

namespace Boards_WP.Views
{
    public sealed partial class HeaderView : UserControl
    {
        public HeaderViewModel ViewModel { get; private set; }

        public HeaderView()
        {
            this.InitializeComponent();

            this.Loaded += HeaderView_Loaded;
        }

        private void HeaderView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current is App myApp)
            {
                this.ViewModel = App.GetService<HeaderViewModel>();
                this.DataContext = this.ViewModel;

                this.Bindings.Update();
            }
        }

        private void CommunitySearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                HandleSecretTrigger(sender);
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
            if (e.ClickedItem is Community selectedCommunity && ViewModel != null)
            {
                ViewModel.SelectCommunityCommand.Execute(selectedCommunity);

                NavigateToPage(typeof(Pages.CommunityView), selectedCommunity);
                ResultsPopup.IsOpen = false;
            }
        }

        public Visibility GetVisibility(bool showText)
        {
            // Restore the logic from 'main' - This is a UI converter
            return showText ? Visibility.Visible : Visibility.Collapsed;
        }

        // Filip's logic belongs in a separate handler called by the SearchBox
        private void HandleSecretTrigger(AutoSuggestBox sender)
        {
            string query = sender.Text.ToLower().Trim();
            
            // SECRET TRIGGER: /weaponizedpenguins
            if (query == "/weaponizedpenguins")
            {
                ResultsPopup.IsOpen = false;
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
