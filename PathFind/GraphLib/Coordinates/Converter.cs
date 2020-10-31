﻿using GraphLib.Coordinates.Interface;
using System;
using System.Linq;

namespace GraphLib.Coordinates
{
    public static class Converter
    {
        public static int ToIndex(ICoordinate coordinate, params int[] referenceDimensions)
        {
            if (coordinate.Coordinates.Count() != referenceDimensions.Length + 1)
                throw new ArgumentException("Not enough arguments");
            var coordinates = coordinate.Coordinates.ToArray();
            int index = coordinates.Last();
            for (int i = 0; i < referenceDimensions.Length; i++)            
                index += referenceDimensions[i] * coordinates[i];
            return index;
        }
    }
}