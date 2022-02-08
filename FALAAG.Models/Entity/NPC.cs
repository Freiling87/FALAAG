using System.Collections.Generic;

namespace FALAAG.Models
{
    public class NPC : Entity
    {
        public string ImagePath { get; }
        public List<ItemPercentage> LootTable { get; } = new List<ItemPercentage>();
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
            LootTable.RemoveAll(ip => ip.ID == id);
            LootTable.Add(new ItemPercentage(id, percentage));
        }

        public NPC Clone()
        {
            NPC newNPC = new NPC(ID, NameActual, NameGeneral, ImagePath, HpMax, Attributes, CurrentWeapon, XpReward, Cash);
            newNPC.LootTable.AddRange(LootTable);
            return newNPC;
        }
    }
}