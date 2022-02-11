using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class Gate : PhysicalObject
	{
		public Direction Direction { get; set; }
		public string ID { get; set; }
		public Material Material { get; set; }
		public Cell Cell { get; set; }

		public Gate (string id, string name)
		{
			// TODO: Reroute all access to this to the GateFactory.
			ID = id;
			Name = name;
		}

		public Gate Clone() =>
			new Gate(ID, Name);
	}
}
