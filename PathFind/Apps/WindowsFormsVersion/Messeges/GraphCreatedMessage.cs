﻿using GraphLib.Interfaces;

namespace WindowsFormsVersion.Messeges
{
    internal sealed class GraphCreatedMessage
    {
        public IGraph Graph { get; }

        public GraphCreatedMessage(IGraph graph)
        {
            Graph = graph;
        }
    }
}
