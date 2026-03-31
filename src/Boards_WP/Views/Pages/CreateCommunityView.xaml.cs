using Boards_WP.Data.Models;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using System.Collections.ObjectModel;

namespace Boards_WP.Views.Pages
{
    public sealed partial class CreateCommunityView : Page
    {
        private ObservableCollection<Community> _sidebarList;

        public CreateCommunityView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (e.Parameter is ObservableCollection<Community> list)
            {
                _sidebarList = list;
            }
        }

        private void CreateCommunity_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameInput.Text)) return;

            var newCommunity = new Community
            {
                CommunityID = 99,
                Name = NameInput.Text,
                Description = DescriptionInput.Text,
                Admin = new User { Username = "@Me" }, 
                MembersNumber = 1
            };

            _sidebarList?.Add(newCommunity);
            this.Frame.Navigate(typeof(CommunityView), newCommunity);
        }
    }
}