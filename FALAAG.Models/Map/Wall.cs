using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class Wall : PhysicalObject
	{
		public Material Material { get; set; }
		public List<Portal> Portals { get; set; } = new List<Portal>();
		public List<ActionOption> ActionOptions = new List<ActionOption>();
		public bool Passable { get; set; }
		public Cell Cell { get; set; }

		public Wall (string id, string name)
		{
			// TODO: Prefer most-solid material when differing types are adjoining.
			// TODO: Use IsTemplate bool to prohibit assignment of templates as instances.
			//	e.g., if a wood and brick wall adjoin, prefer the brick. 
			ID = id;
			Name = name;
		}

		public Wall Clone() =>
			new (ID, Name);

		public Direction GetDirection()
		{
			if (this == Cell.CellAbove.WallBelow)
				return Direction.Above;
			else if (this == Cell.WallBelow)
				return Direction.Below;
			else if (this == Cell.CellEast.WallWest)
				return Direction.East;
			else if (this == Cell.CellNorth.WallSouth)
				return Direction.North;
			else if (this == Cell.WallSouth)
				return Direction.South;
			else if (this == Cell.WallWest)
				return Direction.West;
			else
				return Direction.Null;
		}
	}
}
