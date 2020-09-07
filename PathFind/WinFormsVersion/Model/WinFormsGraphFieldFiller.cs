﻿using GraphLibrary.GraphCreate.GraphFieldFiller;
using GraphLibrary.Model;
using System.Drawing;

namespace WinFormsVersion.Model
{
    internal class WinFormsGraphFieldFiller : AbstractGraphFieldFiller
    {
        protected override IGraphField GetField()
        {
            return new WinFormsGraphField() { Location = new Point(4, 90) };
        }
    }
}