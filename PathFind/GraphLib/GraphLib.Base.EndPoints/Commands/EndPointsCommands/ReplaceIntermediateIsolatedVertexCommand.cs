﻿using Common.Attrbiutes;
using GraphLib.Base.EndPoints.Attributes;
using GraphLib.Base.EndPoints.BaseCommands;
using GraphLib.Base.EndPoints.Commands.VerticesCommands;
using GraphLib.Extensions;
using GraphLib.Interfaces;

namespace GraphLib.Base.EndPoints.Commands.EndPointsCommands
{
    [AttachedTo(typeof(SetEndPointsCommands)), Order(3)]
    internal sealed class ReplaceIntermediateIsolatedVertexCommand : BaseIntermediateEndPointsCommand
    {
        public ReplaceIntermediateIsolatedVertexCommand(BaseEndPoints endPoints)
            : base(endPoints)
        {
        }

        public override bool IsTrue(IVertex vertex)
        {
            return endPoints.HasSourceAndTargetSet()
                && HasIsolatedIntermediates
                && endPoints.CanBeEndPoint(vertex);
        }

        public override void Execute(IVertex vertex)
        {
            var isolated = Intermediates.FirstOrNullVertex(v => v.IsIsolated());
            int isolatedIndex = Intermediates.IndexOf(isolated);
            Intermediates.Remove(isolated);
            MarkedToReplace.Remove(isolated);
            isolated.AsVisualizable().VisualizeAsRegular();
            Intermediates.Insert(isolatedIndex, vertex);
            vertex.AsVisualizable().VisualizeAsIntermediate();
        }
    }
}
