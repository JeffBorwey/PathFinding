﻿using GraphLib.Base.EndPointsInspection.Abstractions;
using GraphLib.Base.VertexCondition.Interface;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using GraphLib.NullRealizations.NullObjects;

namespace GraphLib.Base.VertexCondition.Realizations.EndPointsConditions
{
    internal sealed class UnsetSourceVertexCondition
        : BaseEndPointsInspection, IVertexCondition
    {
        public UnsetSourceVertexCondition(BaseEndPoints endPoints)
            : base(endPoints)
        {

        }

        public bool IsTrue(IVertex vertex)
        {
            return vertex.IsEqual(endPoints.Source);
        }

        public void Execute(IVertex vertex)
        {
            (vertex as IVisualizable)?.VisualizeAsRegular();
            endPoints.Source = new NullVertex();
        }
    }
}