﻿using GraphLib.Coordinates.Interface;
using GraphLib.Extensions;
using System;
using System.Collections.Generic;

namespace GraphLib.Coordinates
{
    /// <summary>
    /// Cartesian coordinates of the vertex on the graph
    /// </summary>
    [Serializable]
    public class Coordinate2D : ICoordinate
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public IEnumerable<int> Coordinates => new int[] { X, Y };

        public IEnumerable<ICoordinate> Environment
        {
            get
            {
                for (int i = X - 1; i <= X + 1; i++)
                    for (int j = Y - 1; j <= Y + 1; j++)
                        yield return new Coordinate2D(i, j);
            }
        }

        public Coordinate2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object pos)
        {
            if (pos == null)
                return false;
            return this.IsEqual(pos as ICoordinate);
        }

        public static bool operator ==(Coordinate2D position1, Coordinate2D position2)
        {
            return position1.Equals(position2);
        }

        public static bool operator !=(Coordinate2D position1, Coordinate2D position2)
        {
            return !position1.Equals(position2);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", X, Y);
        }

        public object Clone()
        {
            return new Coordinate2D(X, Y);
        }
    }
}