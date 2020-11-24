﻿using GraphLib.Extensions;
using GraphLib.Graphs.Abstractions;
using GraphLib.Vertex.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphLib
{
    public sealed class Path : IEnumerable<IVertex>
    {
        public event EventHandler OnVertexHighlighted;

        public Path()
        {
            path = new IVertex[] { };
        }

        public int PathLength { get; private set; }

        public int PathCost { get; private set; }

        public void HighlightPath()
        {
            foreach (IVertex vertex in path)
            {
                if (vertex.IsSimpleVertex())
                {
                    vertex.MarkAsPath();
                    OnVertexHighlighted?.Invoke(vertex, new EventArgs());
                }
            }
        }

        public bool ExtractPath(IGraph graph)
        {
            if (graph.IsExtremeVerticesVisited())
            {
                path = UnwindPath(graph.End);
                PathCost = path.Sum(vertex => (int)vertex.Cost);
                PathLength = path.Count();
            }

            return path.Any();
        }

        public IEnumerator<IVertex> GetEnumerator()
        {
            return path.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return path.GetEnumerator();
        }

        private IEnumerable<IVertex> UnwindPath(IVertex end)
        {
            var temp = end;

            while (!temp.IsStart && !temp.IsDefault)
            {
                yield return temp;
                temp = temp.ParentVertex;
            }
        }

        private IEnumerable<IVertex> path;
    }
}
