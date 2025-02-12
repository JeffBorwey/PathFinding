﻿using GraphLib.Base.EndPoints;
using GraphLib.Interfaces;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsVersion.Model
{
    [DebuggerDisplay("Source - {Source}, Target - {Target}")]
    internal sealed class EndPoints : BaseEndPoints, IEndPoints
    {
        protected override void SubscribeVertex(IVertex vertex)
        {
            if (vertex is Vertex vert)
            {
                vert.MouseClick += SetEndPoints;
                vert.MouseClick += MarkIntermediateToReplace;
            }
        }

        protected override void SetEndPoints(object sender, EventArgs e)
        {
            if (e is MouseEventArgs args && args.Button == MouseButtons.Left)
            {
                base.SetEndPoints(sender, e);
            }
        }

        protected override void UnsubscribeVertex(IVertex vertex)
        {
            if (vertex is Vertex vert)
            {
                vert.MouseClick -= SetEndPoints;
                vert.MouseClick -= MarkIntermediateToReplace;
            }
        }

        protected override void MarkIntermediateToReplace(object sender, EventArgs e)
        {
            if (e is MouseEventArgs args && args.Button == MouseButtons.Middle)
            {
                base.MarkIntermediateToReplace(sender, e);
            }
        }
    }
}
