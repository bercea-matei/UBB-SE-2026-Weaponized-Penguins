using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Boards_WP.Views.Pages // Matches the x:Class above
{
    public sealed partial class BetsView : Page
    {
        public ObservableCollection<Bet> TestBets { get; set; } = new();

        public BetsView()
        {
            this.InitializeComponent(); // This should now turn blue/work

            // Add your sample data here for testing
            TestBets.Add(new Bet
            {
                Expression = "Weaponized Penguins",
                BetCommunity = new Community { Name = "UBB" },
                StartingTime = DateTime.Now,
                EndingTime = DateTime.Now.AddDays(1)
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = this;
        }
    }
}