using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FALAAG.Models
{
	public class Cell : PhysicalObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public string NarrationEntry { get; }
        // Need to set in XML reader
        [JsonIgnore]
        public string ImagePath { get; }

        // You'd think this should be in Entity, but ultmately this will just be the sum of all doable things in the Cell. Anyone can attempt them, why not?
        public List<ActionOption> ActionOptions = new List<ActionOption>();
        [JsonIgnore]
        public Automat AutomatHere { get; set; } // Temp
        [JsonIgnore]
        public List<NPCEncounter> Encounters { get; set; } = new List<NPCEncounter>();
        [JsonIgnore]
        public string EntryDescription { get; set; }
        public List<Automat> Automats { get; set; } = new List<Automat>();
        public List<Feature> Features { get; set; } = new List<Feature>();
        public List<Wall> Walls { get; set; } = new List<Wall>();
        public List<NPC> NPCs { get; set; } = new List<NPC>();
        public List<Feature> PhysicalFeatures { get; set; } = new List<Feature>();
        [JsonIgnore]
        public List<Job> JobsHere { get; set; } = new List<Job>();
        public List<Item> Items { get; set; } = new List<Item>();

		public Cell(int x, int y, int z, string name, string description, string imagePath)
        {
            X = x;
            Y = y;
            Z = z;
            Name = name;
            Description = description;
            ImagePath = imagePath;
        }

        public void AddNPC(string npcID, int chanceOfEncountering)
        {
            if (Encounters.Exists(m => m.NpcID == npcID))
                Encounters.First(m => m.NpcID == npcID).ChanceOfEncountering = chanceOfEncountering;
            else
                Encounters.Add(new NPCEncounter(npcID, chanceOfEncountering));
        }

        public void GetWall(Direction direction) =>
            Walls.Where(g => g.Direction == direction).FirstOrDefault();

        public Cell Clone() =>
            new Cell(X, Y, Z, Name, Description, ImagePath);
	}
}