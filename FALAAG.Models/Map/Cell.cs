using Newtonsoft.Json;
using System;
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
        public Wall WallBelow { get; set; }
        public Wall WallSouth { get; set; }
        public Wall WallWest { get; set; }
        public Cell CellAbove { get; set; }
        public Cell CellBelow { get; set; }
        public Cell CellEast { get; set; }
        public Cell CellNorth { get; set; }
        public Cell CellSouth { get; set; }
        public Cell CellWest { get; set; }
        public List<NPC> NPCs { get; set; } = new List<NPC>();
        public List<Feature> PhysicalFeatures { get; set; } = new List<Feature>();
        [JsonIgnore]
        public List<Job> JobsHere { get; set; } = new List<Job>();
        public List<Item> Items { get; set; } = new List<Item>();
        public bool IsMapEdge { get; set; }
        public List<Wall> Walls
        {
			get
            {
                List<Wall> list = new()
                {
                    CellAbove.WallBelow,
                    WallBelow,
                    CellEast.WallWest,
                    CellNorth.WallSouth,
                    WallSouth,
                    WallWest,
                };

                return list.Where(w => w != null).ToList();
            }
        }

        public Cell(int x, int y, int z, string name, string description, string imagePath)
        {
            X = x;
            Y = y;
            Z = z;
            Name = name;
            Description = description;
            ImagePath = imagePath;
            IsMapEdge = false;
        }

        public Cell(int x, int y, int z, bool EdgeCell = false)
        {
            if (EdgeCell == false)
                throw new ArgumentException("Unintended use of this method");

            X = x;
            Y = y;
            Z = z;
            Name = "Edge";
            Description = "This is the edge of the map. If you're standing here, it's a bug.";
            // Can vary these later with world-justified edges, e.g. desert, arco wall, etc.
            IsMapEdge = true;
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
            // TODO: This could all be greatly simplified by creating getters and setters for the variables defined. 

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



	}
}