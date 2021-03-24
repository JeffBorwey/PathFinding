﻿using ConsoleVersion.InputClass;
using ConsoleVersion.Resource;
using GraphLib.Base;
using GraphLib.Interface;
using System;

namespace ConsoleVersion.Model
{
    internal class VertexEventHolder : BaseVertexEventHolder
    {
        public override void ChangeVertexCost(object sender, EventArgs e)
        {
            var vertex = sender as Vertex;

            if (!vertex.IsObstacle)
            {
                var cost = Input.InputNumber(Resources.VertexCostInputMsg, BaseVertexCost.CostRange);

                vertex.Cost = CostFactory.CreateCost(cost);
            }
        }

        protected override int GetWheelDelta(EventArgs e)
        {
            return 0;
        }

        protected override void SubscribeToEvents(IVertex vertex)
        {
            (vertex as Vertex).OnCostChanged += ChangeVertexCost;
            (vertex as Vertex).OnReverse += Reverse;
        }

        protected override void UnsubscribeFromEvents(IVertex vertex)
        {
            (vertex as Vertex).OnCostChanged -= ChangeVertexCost;
            (vertex as Vertex).OnReverse -= Reverse;
        }
    }
}