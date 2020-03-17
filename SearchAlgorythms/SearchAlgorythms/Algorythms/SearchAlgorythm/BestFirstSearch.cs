﻿using System.Collections.Generic;
using System.Windows.Forms;
using SearchAlgorythms.Top;

namespace SearchAlgorythms.Algorythms.SearchAlgorythm
{
    public class BestFirstSearch : WideSearch
    {
        private Queue<GraphTop> waveQueue = new Queue<GraphTop>();
        private GraphTop end;

        public BestFirstSearch(GraphTop end)
        {
            this.end = end;
        }

        private void MakeWavesFromEnd(GraphTop end)
        {
            var top = end;
            MarkTop(top);
            while (!top.IsStart && waveQueue.Count != 0)
            {
                top = waveQueue.Dequeue();
                MarkTop(top);
            }
        }

        public override void DrawPath(GraphTop end)
        {
            GraphTop top = end;
            while (!top.IsEnd)
            {
                top = GoChippestNeighbour(top);
                Pause(250);
            }
        }

        public override bool IsRightNeighbour(GraphTop top1, GraphTop top2)
        {
            return top1.Value >= top2.Value && top2.IsVisited && !top2.IsStart;
        }

        public override bool IsRightCellToVisit(Button button)
        {
            GraphTop top = button as GraphTop;
            return (base.IsRightCellToVisit(top) && top?.Value != 0) || top?.IsEnd == true;
        }

        private void MarkTop(Button top)
        {
            var currentTop = top as GraphTop;
            if (currentTop is null)
                return;
            var neigbours = currentTop.GetNeighbours();
            foreach (var neigbour in neigbours)
            {
                if (neigbour.Value == 0 && !neigbour.IsEnd)
                {
                    neigbour.Value = currentTop.Value + 1;
                    waveQueue.Enqueue(neigbour);
                }
            }
        }

        public override void ExtractNeighbours(Button button)
        {
            var currentTop = button as GraphTop;
            if (currentTop is null)
                return;
            var neighbours = currentTop.GetNeighbours();
            foreach (var neigbour in neighbours)
                queue.Enqueue(neigbour);
        }

        public override void FindDestionation(GraphTop start)
        {
            MakeWavesFromEnd(end);
            base.FindDestionation(start);
        }
    }
}