﻿using GraphLib.Interfaces;
using System.Collections.Generic;

namespace GraphLib.TestRealizations.TestObjects
{
    public sealed class TestEndPoints : IEndPoints
    {
        public IVertex Target { get; }
        public IVertex Source { get; }
        public IEnumerable<IVertex> EndPoints { get; }

        public TestEndPoints(IVertex source, IVertex target)
        {
            Source = source;
            Target = target;
            EndPoints = new IVertex[] { Source, Target };
        }

        public bool IsEndPoint(IVertex vertex)
        {
            return Source.Equals(vertex) || Target.Equals(vertex);
        }
    }
}
