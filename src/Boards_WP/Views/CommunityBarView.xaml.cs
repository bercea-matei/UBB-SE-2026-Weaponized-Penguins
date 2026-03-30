using System.Collections.ObjectModel;

using Boards_WP.Data.Models; 
using Boards_WP.Views.Pages;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Boards_WP.Views
{
    public sealed partial class CommunityBarView : UserControl
    {
        public ObservableCollection<Community> Communities { get; set; } = new();

        public CommunityBarView()
        {
            this.InitializeComponent();
            CommunityListView.ItemsSource = Communities;

<<<<<<< HEAD
            Communities.Add(new Community { Name = "ComputerScience", Description = "Tech and coding.", Admin = new User { Username = "System" } });
            Communities.Add(new Community { Name = "UBB", Description = "Universitatea Babeș-Bolyai.", Admin = new User { Username = "System" } });
            Communities.Add(new Community { Name = "Weaponized_Penguins", Description = "Project team HQ.", Admin = new User { Username = "System" } });
        }

        private void CommunityListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Community selected)
            {
                // Try to get the frame from the XamlRoot (the actual window the control is in)
                if (this.XamlRoot.Content is Frame rootFrame)
                {
                    rootFrame.Navigate(typeof(CommunityView), selected);
                }
                // If the Window.Content is a ShellPage (UserControl), we need to find the Frame inside it
                else if (this.XamlRoot.Content is UIElement shell)
                {
                    // This looks specifically for a Frame named 'ContentFrame' in your ShellPage
                    // Replace 'ContentFrame' with the x:Name you gave your Frame in ShellPage.xaml
                    var frame = FindChildFrame(shell, "ContentFrame");
                    frame?.Navigate(typeof(CommunityView), selected);
=======
            Communities.Add(new Community { Name = "ComputerScience", Description = "Tech and coding" });
            Communities.Add(new Community { Name = "UBB", Description = "Universitatea Babeș-Bolyai" });
            Communities.Add(new Community { Name = "Weaponized_Penguins", Description = "Project team" });
        }
        


        // this function identifies which community the user clicked on

        // we need to find a way in which the CommunitySidebarView can communicate with the main Frame to change content
        // according to the selected community

        // but they are isolated, so we use XamlRoot and VisualTreeHelper to break out of isolation
        private void CommunityListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Community selected)
            {
                // searching for the Frame (middle section) directly
                if (this.XamlRoot.Content is Frame rootFrame)
                {
                    rootFrame.Navigate(typeof(CommunityView), selected);
                }

                // if the Frame is hidden, we need to search for it on a deeper level
                else if (this.XamlRoot.Content is UIElement shell)
                {
                    var frame = FindChildFrame(shell, "ContentFrame");
                    frame?.Navigate(typeof(CommunityView), selected);
                }
            }
        }

        // A more powerful helper to find the frame by name (looking on deeper layers)
        private Frame FindChildFrame(DependencyObject parent, string frameName)
        {
            int count = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is Frame f && f.Name == frameName) return f;

                var result = FindChildFrame(child, frameName);
                if (result != null) return result;
            }
            return null;
        }

        // function for the Home button
        private void HomeNavigation_ItemClick(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (this.XamlRoot.Content is UIElement shell)
            {
                var rootFrame = FindChildFrame(shell, "ContentFrame");

                if (rootFrame != null)
                {
                    rootFrame.Navigate(typeof(FeedView));
>>>>>>> filip/community-view
                }
            }
        }

<<<<<<< HEAD
        // A more powerful helper to find the frame by name
        private Frame FindChildFrame(DependencyObject parent, string frameName)
        {
            int count = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is Frame f && f.Name == frameName) return f;

                var result = FindChildFrame(child, frameName);
                if (result != null) return result;
            }
            return null;
=======
        // function for the Discover button
        private void DiscoverNavigation_ItemClick(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (this.XamlRoot.Content is UIElement shell)
            {
                var rootFrame = FindChildFrame(shell, "ContentFrame");

                if (rootFrame != null)
                {
                    rootFrame.Navigate(typeof(FeedView));
                }
            }
>>>>>>> filip/community-view
        }
    }
}