using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
	public class World
	{
		private readonly List<Cell> _mapCells = new List<Cell>();

		public void AddCell(Cell location) =>
			_mapCells.Add(location);

		public Cell GetCell(int x, int y, int z) =>
			_mapCells.Where(l => 
				l.X == x && 
				l.Y == y && 
				l.Z == z).DefaultIfEmpty(null).FirstOrDefault();

        public Cell GetNeighbor(Cell cell, Direction direction) =>
            GetCell(
                direction == Direction.East
                    ? cell.X + 1
                    : direction == Direction.West
                        ? cell.X - 1
                        : cell.X,
                direction == Direction.North
                    ? cell.Y + 1
                    : direction == Direction.South
                        ? cell.Y - 1
                        : cell.Y,
                direction == Direction.Above
                    ? cell.Z + 1
                    : direction == Direction.Below
                        ? cell.Z - 1
                        : cell.Z);
    }
}
