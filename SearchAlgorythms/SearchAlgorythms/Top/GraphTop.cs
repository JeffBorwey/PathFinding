﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SearchAlgorythms.Top
{
    public class GraphTop : Button
    {
        private List<GraphTop> neighbours = new List<GraphTop>();

        public GraphTop()
        {
            SetToDefault();
        }

        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsVisited { get; set; }
        public int Value { get; set; }

        public void SetToDefault()
        {
            IsStart = false;
            IsEnd = false;
            IsVisited = false;
            Value = 0;
            BackColor = Color.FromName("White");
        }

        public void AddNeighbour(GraphTop top)
        {
            neighbours.Add(top);
        }

        public List<GraphTop> GetNeighbours()
        {
            return neighbours;
        }
    }
}
