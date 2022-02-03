using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class ItemQuantity
    {
        public string ID { get; }
        public int Quantity { get; }
        public string QuantityItemDescription => $"{Quantity} {ItemFactory.ItemName(ID)}";

        public ItemQuantity(string itemID, int quantity)
        {
            ID = itemID;
            Quantity = quantity;
        }
    }
}