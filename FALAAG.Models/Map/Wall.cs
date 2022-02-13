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

		public Wall (string id, string name)
		{
			// TODO: Reroute all access to this to the WallFactory.
			ID = id;
			Name = name;
		}

		public Wall Clone() =>
			new Wall(ID, Name);
	}
}
