namespace FALAAG.Models
{
    public class ItemQuantity
    {
        private readonly Item _item;

        public string ID => _item.ID;
        public int Quantity { get; }

        public string QuantityItemDescription =>
            $"{Quantity} {_item.Name}";

        public ItemQuantity(Item item, int quantity)
        {
            _item = item;
            Quantity = quantity;
        }
    }
}