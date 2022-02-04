using Engine.Services;
using System.ComponentModel;

namespace Engine.Models
{
    public class GroupedInventoryItem : INotifyPropertyChanged
    {
        #region Header
        public event PropertyChangedEventHandler PropertyChanged;

        public Item Item { get; set; }
		public int Quantity { get; set; }

        public GroupedInventoryItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

		#endregion
	}
}