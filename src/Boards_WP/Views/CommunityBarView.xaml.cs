using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;

namespace Boards_WP.Views
{
    public sealed partial class CommunityBarView : UserControl
    {
<<<<<<< HEAD
        // the communities names list we display in the navigation sidebar
=======
>>>>>>> main
        public ObservableCollection<string> CommunityNames { get; set; } = new();

        public CommunityBarView()
        {
            this.InitializeComponent();

            CommunityListView.ItemsSource = CommunityNames;

            CommunityNames.Add("ComputerScience");
            CommunityNames.Add("UBB");
            CommunityNames.Add("UI_Design_Daily");
            CommunityNames.Add("Weaponized_Penguins");
            CommunityNames.Add("ClujStudents");
        }
    }
}