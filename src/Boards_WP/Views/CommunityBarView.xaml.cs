using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;

namespace Boards_WP.Views
{
    public sealed partial class CommunityBarView : UserControl
    {
        public ObservableCollection<string> CommunityNames { get; set; } = new();

        public CommunityBarView()
        {
            this.InitializeComponent();

            CommunityListView.ItemsSource = CommunityNames;

            CommunityNames.Add("p/PenguinHype");
            CommunityNames.Add("p/DotNetMastery");
            CommunityNames.Add("p/UI_Design_Daily");
            CommunityNames.Add("p/MechanicalKeyboards");
            CommunityNames.Add("p/GamingUniverse");
        }
    }
}