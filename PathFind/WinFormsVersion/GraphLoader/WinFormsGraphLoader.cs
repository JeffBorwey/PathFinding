﻿using System.Windows.Forms;
using GraphLibrary;
using GraphLibrary.Graph;
using GraphLibrary.GraphFactory;
using GraphLibrary.GraphLoader;
using WinFormsVersion.GraphFactory;

namespace WinFormsVersion.GraphLoader
{
    public class WinFormsGraphLoader : AbstractGraphLoader
    {
        private readonly int placeBetweenButtons;

        public WinFormsGraphLoader(int placeBetweenButtons) => this.placeBetweenButtons = placeBetweenButtons;

        protected override AbstractGraph CreateGraph(AbstractGraphInitializer initializer) => initializer.GetGraph();

        protected override AbstractGraphInitializer GetInitializer(VertexInfo[,] info) 
            => new WinFormsGraphInitializer(info, placeBetweenButtons);

        protected override string GetPath()
        {
            var open = new OpenFileDialog();
            return open.ShowDialog() == DialogResult.OK ? open.FileName : "";
        }

        protected override void ShowMessage(string message) => MessageBox.Show(message);
    }
}
