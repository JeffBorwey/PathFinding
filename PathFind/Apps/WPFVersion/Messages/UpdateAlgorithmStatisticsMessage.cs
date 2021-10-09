﻿namespace WPFVersion.Messages
{
    internal sealed class UpdateAlgorithmStatisticsMessage
    {
        public int Index { get; }
        public string Time { get; }
        public int VisitedVertices { get; }
        public double PathCost { get; }
        public int PathLength { get; }

        public UpdateAlgorithmStatisticsMessage(int index,
            string time,
            int visitedVertices,
            int pathLength = 0,
            double pathCost = 0)
        {
            Index = index;
            Time = time;
            VisitedVertices = visitedVertices;
            PathCost = pathCost;
            PathLength = pathLength;
        }
    }
}