﻿using Algorithm.Base;
using Algorithm.Infrastructure.EventArguments;
using Algorithm.Interfaces;
using Common.Extensions.EnumerableExtensions;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using System;
using System.Runtime.CompilerServices;
using Visualization.Extensions;
using Visualization.Interfaces;
using Visualization.Realizations;

namespace Visualization
{
    public abstract class PathfindingVisualization : IVisualization
    {
        protected PathfindingVisualization(IGraph graph)
        {
            visualizations = new CompositeVisualization(graph, obstacles, enqueued, visited, path, intermediate, source, target);
            visualizationSlides = new CompositeVisualizationSlides(obstacles, enqueued, visited, path, intermediate, source, target);
            this.graph = graph;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => visualizationSlides.Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(IAlgorithm algorithm) => visualizationSlides.Remove(algorithm);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Visualize(IAlgorithm algorithm) => visualizations.Visualize(algorithm);

        protected virtual void SubscribeOnAlgorithmEvents(PathfindingAlgorithm algorithm)
        {
            algorithm.VertexVisited += OnVertexVisited;
            algorithm.VertexEnqueued += OnVertexEnqueued;
            algorithm.Finished += OnAlgorithmFinished;
            algorithm.Started += OnAlgorithmStarted;
        }

        protected void AddEndPoints(IAlgorithm algorithm, IEndPoints endPoints)
        {
            source.Add(algorithm, endPoints.Source);
            target.Add(algorithm, endPoints.Target);
            var intermediates = endPoints.GetIntermediates();
            intermediate.AddRange(algorithm, intermediates);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void AddPathVertices(IAlgorithm algorithm, IGraphPath grapPath)
        {
            path.AddRange(algorithm, grapPath.Path);
        }

        protected virtual void OnAlgorithmStarted(object sender, EventArgs e)
        {
            if (sender is IAlgorithm algorithm)
            {
                obstacles.AddRange(algorithm, graph.GetObstacles());
            }
        }

        protected virtual void OnVertexVisited(object sender, AlgorithmEventArgs e)
        {
            if (sender is IAlgorithm algo && e.Current.TryVisualizeAsVisited())
            {
                visited.Add(algo, e.Current);
            }
        }

        protected virtual void OnVertexEnqueued(object sender, AlgorithmEventArgs e)
        {
            if (sender is IAlgorithm algo && e.Current.TryVisualizeAsEnqueued())
            {
                enqueued.Add(algo, e.Current);
            }
        }

        protected virtual void OnAlgorithmFinished(object sender, EventArgs e)
        {
            if (sender is IAlgorithm algorithm)
            {
                enqueued.RemoveRange(algorithm, visited.GetVertices(algorithm));
            }
        }

        private readonly VisitedVertices visited = new VisitedVertices();
        private readonly EnqueuedVertices enqueued = new EnqueuedVertices();
        private readonly PathVertices path = new PathVertices();
        private readonly IntermediateVertices intermediate = new IntermediateVertices();
        private readonly SourceVertices source = new SourceVertices();
        private readonly TargetVertices target = new TargetVertices();
        private readonly ObstacleVertices obstacles = new ObstacleVertices();
        private readonly IVisualization visualizations;
        private readonly IVisualizationSlides visualizationSlides;
        private readonly IGraph graph;
    }
}