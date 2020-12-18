﻿using GraphLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphLib.Coordinates.Abstractions
{
    [Serializable]
    public abstract class BaseCoordinate : ICoordinate
    {
        public BaseCoordinate(params int[] coordinates)
        {
            Coordinates = coordinates.ToArray();
        }

        public IEnumerable<int> Coordinates { get; }

        public abstract IEnumerable<ICoordinate> Environment { get; }

        public bool IsDefault => false;

        public override bool Equals(object pos)
        {
            if (pos is ICoordinate coordinate)
            {
                return coordinate.IsEqual(this);
            }

            throw new ArgumentException("Invalid value to compare");
        }

        public override int GetHashCode()
        {
            return Coordinates.Aggregate((x, y) => x ^ y);
        }

        public override string ToString()
        {
            var information = new StringBuilder("(");
            var coordinatesInString = Coordinates.
                Select(coordinate => coordinate.ToString());

            for (int i = 0; i < coordinatesInString.Count() - 1; i++)
            {
                information.Append(coordinatesInString.
                    ElementAt(i)).Append(",");
            }

            information.Append(coordinatesInString.Last()).Append(")");
            return information.ToString();
        }

        public abstract object Clone();

        protected IEnumerable<ICoordinate> coordinateEnvironment;
    }
}
