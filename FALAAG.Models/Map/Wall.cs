using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class Wall : PhysicalObject
	{
		public Direction Direction { get; set; }
		public string ID { get; set; }
		public Material Material { get; set; }
		public Cell Cell { get; set; }
		public Portal Portal { get; set; }

		public Wall (string id, string name)
		{
			// TODO: Reroute all access to this constructor to the WallFactory.
			// TODO: Prefer most-solid material when differing types are adjoining.
			//	e.g., if a wood and brick wall adjoin, prefer the brick. 
			ID = id;
			Name = name;
		}

		public Wall Clone() =>
			new Wall(ID, Name);
	}
}
