﻿using GraphLib.Extensions;
using GraphLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Realizations.Heuristic.Distances
{
    /// <summary>
    /// A base class for all distance classes.
    /// Contains methods for calculating distance
    /// between two <see cref="IVertex"/> classes.
    /// This is an abstract class
    /// </summary>
    public abstract class Distance
    {
        public double Calculate(IVertex first, IVertex second)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException();
            }

            if (first.Position == null || second.Position == null)
            {
                throw new ArgumentException();
            }

            return Aggregate(first.GetCoordinates().Zip(second.GetCoordinates(), ZipMethod));
        }

        protected abstract double Aggregate(IEnumerable<double> collection);

        protected abstract double ZipMethod(int first, int second);
    }
}