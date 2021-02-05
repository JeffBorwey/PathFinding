﻿using Common.Extensions;
using GraphLib.Infrastructure;
using GraphLib.Interface;
using GraphLib.NullObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphLib.Extensions
{
    public static class IVertexExtension
    {
        public static bool IsValidToBeExtreme(this IVertex vertex)
        {
            return vertex.IsSimpleVertex() && !vertex.IsIsolated();
        }

        public static bool IsIsolated(this IVertex vertex)
        {
            return vertex.IsObstacle || !vertex.Neighbours.Any();
        }

        /// <summary>
        /// Returns vertex to its start state
        /// </summary>
        /// <param name="vertex"></param>
        public static void SetToDefault(this IVertex vertex)
        {
            vertex.IsStart = false;
            vertex.IsEnd = false;
            vertex.IsVisited = false;
            vertex.AccumulatedCost = 0;
            vertex.ParentVertex = new DefaultVertex();
            vertex.MarkAsSimpleVertex();
        }

        public static void Initialize(this IVertex vertex)
        {
            vertex.Neighbours = new List<IVertex>();
            vertex.SetToDefault();
            vertex.IsObstacle = false;
        }

        public static void Initialize(this IVertex vertex, VertexSerializationInfo info)
        {
            vertex.Position = (ICoordinate)info.Position.Clone();
            vertex.Cost = info.Cost;
            vertex.IsObstacle = info.IsObstacle;

            if (vertex.IsObstacle)
            {
                vertex.MarkAsObstacle();
            }
        }

        public static void WashVertex(this IVertex vertex)
        {
            vertex.IsObstacle = true;
        }

        public static bool IsSimpleVertex(this IVertex vertex)
        {
            return !vertex.IsStart && !vertex.IsEnd;
        }

        public static IEnumerable<IVertex> GetUnvisitedNeighbours(this IVertex self)
        {
            return self.Neighbours.Where(vertex => !vertex.IsVisited && !vertex.IsObstacle);
        }

        internal static void Refresh(this IVertex vertex)
        {
            if (!vertex.IsObstacle)
            {
                vertex.SetToDefault();
            }
        }

        internal static void Isolate(this IVertex self)
        {
            self.Neighbours.ForEach(vertex => vertex.Neighbours.Remove(self));
            self.Neighbours.Clear();
        }

        internal static VertexSerializationInfo GetSerializationInfo(this IVertex self)
        {
            return new VertexSerializationInfo(self);
        }

        /// <summary>
        /// Sets <paramref name="self"/> as neighbour of its neighbours
        /// </summary>
        /// <param name="self"></param>
        public static void ConnectWithNeighbours(this IVertex self)
        {
            self.Neighbours
                .Where(vertex => self.CanBeNeighbourOf(vertex))
                .ForEach(vertex => vertex.Neighbours.Add(self));
        }

        public static bool IsEqual(this IVertex self, IVertex vertex)
        {
            bool hasEqualCost = self.Cost.Equals(vertex.Cost);
            bool hasEqualPosition = self.Position.IsEqual(vertex.Position);
            bool hasEqualState = self.IsObstacle == vertex.IsObstacle;
            return hasEqualCost && hasEqualPosition && hasEqualState;
        }

        /// <summary>
        /// Sets certain vertices of <paramref name="self"/>'s environment as its neighbors
        /// </summary>
        /// <param name="self"></param>
        /// <param name="graph">A graph, where vertex is situated</param>
        /// <exception cref="ArgumentNullException">Thrown when
        /// any of parametre is empty</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="graph"/> 
        /// doesn't contain <paramref name="self"/></exception>
        internal static void SetNeighbours(this IVertex self, IGraph graph)
        {
            var message = "An error was occured while setting vertices neighbours\n";
            if (graph == null)
            {
                message += "Argument was null\n";
                throw new ArgumentNullException(nameof(graph), message);
            }

            if (!graph.Contains(self))
            {
                message += "Vertex doesn't belong to graph\n";
                throw new ArgumentException(message, nameof(graph));
            }

            if (!self.IsObstacle && !graph.IsDefault)
            {
                self.Position.Environment
                    .Where(coordinate => coordinate.IsWithinGraph(graph))
                    .Select(coordinate => graph[coordinate])
                    .Where(vertex => vertex.CanBeNeighbourOf(self))
                    .ForEach(self.Neighbours.Add);
            }
        }

        private static bool CanBeNeighbourOf(this IVertex self, IVertex vertex)
        {
            return !self.IsObstacle 
                && !ReferenceEquals(vertex, self)
                && !self.IsNeighbourOf(vertex);
        }

        private static bool IsNeighbourOf(this IVertex self, IVertex vertex)
        {
            return vertex.Neighbours.Contains(self);
        }
    }
}