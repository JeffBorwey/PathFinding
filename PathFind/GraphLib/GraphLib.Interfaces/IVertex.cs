﻿using System.Collections.Generic;

namespace GraphLib.Interface
{
    /// <summary>
    /// Represents a vertex of graph
    /// </summary>
    public interface IVertex
    {
        bool IsObstacle { get; set; }

        IVertexCost Cost { get; set; }

        ICollection<IVertex> Neighbours { get; set; }

        ICoordinate Position { get; set; }
    }
}