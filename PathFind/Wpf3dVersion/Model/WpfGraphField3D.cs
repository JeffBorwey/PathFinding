﻿using GraphLib.GraphField;
using GraphLib.Vertex.Interface;
using System.Linq;
using System.Windows.Media.Media3D;
using OffsetAction = System.Action<System.Windows.Media.Media3D.TranslateTransform3D, double>;

namespace Wpf3dVersion.Model
{
    public class WpfGraphField3D : ModelVisual3D, IGraphField
    {
        public double DistanceBetweenVerticesAtXAxis { get; set; }

        public double DistanceBetweenVerticesAtYAxis { get; set; }

        public double DistanceBetweenVerticesAtZAxis { get; set; }

        public WpfGraphField3D(int width, 
            int length, int height) : this()
        {
            Width = width;
            Length = length;
            Height = height;
        }

        public WpfGraphField3D()
        {
            DistanceBetweenVerticesAtXAxis = 0;
            DistanceBetweenVerticesAtYAxis = 0;
            DistanceBetweenVerticesAtZAxis = 0;
        }

        public void Add(IVertex vertex)
        {
            WpfVertex3D vertex3D = vertex as WpfVertex3D;
            SetVertexOffset(vertex3D);
            Children.Add(vertex3D);
        }

        public void SetDistanceBetweenVertices()
        {
            foreach (WpfVertex3D vertex in Children)
            {
                SetVertexOffset(vertex);
            }
        }

        public void CenterGraph(params double[] additionalOffset)
        {
            var dimensionSizes = new int[] { Width, Length, Height };
            var centerOffsets = new double[dimensionSizes.Length];           
            foreach (WpfVertex3D vertex in Children)
            {
                for (int i = 0; i < dimensionSizes.Length; i++)
                {
                    var offsetCalculate = new OffsetCalculate
                    {
                        DimensionSize = dimensionSizes.ElementAtOrDefault(i),
                        VertexSize = vertex.Size,
                        AdditionalOffset = additionalOffset.ElementAtOrDefault(i),
                        DistanceBetweenVertices = DistanceBetween.ElementAtOrDefault(i)
                    };
                    centerOffsets[i] = offsetCalculate.CalculateCenterOffset();
                }
                SetVertexOffset(vertex, centerOffsets);
            }
        }

        private void SetVertexOffset(WpfVertex3D vertex, 
            params double[] additionalOffset)
        {            
            var coordinates = vertex.Position.Coordinates;
            var translate = new TranslateTransform3D();
            for (int i = 0; i < coordinates.Count(); i++)
            {
                var offsetCalculate = new OffsetCalculate
                {
                    CoordinateValue = coordinates.ElementAtOrDefault(i),
                    VertexSize = vertex.Size,
                    DistanceBetweenVertices = DistanceBetween.ElementAtOrDefault(i),
                    AdditionalOffset = additionalOffset.ElementAtOrDefault(i)
                };
                var offset = offsetCalculate.CalculateVertexOffset();
                OffsetActions[i](translate, offset);
            }
            vertex.Transform = translate;
        }

        private int Width { get; set; }

        private int Length { get; set; }

        private int Height { get; set; }

        private double[] DistanceBetween => new double[] 
        { 
            DistanceBetweenVerticesAtXAxis, 
            DistanceBetweenVerticesAtYAxis, 
            DistanceBetweenVerticesAtZAxis 
        };

        private OffsetAction[] OffsetActions => new OffsetAction[]
        {
            (transform, offset) => { transform.OffsetX = offset; },
            (transform, offset) => { transform.OffsetY = offset; },
            (transform, offset) => { transform.OffsetZ = offset; }
        };
    }

    class OffsetCalculate
    {
        public int CoordinateValue { private get; set; }

        public double VertexSize { private get; set; }

        public double AdditionalOffset { private get; set; }

        public int DimensionSize { private get; set; }

        public double DistanceBetweenVertices { private get; set; }

        public double CalculateCenterOffset()
        {
            return (-DimensionSize + AdditionalOffset) 
                * (VertexSize + DistanceBetweenVertices) / 2;
        }

        public double CalculateVertexOffset()
        {
            return (VertexSize + DistanceBetweenVertices)
                * CoordinateValue + AdditionalOffset;
        }
    }
}