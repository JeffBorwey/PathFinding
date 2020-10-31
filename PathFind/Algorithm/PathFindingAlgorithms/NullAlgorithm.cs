﻿using System;
using GraphLib.Vertex.Interface;
using GraphLib.Vertex;
using Algorithm.EventArguments;
using Algorithm.PathFindingAlgorithms.Interface;
using GraphLib.Graphs;
using GraphLib.Graphs.Abstractions;

namespace Algorithm.PathFindingAlgorithms
{
    public sealed class NullAlgorithm : IPathFindingAlgorithm
    {
        private NullAlgorithm()
        {

        }

        public static NullAlgorithm Instance
        {
            get
            {
                if (instance == null)
                    instance = new NullAlgorithm();
                return instance;
            }
        }

        public IGraph Graph { get => NullGraph.Instance; set => _ = value; }

        public event AlgorithmEventHanlder OnStarted;
        public event Action<IVertex> OnVertexVisited;
        public event AlgorithmEventHanlder OnFinished;
        public event Action<IVertex> OnEnqueued;

        public void FindPath()
        {
            OnStarted?.Invoke(this, new AlgorithmEventArgs());
            OnVertexVisited?.Invoke(NullVertex.Instance);
            OnFinished?.Invoke(this, new AlgorithmEventArgs());
            OnEnqueued?.Invoke(NullVertex.Instance);
            OnStarted = delegate { };
            OnVertexVisited = delegate { };
            OnFinished = delegate { };
            OnEnqueued = delegate { };
        }

        private static NullAlgorithm instance = null;
    }
}