using System.Collections.Generic;
using Engine.Factories;
using Engine.Services;

namespace Engine.Models
{
    public class NPC : Entity
    {
        private readonly List<ItemPercentage> _lootTable =
            new List<ItemPercentage>();

        public string ID { get; }
        public string ImagePath { get; }
        public int XpReward { get; }

        public NPC(string id, string nameActual, string nameGeneral, string imagePath,
                       int hpMax, IEnumerable<EntityAttribute> attributes,
                       Item currentWeapon,
                       int xpReward, int cash) :
            base(id, nameActual, nameGeneral, hpMax, hpMax, attributes, cash)
        {
            ID = id;
            ImagePath = imagePath;
            CurrentWeapon = currentWeapon;
            XpReward = xpReward;
        }

        public void AddItemToLootTable(string id, int percentage)
        {
            _lootTable.RemoveAll(ip => ip.ID == id);
            _lootTable.Add(new ItemPercentage(id, percentage));
        }

        public NPC GetNewInstance()
        {
            NPC newNPC =
                new NPC(ID, NameActual, NameGeneral, ImagePath, HpMax, Attributes,
                            CurrentWeapon, XpReward, Cash);

            foreach (ItemPercentage itemPercentage in _lootTable)
            {
                newNPC.AddItemToLootTable(itemPercentage.ID, itemPercentage.Percentage);

                if (DiceService.Instance.Roll(100).Value <= itemPercentage.Percentage)
                    newNPC.InventoryAddItem(ItemFactory.CreateItem(itemPercentage.ID));
            }

            return newNPC;
        }
    }
}