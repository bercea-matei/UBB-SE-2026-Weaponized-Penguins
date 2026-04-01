using System.Collections.ObjectModel;

using Boards_WP.Data.Models; 
using Boards_WP.Views.Pages;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Boards_WP.Views
{
    public sealed partial class CommunityBarView : UserControl
    {
        public ObservableCollection<Community> Communities { get; set; } = new();

        public CommunityBarView()
        {
            //to do - load communities from service
            this.InitializeComponent();
            CommunityListView.ItemsSource = Communities;

            Communities.Add(new Community { Name = "ComputerScience", Description = "Tech and coding.", Admin = new User { Username = "System" } });
            Communities.Add(new Community { Name = "UBB", Description = "Universitatea Babeș-Bolyai.", Admin = new User { Username = "System" } });
            Communities.Add(new Community { Name = "Weaponized_Penguins", Description = "Project team HQ.", Admin = new User { Username = "System" } });
        }

        private void CommunityListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Community selected)
            { 
                if (this.XamlRoot.Content is Frame rootFrame)
                {
                    rootFrame.Navigate(typeof(CommunityView), selected);
                }
                
                else if (this.XamlRoot.Content is UIElement shell)
                {

                    var frame = FindChildFrame(shell, "ContentFrame");
                    frame?.Navigate(typeof(CommunityView), selected);
                }
            }
        }

        
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

        private void HomeNavigation_ItemClick(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (this.XamlRoot.Content is UIElement shell)
            {
                var rootFrame = FindChildFrame(shell, "ContentFrame");

                if (rootFrame != null)
                {
                    var feedVm = App.Current.Services.GetRequiredService<ViewModels.FeedViewModel>();

                   
                    feedVm.IsHome = true;
                    feedVm.LoadHome();
                    rootFrame.Navigate(typeof(FeedView));
                }
            }
        }

        
        private void DiscoverNavigation_ItemClick(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (this.XamlRoot.Content is UIElement shell)
            {
                var rootFrame = FindChildFrame(shell, "ContentFrame");

                if (rootFrame != null)
                {
                    var feedVm = App.Current.Services.GetRequiredService<ViewModels.FeedViewModel>();

                    
                    feedVm.IsHome = false;
                    feedVm.LoadHome();
                    rootFrame.Navigate(typeof(FeedView));
                }
            }
        }

        private void StartCommunity_ItemClick(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (this.XamlRoot.Content is UIElement shell)
            {
                var rootFrame = FindChildFrame(shell, "ContentFrame");
                if (rootFrame != null)
                {
                    
                    rootFrame.Navigate(typeof(CreateCommunityView), Communities);
                }
            }
        }
    }
}