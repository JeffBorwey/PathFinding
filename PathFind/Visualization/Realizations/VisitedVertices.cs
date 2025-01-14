﻿using GraphLib.Interfaces;
using Visualization.Abstractions;
using Visualization.Interfaces;

namespace Visualization.Realizations
{
    /// <summary>
    /// A class, that stores visited vertices by algorithm
    /// </summary>
    internal sealed class VisitedVertices : AlgorithmVertices, IVisualization
    {
        protected override void Visualize(IVisualizable visualizable)
        {
            visualizable.VisualizeAsVisited();
        }
    }
}
