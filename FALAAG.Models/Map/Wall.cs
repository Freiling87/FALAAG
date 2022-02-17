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

		public Direction GetDirection(Cell context)
		{
			if (this == context.CellAbove.WallBelow)
				return Direction.Above;
			else if (this == context.WallBelow)
				return Direction.Below;
			else if (this == context.CellEast.WallWest)
				return Direction.East;
			else if (this == context.CellNorth.WallSouth)
				return Direction.North;
			else if (this == context.WallSouth)
				return Direction.South;
			else if (this == context.WallWest)
				return Direction.West;
			else
				return Direction.Null;
		}
	}
}
