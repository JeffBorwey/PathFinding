﻿using GraphLibrary.Graph;

namespace GraphLibrary
{
    public static class GraphDataFormatter
    {
        public static string GetFormattedData(AbstractGraph graph, string format)
        {
            return string.Format(format,
               graph.Width,
               graph.Height,
               graph.ObstaclePercent,
               graph.ObstacleNumber,
               graph.Size);
        }
    }
}