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

		public void AddCell(Cell cell)
        {
            _mapCells.Add(cell);
            cell.UpdateWalls();
        }

		public Cell GetCell(int x, int y, int z) =>
			_mapCells.Where(c => 
				c.X == x && 
				c.Y == y && 
				c.Z == z).DefaultIfEmpty(null).FirstOrDefault();

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

        public Wall GetWall(Cell cell, Direction direction) =>
            direction switch
            {
                Direction.Above => GetNeighbor(cell, Direction.Above).WallBelow,
                Direction.Below => cell.WallBelow,
                Direction.East => GetNeighbor(cell, Direction.East).WallWest,
                Direction.North => GetNeighbor(cell, Direction.North).WallSouth,
                Direction.South => cell.WallSouth,
                Direction.West => cell.WallWest,
                _ => throw new ArgumentException()
            };

        public Direction? GetWallDirection(Cell originCell, Wall wall)
        {
            if (wall == GetNeighbor(originCell, Direction.Above).WallBelow)
                return Direction.Above;
            else if (wall == originCell.WallBelow)
                return Direction.Below;
            else if (wall == GetNeighbor(originCell, Direction.East).WallWest)
                return Direction.East;
            else if (wall == GetNeighbor(originCell, Direction.North).WallSouth)
                return Direction.North;
            else if (wall == originCell.WallSouth)
                return Direction.South;
            else if (wall == originCell.WallWest)
                return Direction.West;

            return null;
		}
}
}
