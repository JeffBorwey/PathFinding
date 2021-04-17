﻿using Algorithm.Infrastructure.EventArguments;
using Algorithm.Infrastructure.Handlers;
using Algorithm.Interfaces;
using Common.Attributes;
using GraphLib.Interfaces;
using System;

namespace Algorithm.Common
{
    [Default]
    [Filterable]
    public sealed class DefaultAlgorithm : IAlgorithm
    {
        public event AlgorithmEventHandler OnStarted;
        public event AlgorithmEventHandler OnVertexVisited;
        public event AlgorithmEventHandler OnFinished;
        public event AlgorithmEventHandler OnVertexEnqueued;
        public event EventHandler OnInterrupted;

        public IGraphPath FindPath(IEndPoints endpoints)
        {
            OnStarted?.Invoke(this, new AlgorithmEventArgs());
            OnVertexVisited?.Invoke(this, new AlgorithmEventArgs());
            OnVertexEnqueued?.Invoke(this, new AlgorithmEventArgs());
            OnFinished?.Invoke(this, new AlgorithmEventArgs());
            return new NullGraphPath();
        }

        public void Interrupt()
        {
            OnInterrupted?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            
        }
    }
}
