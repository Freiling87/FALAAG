using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FALAAG.Models
{
	public class Cell : PhysicalObject
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public string NarrationEntry { get; }
        // Need to set in XML reader
        [JsonIgnore]
        public string ImagePath { get; }

        [JsonIgnore]
        public Automat AutomatHere { get; set; } // Temp
        [JsonIgnore]
        public List<NPCEncounter> Encounters { get; set; } = new List<NPCEncounter>();
        [JsonIgnore]
        public string EntryDescription { get; set; }
        public List<Automat> Automats { get; set; } = new List<Automat>();
        public List<Feature> Features { get; set; } = new List<Feature>();
        public List<Gate> Gates { get; set; } = new List<Gate>();
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
	}
}