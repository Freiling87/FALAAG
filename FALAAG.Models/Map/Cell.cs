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
        // Ceiling, North, and East walls are stored in neighboring cells
        // I hate this solution but it seems like the most elegant
        public Wall WallBelow { get; set; }
        public Wall WallSouth { get; set; }
        public Wall WallWest { get; set; }
        public Cell CellAbove { get; set; }
        public Cell CellEast { get; set; }
        public Cell CellNorth { get; set; }
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

        public Cell Clone() =>
            new Cell(X, Y, Z, Name, Description, ImagePath);

        public void AddWall(Wall wall, Direction direction)
		{
            switch (direction)
			{
                case Direction.Above:
                    wall.Cell = CellAbove;
                    wall.Cell.WallBelow = wall;
                    break;
                case Direction.Below:
                    wall.Cell = this;
                    WallBelow = wall;
                    break;
                case Direction.East:
                    wall.Cell = CellEast;
                    wall.Cell.WallWest = wall;
                    break;
                case Direction.North:
                    wall.Cell = CellNorth;
                    wall.Cell.WallSouth = wall;
                    break;
                case Direction.South:
                    wall.Cell = this;
                    WallSouth = wall;
                    break;
                case Direction.West:
                    wall.Cell = this;
                    WallWest = wall;
                    break;
			}
		}

        public Wall GetWall(Direction direction) =>
            direction switch
            {
                Direction.Above => CellAbove.WallBelow,
                Direction.Below => WallBelow,
                Direction.East => CellEast.WallWest,
                Direction.North => CellNorth.WallSouth,
                Direction.South => WallSouth,
                Direction.West => WallWest,
            };

        public List<Wall> Walls() =>
            new List<Wall>()
            {
                CellAbove.WallBelow,
                WallBelow,
                CellEast.WallWest,
                CellNorth.WallSouth,
                WallSouth,
                WallWest,
            };

        public void UpdateWalls()
		{

		}

	}
}