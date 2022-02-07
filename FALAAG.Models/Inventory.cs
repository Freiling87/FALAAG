using System.Collections.Generic;
using System.Linq;
using FALAAG.Models.Shared;
using Newtonsoft.Json;

namespace FALAAG.Models
{
    // This class is modeled after functional programming principles. 
    // See https://soscsrpg.com/build-a-c-wpf-rpg/lesson-15-3-building-a-functional-inventory-class/ for more info.
    public class Inventory
    {
        #region Backing variables
        private readonly List<Item> _backingInventory = new List<Item>();
        private readonly List<GroupedInventoryItem> _backingGroupedInventoryItems = new List<GroupedInventoryItem>();
        #endregion
        #region Properties
        public IReadOnlyList<Item> Items => 
            _backingInventory.AsReadOnly();

        [JsonIgnore]
        public IReadOnlyList<GroupedInventoryItem> GroupedInventory =>
            _backingGroupedInventoryItems.AsReadOnly();

        [JsonIgnore]
        public IReadOnlyList<Item> Weapons =>
            _backingInventory.ItemsThatAre(Item.ItemCategory.Weapon).AsReadOnly();

        [JsonIgnore]
        public IReadOnlyList<Item> Consumables =>
            _backingInventory.ItemsThatAre(Item.ItemCategory.Consumable).AsReadOnly();

        [JsonIgnore]
        public bool HasConsumable => 
            Consumables.Any();
        #endregion
        #region Constructors
        public Inventory(IEnumerable<Item> items = null)
        {
            if (items == null)
                return;

            foreach (Item item in items)
            {
                _backingInventory.Add(item);
                AddItemToGroupedInventory(item);
            }
        }
        #endregion
        #region Public functions
        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items) =>
            items.All(reqItem => Items.Count(i => i.ID == reqItem.ID) >= reqItem.Quantity);
        #endregion
        #region Private functions
        // REFACTOR: Look for a better way to do this (extension method?)
        private void AddItemToGroupedInventory(Item item)
        {
            if (item.unstackable)
                _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 1));
            else
            {
                if (_backingGroupedInventoryItems.All(gi => gi.Item.ID != item.ID))
                    _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 0));

                _backingGroupedInventoryItems.First(gi => gi.Item.ID == item.ID).Quantity++;
            }
        }

        #endregion
        public Inventory AddItem(Item item) =>
            AddItems(new List<Item> { item });
        public Inventory AddItems(IEnumerable<Item> items) =>
            new Inventory(Items.Concat(items));
        public Inventory RemoveItem(Item item) =>
            RemoveItems(new List<Item> { item });
        public Inventory RemoveItems(IEnumerable<Item> items)
        {
            // REFACTOR: Look for a cleaner solution, with fewer temporary variables.
            List<Item> workingInventory = Items.ToList();
            IEnumerable<Item> itemsToRemove = items.ToList();

            foreach (Item item in itemsToRemove)
                workingInventory.Remove(item);

            return new Inventory(workingInventory);
        }
        public Inventory RemoveItems(IEnumerable<ItemQuantity> itemQuantities)
        {
            // REFACTOR
            Inventory workingInventory = new Inventory(Items);

            foreach (ItemQuantity itemQuantity in itemQuantities)
                for (int i = 0; i < itemQuantity.Quantity; i++)
                    workingInventory = workingInventory.RemoveItem(workingInventory.Items.First(item => item.ID == itemQuantity.ID));

            return workingInventory;
        }
    }
}