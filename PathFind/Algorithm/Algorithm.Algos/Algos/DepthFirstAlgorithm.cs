﻿using Algorithm.Base;
using Algorithm.Interfaces;
using Algorithm.Realizations.Heuristic.Distances;
using GraphLib.Interfaces;
using System.ComponentModel;
using System.Diagnostics;

namespace Algorithm.Algos.Algos
{
    [DebuggerDisplay("Depth first algorithm")]
    [Description("Depth first algorithm")]
    public sealed class DepthFirstAlgorithm : GreedyAlgorithm
    {
        public DepthFirstAlgorithm(IEndPoints endPoints, IHeuristic heuristic)
            : base(endPoints)
        {
            this.heuristic = heuristic;
        }

        public DepthFirstAlgorithm(IEndPoints endPoints)
            : this(endPoints, new ManhattanDistance())
        {

        }

        protected override double GreedyHeuristic(IVertex vertex)
        {
            return heuristic.Calculate(vertex, endPoints.Source);
        }

        private readonly IHeuristic heuristic;
    }
}
