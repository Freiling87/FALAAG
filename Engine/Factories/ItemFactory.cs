using Engine.Models;
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Engine.Actions;
using Engine.Shared;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private const string _contentDataFilePath = ".\\GameData\\Items.xml";
        private static readonly List<Item> _gameItemTemplates = new List<Item>();

        static ItemFactory()
        {
            if (File.Exists(_contentDataFilePath))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(_contentDataFilePath));

                // XML tags work as a filepath here
                LoadItemsFromNodes(data.SelectNodes("/Items/Weapons/Weapon"));
                LoadItemsFromNodes(data.SelectNodes("/Items/HealingItems/HealingItem"));
                LoadItemsFromNodes(data.SelectNodes("/Items/MiscellaneousItems/MiscellaneousItem"));
                LoadItemsFromNodes(data.SelectNodes("/Items/WearableItems/WearableItem"));
            }
            else
                throw new FileNotFoundException($"Missing data file: {_contentDataFilePath}");
        }

        public static Item CreateItem(string itemTypeID) =>
            _gameItemTemplates.FirstOrDefault(item => item.ID == itemTypeID)?.Clone();
        private static Item.ItemCategory DetermineItemCategory(string itemType)
        {
            switch (itemType)
            {
                case "Weapon":
                    return Item.ItemCategory.Weapon;
                case "HealingItem":
                    return Item.ItemCategory.Consumable;
                default:
                    return Item.ItemCategory.Miscellaneous;
            }
        }
        public static string ItemName(string itemTypeID) =>
            // ?. is null conditional: If FirstOrDefault() does not return a null, retrieve Name.
            // ?? is null-coalescing: If Name ends up null, then output "".
            _gameItemTemplates.FirstOrDefault(i => i.ID == itemTypeID)?.Name ?? "";
        private static void LoadItemsFromNodes(XmlNodeList nodes)
        {
            if (nodes == null)
                return;

            foreach (XmlNode node in nodes)
            {
                Item.ItemCategory itemCategory = DetermineItemCategory(node.Name);

                Item gameItem = new Item(itemCategory,
                    node.AttributeAsString("ID"),
                    node.AttributeAsString("Name"),
                    node.AttributeAsInt("Value"),
                    itemCategory == Item.ItemCategory.Weapon);

                if (itemCategory == Item.ItemCategory.Weapon)
                    gameItem.Action = new AttackWithWeapon(gameItem, node.AttributeAsString("DamageDice"));
                else if (itemCategory == Item.ItemCategory.Consumable)
                    gameItem.Action = new Heal(gameItem,
                        node.AttributeAsInt("HitPointsToHeal"));

                _gameItemTemplates.Add(gameItem);
            }
        }
    }
}