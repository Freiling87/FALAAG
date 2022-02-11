using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class Chunk
	{
		// The Chunk should be a template - do not assign the cells inside to the map, but copy them and adjust their x/y/z accordingly.

		public string ID;
		public List<Cell> Cells = new List<Cell>();
		public Cell entryCell; // For accurate placement, as opposed to placement by Origin
		public Cell Origin =>
			Cells.Where(c => c.X == 0 && c.Y == 0 && c.Z == 0).FirstOrDefault();

		public bool Rotated { get; set; }

		int width =>
			Rotated
				? (Cells.Max(c => c.X) - Cells.Min(c => c.X)) + 1
				: (Cells.Max(c => c.Y) - Cells.Min(c => c.Y)) + 1;

		int depth =>
			Rotated
				? (Cells.Max(c => c.Y) - Cells.Min(c => c.Y)) + 1
				: (Cells.Max(c => c.X) - Cells.Min(c => c.X)) + 1;

		int height =>
				(Cells.Max(c => c.Z) - Cells.Min(c => c.Z)) + 1;

		public Chunk(string ID)
		{

		}

		public Chunk Clone() =>
			new Chunk(ID);
	}
}
