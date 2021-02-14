﻿using GraphLib.Base;
using System;
using System.Linq;

namespace GraphLib
{
    /// <summary>
    /// A class representing cartesian two-dimensional coordinates
    /// </summary>
    [Serializable]
    public sealed class Coordinate2D : BaseCoordinate
    {
        public int X => CoordinatesValues.First();

        public int Y => CoordinatesValues.Last();

        public Coordinate2D(int x, int y)
            : this(new int[] { x, y })
        {

        }

        public Coordinate2D(params int[] coordinates)
            : base(numberOfDimensions: 2, coordinates)
        {

        }
    }
}
