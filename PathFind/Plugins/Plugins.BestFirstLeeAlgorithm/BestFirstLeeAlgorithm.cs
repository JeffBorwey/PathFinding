﻿using Algorithm.Extensions;
using AssembleClassesLib.Attributes;
using GraphLib.Interfaces;
using System.Linq;

namespace Plugins.BestFirstLeeAlgorithm
{
    [ClassName("Lee algorithm (heuristic)")]
    public class BestFirstLeeAlgorithm : LeeAlgorithm.LeeAlgorithm
    {
        public BestFirstLeeAlgorithm(IGraph graph, IEndPoints endPoints)
            : base(graph, endPoints)
        {

        }

        protected override IVertex NextVertex
        {
            get
            {
                verticesQueue = verticesQueue.OrderBy(accumulatedCosts.GetAccumulatedCost).ToQueue();
                return base.NextVertex;
            }
        }

        protected virtual double CalculateHeuristic(IVertex vertex)
        {
            return vertex.CalculateChebyshevDistanceTo(endPoints.End);
        }

        protected override double CreateWave()
        {
            return base.CreateWave() + CalculateHeuristic(CurrentVertex);
        }
    }
}
