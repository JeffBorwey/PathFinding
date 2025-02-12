﻿using Algorithm.Interfaces;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using System;

namespace Algorithm.Realizations.Heuristic
{
    public sealed class Angle : IHeuristic
    {
        private const double Radians = 57.2956;

        public Angle(IVertex startingPoint)
        {
            this.startingPoint = startingPoint.Position;
        }

        public double Calculate(IVertex first, IVertex second)
        {
            var firstSubstract = first.Position.Substract(startingPoint);
            var secondSubstract = second.Position.Substract(startingPoint);
            double scalarProduct = firstSubstract.GetScalarProduct(secondSubstract);
            double firstVectorLength = firstSubstract.GetVectorLength();
            double secondVectorLength = secondSubstract.GetVectorLength();
            double vectorSum = firstVectorLength * secondVectorLength;
            var cosValue = vectorSum > 0 ? scalarProduct / vectorSum : 0;
            return Math.Round(Radians * Math.Acos(cosValue), digits: 3);
        }

        private readonly ICoordinate startingPoint;
    }
}