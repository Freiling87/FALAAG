using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
    public enum Direction
    {
        Above,
        Below,
        East,
        North,
        South,
        West,
        Null
    }

    public static class DirectionManager
    {
        static readonly List<Direction> PlaneXY = new List<Direction>() { Direction.North, Direction.East, Direction.South, Direction.West };
        static readonly List<Direction> PlaneXZ = new List<Direction>() { Direction.East, Direction.Below, Direction.West, Direction.Above };
        static readonly List<Direction> PlaneYZ = new List<Direction>() { Direction.North, Direction.Below, Direction.South, Direction.Above };

        private static Direction? FlipDirection(Direction direction) =>
        direction switch
        {
            Direction.Above => Direction.Below,
            Direction.Below => Direction.Above,
            Direction.East => Direction.West,
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.West => Direction.East,
        };

        private static List<Direction> RotationPlane(CartesianAxis pinnedAxis)
		{
            if (pinnedAxis == CartesianAxis.X)
                return PlaneYZ;
            else if (pinnedAxis == CartesianAxis.Y)
                return PlaneXZ;
            else
                return PlaneXY;
		}

        private static Direction RotateDirection(Direction direction, int rotations = 1, bool clockwise = true, CartesianAxis pinnedAxis = CartesianAxis.Z)
        {
            if (!clockwise)
                rotations *= -1;

            List <Direction> rotationAxis = RotationPlane(pinnedAxis);
            int dirInt = Math.Abs(rotationAxis.IndexOf(direction) + rotations);
            direction = rotationAxis[dirInt % 4];

            return direction;
        }
    }
}
