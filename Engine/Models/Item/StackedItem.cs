using Engine.Services;
using System.ComponentModel;

namespace Engine.Models
{
    public class GroupedInventoryItem : INotifyPropertyChanged
    {
		#region Header
		private Item _item;
        private int _quantity;

        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public GroupedInventoryItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

		#endregion
	}
}