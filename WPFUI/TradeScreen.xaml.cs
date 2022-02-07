using System.Windows;
using FALAAG.Models;
using FALAAG.ViewModels;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for TradeScreen.xaml
    /// </summary>
    public partial class TradeScreen : Window
    {
        public GameSession Session => DataContext as GameSession;

        public TradeScreen()
        {
            InitializeComponent();
        }

        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                Session.Player.CashReceive(groupedInventoryItem.Item.SellPriceSimple);
                Session.CurrentAutomat.InventoryAddItem(groupedInventoryItem.Item);
                Session.Player.InventoryRemoveItem(groupedInventoryItem.Item);
            }
        }

        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem =
                ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                if (Session.Player.Cash >= groupedInventoryItem.Item.Value)
                {
                    Session.Player.CashSpend(groupedInventoryItem.Item.Value);
                    Session.CurrentAutomat.InventoryRemoveItem(groupedInventoryItem.Item);
                    Session.Player.InventoryAddItem(groupedInventoryItem.Item);
                }
                else
                    MessageBox.Show("You can't afford that.");
            }
        }

        private void OnClick_Close(object sender, RoutedEventArgs e) =>
            Close();
    }
}