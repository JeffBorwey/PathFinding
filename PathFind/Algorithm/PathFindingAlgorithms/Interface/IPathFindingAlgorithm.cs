﻿using Algorithm.EventArguments;
using GraphLib.Graphs.Abstractions;
using GraphLib.Vertex.Interface;
using System;

namespace Algorithm.PathFindingAlgorithms.Interface
{
    public delegate void AlgorithmEventHanlder(object sender, AlgorithmEventArgs e);

    /// <summary>
    /// A base interface of path finding algorithms
    /// </summary>
    public interface IPathFindingAlgorithm
    {
        event AlgorithmEventHanlder OnStarted;
        event Action<IVertex> OnVertexVisited;
        event AlgorithmEventHanlder OnFinished;
        event Action<IVertex> OnEnqueued;
        IGraph Graph { get; }
        /// <summary>
        /// Finds path from start vertex to end vertex by definite rules
        /// </summary>
        void FindPath();
    }
}