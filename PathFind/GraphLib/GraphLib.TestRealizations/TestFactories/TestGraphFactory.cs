﻿using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using System.Collections.Generic;

namespace GraphLib.TestRealizations.TestFactories
{
    public class TestGraphFactory : IGraphFactory
    {
        public IGraph CreateGraph(IEnumerable<IVertex> vertices, int[] dimensionSizes)
        {
            return new TestGraph(vertices, dimensionSizes);
        }
    }
}
