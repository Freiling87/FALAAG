using Engine.Services;

namespace Engine.Models
{
    public class GroupedInventoryItem : BaseNotificationClass
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
                OnPropertyChanged();
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public GroupedInventoryItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
		#endregion
	}
}