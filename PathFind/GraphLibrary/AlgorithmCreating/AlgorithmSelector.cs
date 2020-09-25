﻿using GraphLibrary.DistanceCalculating;
using GraphLibrary.Enums;
using GraphLibrary.Graphs.Interface;
using GraphLibrary.PathFindingAlgorithm;
using GraphLibrary.PathFindingAlgorithm.Interface;
using GraphLibrary.Vertex.Interface;

namespace GraphLibrary.AlgorithmCreating
{
    public static class AlgorithmFactory
    {
        private static double CastAndDistanceGreedyFunction(
            IVertex vertex, IVertex destination)
        {
            return vertex.Cost + DistanceCalculator.GetChebyshevDistance(vertex, destination);
        }

        public static IPathFindingAlgorithm CreateAlgorithm(
            Algorithms algorithms, IGraph graph)
        {
            switch (algorithms)
            {
                case Algorithms.LeeAlgorithm:
                    return new LeeAlgorithm()
                    {
                        Graph = graph
                    };

                case Algorithms.DeepPathFind:
                    return new GreedyAlgorithm()
                    {
                        Graph = graph,
                        GreedyFunction = vertex => DistanceCalculator.GetChebyshevDistance(vertex, graph.Start)
                    };

                case Algorithms.DijkstraAlgorithm:
                    return new DijkstraAlgorithm()
                    {
                        Graph = graph
                    };

                case Algorithms.AStarAlgorithm:
                    return new AStarAlgorithm()
                    {
                        Graph = graph,
                        HeuristicFunction = vertex => DistanceCalculator.GetChebyshevDistance(vertex, graph.End)
                    };

                case Algorithms.AStarModified:
                    return new AStarModified()
                    {
                        Graph = graph,
                        HeuristicFunction = vertex => DistanceCalculator.GetChebyshevDistance(vertex, graph.End)
                    };

                case Algorithms.DistanceGreedyAlgorithm:
                    return new GreedyAlgorithm()
                    {
                        Graph = graph,
                        GreedyFunction = vertex => DistanceCalculator.GetChebyshevDistance(vertex, graph.End)
                    };

                case Algorithms.ValueGreedyAlgorithm:
                    return new GreedyAlgorithm()
                    {
                        Graph = graph,
                        GreedyFunction = vertex => vertex.Cost
                    };

                case Algorithms.ValueDistanceGreedyAlgorithm:
                    return new GreedyAlgorithm()
                    {
                        Graph = graph,
                        GreedyFunction = vertex => CastAndDistanceGreedyFunction(vertex, graph.End)
                    };

                default: return NullAlgorithm.Instance;
            }
        }
    }
}