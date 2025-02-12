﻿using Common.Extensions.EnumerableExtensions;
using GraphLib.Interfaces;
using GraphLib.Proxy.Extensions;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ValueRange;
using ValueRange.Extensions;

namespace GraphLib.Extensions
{
    public static class CoordinateExtensions
    {
        /// <summary>
        /// Compares two coordinates
        /// </summary>
        /// <param name="self"></param>
        /// <param name="coordinate"></param>
        /// <returns>true if all of the coordinates values of <paramref name="self"/> 
        /// equals to the corresponding coordinates of <paramref name="coordinate"/>;
        /// false if not, or if they have not equal number of coordinates values
        /// or any of parametres is null</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqual(this ICoordinate self, ICoordinate coordinate)
        {
            if (self == null || coordinate == null)
            {
                return false;
            }

            return self.CoordinatesValues.Juxtapose(coordinate.CoordinatesValues);
        }

        /// <summary>
        /// Checks, whether <paramref name="neighbour"/> 
        /// is cardinal to <paramref name="coordinate"/>
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="neighbour"></param>
        /// <returns>True if <paramref name="neighbour"/> is cardinal to <paramref name="coordinate"/></returns>
        public static bool IsCardinal(this ICoordinate coordinate, ICoordinate neighbour)
        {
            return coordinate.CoordinatesValues.IsCardinal(neighbour.CoordinatesValues);
        }

        /// <summary>
        /// Checks whether coordinate is within graph
        /// </summary>
        /// <param name="self"></param>
        /// <param name="graph"></param>
        /// <returns>true if <paramref name="self"/> coordinates 
        /// values is not greater than the corresponding dimension if graph 
        /// and is not lesser than 0; false if it is or coordinate has more or 
        /// less coordinates values than <paramref name="graph"/> has dimensions</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of parametres is null</exception>
        public static bool IsWithinGraph(this ICoordinate self, IGraph graph)
        {
            bool IsWithin(int coordinate, int graphDimension)
            {
                var range = new InclusiveValueRange<int>(graphDimension - 1);
                return range.Contains(coordinate);
            }

            return self.IsWithinGraph(graph, IsWithin);
        }

        /// <summary>
        /// Checks whether coordinate is within graph according to <paramref name="predicate"/>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="graph"></param>
        /// <param name="predicate"></param>
        /// <exception cref="ArgumentNullException">Thrown when any of parametres is null</exception>
        public static bool IsWithinGraph(this ICoordinate self, IGraph graph, Func<int, int, bool> predicate)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            return self.CoordinatesValues.Juxtapose(graph.DimensionsSizes, predicate);
        }

        public static ICoordinate Substract(this ICoordinate self, ICoordinate coordinate)
        {
            var substract = self.CoordinatesValues.Zip(coordinate.CoordinatesValues, (x, y) => x - y);
            return substract.ToCoordinate();
        }

        public static double GetScalarProduct(this ICoordinate self, ICoordinate coordinate)
        {
            return self.CoordinatesValues.Zip(coordinate.CoordinatesValues, (a, b) => a * b).SumOrDefault();
        }

        public static double GetVectorLength(this ICoordinate self)
        {
            return Math.Sqrt(self.CoordinatesValues.Select(x => x * x).SumOrDefault());
        }
    }
}
