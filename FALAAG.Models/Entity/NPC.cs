using System.Collections.Generic;

namespace FALAAG.Models
{
    public class NPC : Entity
    {
        public string ImagePath { get; }
        public List<ItemPercentage> LootTable { get; } = new List<ItemPercentage>();
        public int XpReward { get; }
		public int Importance { get; set; }
        public bool IsHostileToPlayer { get; set; } = false;

		public NPC(string id, string nameActual, string nameGeneral, string imagePath,
                       IEnumerable<EntityAttribute> attributes,
                       IEnumerable<Skill> skills,
                       Item currentWeapon,
                       int xpReward) :
            base(id, nameActual, nameGeneral, attributes, new List<Skill>())
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
            NPC newNPC = new NPC(ID, NameActual, NameGeneral, ImagePath, Attributes, new List<Skill>(), CurrentWeapon, XpReward);
            newNPC.LootTable.AddRange(LootTable);
            return newNPC;
        }
	}
}