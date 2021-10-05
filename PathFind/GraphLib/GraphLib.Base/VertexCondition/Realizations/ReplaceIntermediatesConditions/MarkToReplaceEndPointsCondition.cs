﻿using GraphLib.Base.EndPointsInspection.Abstractions;
using GraphLib.Base.VertexCondition.Interface;
using GraphLib.Extensions;
using GraphLib.Interfaces;

namespace GraphLib.Base.VertexCondition.Realizations.ReplaceIntermediatesConditions
{
    internal sealed class MarkToReplaceEndPointsCondition
        : BaseIntermediateEndPointsInspection, IVertexCondition
    {
        public MarkToReplaceEndPointsCondition(BaseEndPoints endPoints) : base(endPoints)
        {
        }

        public void Execute(IVertex vertex)
        {
            endPoints.markedToReplaceIntermediates.Enqueue(vertex);
            (vertex as IVisualizable)?.VisualizeAsMarkedToReplaceIntermediate();
        }

        public bool IsTrue(IVertex vertex)
        {
            return !vertex.IsOneOf(endPoints.Source, endPoints.Target)
                && IsIntermediate(vertex)
                && !endPoints.markedToReplaceIntermediates.Contains(vertex);
        }
    }
}