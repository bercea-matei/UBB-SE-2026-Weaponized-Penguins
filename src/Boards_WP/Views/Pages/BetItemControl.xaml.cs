using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Windows.Input;

using Boards_WP.Data.Models;

namespace Boards_WP.Views
{
    public sealed partial class BetItemControl : UserControl
    {
       
        public static readonly DependencyProperty BetDataProperty =
            DependencyProperty.Register(nameof(BetData), typeof(Bet), typeof(BetItemControl), new PropertyMetadata(null));

        public Bet BetData
        {
            get => (Bet)GetValue(BetDataProperty);
            set => SetValue(BetDataProperty, value);
        }

        
        public static readonly DependencyProperty BetCommandProperty =
            DependencyProperty.Register(nameof(BetCommand), typeof(ICommand), typeof(BetItemControl), new PropertyMetadata(null));

        public ICommand BetCommand
        {
            get => (ICommand)GetValue(BetCommandProperty);
            set => SetValue(BetCommandProperty, value);
        }

        public BetItemControl()
        {
            this.InitializeComponent();
        }

       
        private void ShowBetPanel_Click(object sender, RoutedEventArgs e)
        {
            BetInputPanel.Visibility = Visibility.Visible;
            TokenInput.Focus(FocusState.Programmatic);
        }

        
        private void HideBetPanel_Click(object sender, RoutedEventArgs e)
        {
            BetInputPanel.Visibility = Visibility.Collapsed;
            TokenInput.Text = string.Empty;
        }
    }
}