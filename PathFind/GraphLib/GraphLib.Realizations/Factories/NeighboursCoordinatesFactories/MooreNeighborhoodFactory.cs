﻿using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using GraphLib.Realizations.NeighboursCoordinates;

namespace GraphLib.Realizations.Factories.NeighboursCoordinatesFactories
{
    public sealed class MooreNeighborhoodFactory : INeighborhoodFactory
    {
        public INeighborhood CreateNeighborhood(ICoordinate coordinate)
        {
            return new MooreNeighborhood(coordinate);
        }
    }
}