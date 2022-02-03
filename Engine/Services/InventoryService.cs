using System.Collections.Generic;
using System.Linq;
using Engine.Factories;
using Engine.Models;

namespace Engine.Services
{
    // "You can see in the tests how the “functional” inventory is easy to test.
    // We don’t need to create a Player object, or any other infrastructure –
    // just so we can test one two classes. This is one of the cool things about functional programming."

    public static class InventoryService
    {
        public static Inventory AddItem(this Inventory inventory, Item item) =>
            inventory.AddItems(new List<Item> { item });

        public static Inventory AddItemFromFactory(this Inventory inventory, string ID) => 
            inventory.AddItems(new List<Item> { ItemFactory.CreateItem(ID) });

        public static Inventory AddItems(this Inventory inventory, IEnumerable<Item> items) =>
            new Inventory(inventory.Items.Concat(items));

        public static Inventory AddItems(this Inventory inventory, IEnumerable<ItemQuantity> itemQuantities)
        {
            List<Item> itemsToAdd = new List<Item>();

            foreach (ItemQuantity itemQuantity in itemQuantities)
                for (int i = 0; i < itemQuantity.Quantity; i++)
                    itemsToAdd.Add(ItemFactory.CreateItem(itemQuantity.ID));

            return inventory.AddItems(itemsToAdd);
        }

        public static Inventory RemoveItem(this Inventory inventory, Item item) =>
            inventory.RemoveItems(new List<Item> { item });

        public static Inventory RemoveItems(this Inventory inventory, IEnumerable<Item> items)
        {
            // REFACTOR: Look for a cleaner solution, with fewer temporary variables.
            List<Item> workingInventory = inventory.Items.ToList();
            IEnumerable<Item> itemsToRemove = items.ToList();

            foreach (Item item in itemsToRemove)
                workingInventory.Remove(item);

            return new Inventory(workingInventory);
        }

        public static Inventory RemoveItems(this Inventory inventory,
                                            IEnumerable<ItemQuantity> itemQuantities)
        {
            // REFACTOR
            Inventory workingInventory = inventory;

            foreach (ItemQuantity itemQuantity in itemQuantities)
                for (int i = 0; i < itemQuantity.Quantity; i++)
                    workingInventory = workingInventory.RemoveItem(workingInventory.Items.First(item => item.ID == itemQuantity.ID));

            return workingInventory;
        }

        public static List<Item> ItemsThatAre(this IEnumerable<Item> inventory, Item.ItemCategory category) =>
            inventory.Where(i => i.Category == category).ToList();
    }
}