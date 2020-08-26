﻿using GraphLibrary.Extensions.MatrixExtension;
using GraphLibrary.Extensions.RandomExtension;
using GraphLibrary.Graph;
using GraphLibrary.Vertex;
using System;

namespace GraphLibrary.GraphFactory
{
    public abstract class AbstractGraphFactory 
        : AbstractGraphSetter, IGraphFactory
    {
        protected Random rand;

        public AbstractGraphFactory(int percentOfObstacles,
            int width, int height, int placeBetweenVertices) : base(placeBetweenVertices)
        {
            rand = new Random();
            IVertex InitializeTop(IVertex vertex)
            {
                vertex = CreateGraphTop();
                if (rand.IsObstacleChance(percentOfObstacles))
                {
                    vertex.IsObstacle = true;
                    vertex.MarkAsObstacle();
                }
                return vertex;
            }

            SetGraph(width, height);
            vertices.Apply(InitializeTop);
            vertices.Apply(SetLocation);
            var setter = new NeigbourSetter(GetGraph());
            setter.SetNeighbours();
        }

        protected abstract IVertex CreateGraphTop();

        public abstract AbstractGraph GetGraph();
    }
}
