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
		public bool Passable { get; set; }
		public Cell Cell { get; set; }
		public List<ActionCommand> MovementActionCommands
		{
			get
			{
				List<ActionCommand> list = ActionCommands.ToList();

				foreach(Portal portal in Portals)
				{
					list.Concat(portal.ActionCommands);

					foreach (ObjectAttachment objectAttachment in portal.ObjectAttachments)
						list.Concat(objectAttachment.ActionCommands);
				}

				return list;
			}
		}

		public Wall (string id, string name)
		{
			// TODO: Prefer most-solid material when differing types are adjoining.
			// TODO: Use IsTemplate bool to prohibit assignment of templates as instances.
			//	e.g., if a wood and brick wall adjoin, prefer the brick. 
			ID = id;
			Name = name;
			Passable = false;
		}

		public Wall Clone()
		{
			Wall wall = new(ID, Name);
			wall.ActionCommands = this.ActionCommands;
			return wall;
		}

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
