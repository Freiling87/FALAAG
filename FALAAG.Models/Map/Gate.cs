using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class Gate : PhysicalObject
	{
		private bool _opacity;
		// 100: No visbility
		// 50: Reduce the Visibility Distance of all objects past it by 50%
		private bool _solidity;
		// 100: Solid earth, blocks sound completely
		// 25: Sheetrock wall, allows sensation of objects beyond at 75% of Audibility Distance 
		public Direction Direction;

		public Gate (Cell location, Direction direction, bool priority)
		{
			Direction = direction;

			// priority bool: Override adjoining gates. E.g., if you spawn a basement, it needs a basement door so you can't spawn a solid floor on the cell above.
			// Gates should have a View Distance, at which the player can identify them from afar. 
			// E.g., Alley Entrance should be visible from *very* far, whereas Basement Window maybe only a cell away.
		
			location.Gates.Add(this); // Might want to use a Dictionary for gates in cells, since only one will occupy each direction.
		}
	}
}
