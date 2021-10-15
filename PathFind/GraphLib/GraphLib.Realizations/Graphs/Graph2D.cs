﻿using Common.Interface;
using GraphLib.Base;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using System.Linq;

namespace GraphLib.Realizations.Graphs
{
    public sealed class Graph2D : BaseGraph, IGraph, ICloneable<IGraph>
    {
        public int Width { get; }

        public int Length { get; }

        public Graph2D(params int[] dimensions)
            : base(numberOfDimensions: 2, dimensions)
        {
            Width = DimensionsSizes.First();
            Length = DimensionsSizes.Last();
        }

        public override IGraph Clone()
        {
            var graph = new Graph2D(DimensionsSizes);
            return graph.CopyVerticesDeeply(this);
        }
    }
}