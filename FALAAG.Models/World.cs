using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class World
	{
		private readonly List<Cell> _cells = new List<Cell>();

		public void AddCell(Cell location) =>
			_cells.Add(location);

		public Cell GetCell(int x, int y, int z) =>
			_cells.Where(l => 
				l.X == x && 
				l.Y == y && 
				l.Z == z).DefaultIfEmpty(null).FirstOrDefault();
	}
}
