﻿using GraphLib.Base;
using GraphLib.Interface;
using System;
using System.Windows.Forms;

namespace WindowsFormsVersion.Model
{
    internal sealed class EndPoints : BaseEndPoints
    {
        protected override void SubscribeVertex(IVertex vertex)
        {
            (vertex as Vertex).MouseClick += SetEndPoints;
        }

        public override void SetEndPoints(object sender, EventArgs e)
        {
            if ((e as MouseEventArgs).Button == MouseButtons.Left)
            {
                base.SetEndPoints(sender, e);
            }
        }
    }
}
