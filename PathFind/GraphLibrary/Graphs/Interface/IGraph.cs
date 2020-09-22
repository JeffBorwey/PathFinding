﻿using GraphLibrary.DTO;
using GraphLibrary.Vertex.Interface;
using System.Collections.Generic;

namespace GraphLibrary.Graphs.Interface
{
    public struct GraphParametres
    {
        public int Width { get; }
        public int Height { get; }
        public int ObstaclePercent { get; }

        public GraphParametres(int width,
            int height, int obstaclePercent)
        {
            Width = width;
            Height = height;
            ObstaclePercent = obstaclePercent;
        }
    }

    /// <summary>
    /// Provides methods for accessing the vertices of the graph, 
    /// as well as for getting information about the graph
    /// </summary>
    public interface IGraph : IEnumerable<IVertex>
    {
        IVertex this[int width, int height] { get; set; }
        IVertex this[Position position] { get; set; }
        IVertex End { get; set; }
        int Height { get; }
        int NumberOfVisitedVertices { get; }
        int ObstacleNumber { get; }
        int ObstaclePercent { get; }
        int Size { get; }
        IVertex Start { get; set; }
        VertexDto[,] VerticesDto { get; }
        int Width { get; }
        IVertex[,] Array { get; }
        Position GetIndices(IVertex vertex);
    }
}