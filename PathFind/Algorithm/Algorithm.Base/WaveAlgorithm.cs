﻿using Algorithm.Extensions;
using Algorithm.Infrastructure.EventArguments;
using Algorithm.Interfaces;
using Algorithm.NullRealizations;
using Algorithm.Realizations.GraphPaths;
using Common.Extensions;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using Interruptable.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Base
{
    /// <summary>
    /// A base class for all wave algorithms, such as Dijkstra's algorithm,
    /// A* algorithm or Lee algorithm. This class is abstract
    /// </summary>
    public abstract class WaveAlgorithm : PathfindingAlgorithm,
        IAlgorithm, IInterruptableProcess, IInterruptable, IDisposable
    {
        protected WaveAlgorithm(IGraph graph, IIntermediateEndPoints endPoints)
            : base(graph, endPoints)
        {
            verticesQueue = new Queue<IVertex>();
        }

        /// <summary>
        /// Finds the cheapest path in the graph. 
        /// This method can't be overriden
        /// </summary>
        /// <returns>A cheapest path in the graph 
        /// between speciаied end points</returns>
        public override sealed IGraphPath FindPath()
        {
            IGraphPath path = new NullGraphPath();
            PrepareForPathfinding();
            foreach (var endPoint in endPoints.ToEndPoints())
            {
                CurrentEndPoints = endPoint;
                PrepareForLocalPathfinding();
                do
                {
                    VisitVertex(CurrentVertex);
                    var neighbours = GetUnvisitedNeighbours(CurrentVertex);
                    ExtractNeighbours(neighbours);
                    RelaxNeighbours(neighbours);
                    CurrentVertex = NextVertex;
                } while (!IsDestination(CurrentEndPoints));
                if (!IsAbleToContinue) break;
                var foundPath = CreateGraphPath();
                path = new CombinedGraphPath(path, foundPath);
                Reset();
            }
            CompletePathfinding();
            return !IsAbleToContinue ? new NullGraphPath() : path;
        }

        protected override sealed void Reset()
        {
            base.Reset();
            verticesQueue.Clear();
            accumulatedCosts?.Clear();
        }

        protected virtual void PrepareForLocalPathfinding()
        {
            CurrentVertex = CurrentEndPoints.Source;
        }

        protected virtual IGraphPath CreateGraphPath()
        {
            return new GraphPath(parentVertices, CurrentEndPoints);
        }

        protected virtual void VisitVertex(IVertex vertex)
        {
            visitedVertices.Add(vertex);
            RaiseVertexVisited(new AlgorithmEventArgs(vertex));
        }

        protected abstract void RelaxNeighbours(IVertex[] vertex);
        protected virtual IEndPoints CurrentEndPoints { get; set; }

        protected virtual void ExtractNeighbours(IVertex[] neighbours)
        {
            foreach (var neighbour in neighbours)
            {
                RaiseVertexEnqueued(new AlgorithmEventArgs(neighbour));
                verticesQueue.Enqueue(neighbour);
            }

            verticesQueue = verticesQueue
                .DistinctBy(vertex => vertex.Position)
                .ToQueue();
        }

        protected Queue<IVertex> verticesQueue;

        private IVertex[] GetUnvisitedNeighbours(IVertex vertex)
        {
            return visitedVertices
                .GetUnvisitedNeighbours(vertex)
                .FilterObstacles()
                .ToArray();
        }
    }
}