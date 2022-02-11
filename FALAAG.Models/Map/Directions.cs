using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FALAAG.Models
{
    public enum Direction
    {
        East,
        North,
        South,
        West,
        Above,
        Below,
        Center
    }

    public static class Directions
    {
        static readonly List<Direction> PlaneXY = new List<Direction>() { Direction.North, Direction.East, Direction.South, Direction.West };
        static readonly List<Direction> PlaneXZ = new List<Direction>() { Direction.East, Direction.Below, Direction.West, Direction.Above };
        static readonly List<Direction> PlaneYZ = new List<Direction>() { Direction.North, Direction.Below, Direction.South, Direction.Above };

        private static Direction FlipDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.East:
                    return Direction.West;
                case Direction.North:
                    return Direction.South;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                case Direction.Above:
                    return Direction.Below;
                case Direction.Below:
                    return Direction.Above;
                default:
                    return Direction.Center;
            }
        }

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
