﻿using Common.Extensions;
using GraphLib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphLib.Extensions
{
    public static class IEnumerableExtensions
    {
        private static int Multiply(int x, int y) => x * y;

        /// <summary>
        /// Converts coordinates values to index in a multidimensional array
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dimensions">dimensions of array except first dimension</param>
        /// <returns>Index of coordinate in a multidimensional array</returns>
        /// <exception cref="ArgumentNullException">Thrown when argument is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        public static int ToIndex(this IEnumerable<int> self, params int[] dimensions)
        {
            #region InvariantsObservance
            var message = "An error occured while converting coordinate to index\n";
            if (dimensions == null)
            {
                message += "Argument can't be null\n";
                throw new ArgumentNullException(nameof(dimensions), message);
            }

            if (self.Count() != dimensions.Length)
            {
                message += "Dimensions length must be equal to the number of coordinate values\n";
                message += $"{nameof(dimensions)} has {dimensions.Length} length " +
                    $"and {nameof(self)} has {self.Count()} length\n";
                throw new ArgumentOutOfRangeException(nameof(dimensions), message);
            }

            if (!self.Match(dimensions, (a, b) => a < b))
            {
                message += "Coordinate is out of dimensions range\n";
                throw new Exception(message);
            }
            #endregion

            return dimensions
                .Skip(1)
                .StepAggregate(Multiply)
                .Zip(self, Multiply)
                .Sum() + self.Last();
        }

        /// <summary>
        /// Checks whether coordinate is within graph
        /// </summary>
        /// <param name="coordinateValues"></param>
        /// <param name="graph"></param>
        /// <returns>true if <paramref name="coordinate"/> coordinates 
        /// values is not greater than the corresponding dimension if graph 
        /// and is not lesser than 0; false if it is or coordinate has more or 
        /// less coordinates values than <paramref name="graph"/> has dimensions</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of parametres is null</exception>
        public static bool IsWithinGraph(this IEnumerable<int> coordinateValues, IGraph graph)
        {
            #region InvariantsObservance
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph), "Argument can't be null");
            }
            #endregion

            return coordinateValues.Match(graph.DimensionsSizes, IsWithin);
        }


        /// <summary>
        /// At each step skips the number of elements equal 
        /// to the number of loop iterations and applies an 
        /// aggregate function to the remaining elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        /// <returns>Result of aggregation</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of arguments is null</exception>
        /// <exception cref="InvalidOperationException"></exception>
        internal static IEnumerable<T> StepAggregate<T>(this IEnumerable<T> collection, Func<T, T, T> func)
        {
            return collection.Select((arr, i) => arr.Skip(i).Aggregate(func));
        }

        public static T AggregateOrDefault<T>(this IEnumerable<T> collection, Func<T,T,T> func)
        {
            return collection.Any() ? collection.Aggregate(func) : default;
        }

        internal static IEnumerable<T> ForEachIndex<T>(this IEnumerable<T> collection, Action<int> action)
        {
            for (int i = 0; i < collection.Count(); i++)
            {
                action(i);
            }

            return collection;
        }

        private static IEnumerable<T> Select<T>(this IEnumerable<T> collection, Func<IEnumerable<T>, int, T> func)
        {           
            for (int i = 0; i < collection.Count(); i++)
            {
                yield return func(collection, i);
            }
        }

        private static bool IsWithin(int coordinate, int graphDimension)
        {
            return coordinate < graphDimension && coordinate >= 0;
        }


    }
}
