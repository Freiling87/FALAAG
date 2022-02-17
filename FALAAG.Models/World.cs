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
        public int width;
        public int depth;
        public int height;

		public void AddCell(Cell cell)
        {
            _mapCells.Add(cell);
        }
        public void AddEdgeCell(int x, int y, int z)
		{
            Cell cell = new (x, y, z, true);
            // TODO: Set it as impassable, etc.
            _mapCells.Add(cell);
        }

        // TODO: See how many of these you can move to Cell, now that it's storing neighbor info.
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

        public void PlaceEdgeCells()
		{
            // TODO: In the future, when a map editor is made or custom chunks are proc-gen placed, you will need to move !their walls and give them to neighbors as appropriate.

            int minX = _mapCells.Min(c => c.X);
            int minY = _mapCells.Min(c => c.Y);
            int minZ = _mapCells.Min(c => c.Z);
            int maxX = _mapCells.Max(c => c.X);
            int maxY = _mapCells.Max(c => c.Y);
            int maxZ = _mapCells.Max(c => c.Z);

            for (int xTraversal = minX; xTraversal <= maxX + 1; xTraversal++)
                for (int yTraversal = minY; yTraversal <= maxY + 1; yTraversal++)
                    for (int zTraversal = minZ; zTraversal <= maxZ + 1; zTraversal++)
                        if (GetCell(xTraversal, yTraversal, zTraversal) == null)
                            AddEdgeCell(xTraversal, yTraversal, zTraversal);
        }

        public void UpdateNeighbors()
        {
            foreach (Cell cell in _mapCells)
            {
                cell.CellAbove = GetNeighbor(cell, Direction.Above);
                cell.CellBelow = GetNeighbor(cell, Direction.Below);
                cell.CellEast = GetNeighbor(cell, Direction.East);
                cell.CellNorth = GetNeighbor(cell, Direction.North);
                cell.CellSouth = GetNeighbor(cell, Direction.South);
                cell.CellWest = GetNeighbor(cell, Direction.West);
            }
        }
    }
}
