﻿using Common.Extensions.EnumerableExtensions;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using GraphLib.Realizations.Graphs;
using GraphLib.Realizations.Neighbourhoods;
using GraphLib.TestRealizations.TestFactories.Matrix;
using GraphLib.TestRealizations.TestObjects;

namespace GraphLib.TestRealizations.TestFactories
{
    public sealed class TestGraph2DAssemble : IGraphAssemble
    {
        public IGraph AssembleGraph(int obstaclePercent = 0, params int[] sizes)
        {
            int size = Constants.DimensionSizes2D.GetMultiplication();
            var vertices = new TestVertex[size];
            for (int index = 0; index < size; index++)
            {
                var coordinates = Constants.DimensionSizes2D.ToCoordinates(index);
                var coordinate = new TestCoordinate(coordinates);
                var neighborhood = new MooreNeighborhood(coordinate);
                vertices[index] = new TestVertex(neighborhood, coordinate);
            }
            var graph = new Graph2D(vertices, Constants.DimensionSizes2D);
            var costMatrix = new CostMatrix(graph);
            var obstacleMatric = new ObstacleMatrix(graph);
            var matrices = new Matrices(costMatrix, obstacleMatric);
            matrices.Overlay();
            return graph;
        }
    }
}
