﻿using GraphLibrary.Constants;
using GraphLibrary.Graph;
using GraphLibrary.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WinFormsVersion.Forms;
using WinFormsVersion.GraphLoader;
using WinFormsVersion.GraphSaver;
using WinFormsVersion.Model;
using WinFormsVersion.View;

namespace WinFormsVersion.ViewModel
{
    public class MainWindowViewModel : AbstractMainModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string graphParametres;
        public override string GraphParametres
        {
            get { return graphParametres; }
            set { graphParametres = value; OnPropertyChanged(); }
        }

        private string statistics;
        public override string Statistics
        {
            get { return statistics; }
            set { statistics = value; OnPropertyChanged(); }
        }

        private IGraphField graphField;
        public override IGraphField GraphField
        {
            get { return graphField; }
            set { graphField = value; OnPropertyChanged(); }
        }

        private AbstractGraph graph;
        public override AbstractGraph Graph
        {
            get { return graph; }
            set { graph = value; OnPropertyChanged(); }
        }
        public Form Window { get; set; }
        public MainWindow MainWindow { get; set; }

        public MainWindowViewModel()
        {
            GraphField = new WinFormsGraphField();
            Format = "Time: {0}:{1}.{2}\nSteps: {3}\nPath length: {4}\nVisited vertices: {5}";
            saver = new WinFormsGraphSaver();
            loader = new WinFormsGraphLoader(Const.SIZE_BETWEEN_VERTICES);
            filler = new WinFormsGraphFiller();
        }

        public override void PathFind()
        {
            var model = new PathFindViewModel(this);
            var form = new PathFindWindow(model);
            PrepareWindow(form);
        }

        public override void CreateNewGraph()
        {
            var model = new CreateGraphViewModel(this);
            var form = new CreateGraphWindow(model);
            PrepareWindow(form);
        }

        public void Dispose()
        {
            OnDispose();
        }

        public void SaveGraph(object sender, EventArgs e)
        {
            base.SaveGraph();
        }

        public void LoadGraph(object sender, EventArgs e)
        {
            base.LoadGraph();
            OnPropertyChanged(nameof(GraphField));
            OnPropertyChanged(nameof(Graph));
            OnPropertyChanged(nameof(GraphParametres));
        }

        public void ClearGraph(object sender, EventArgs e)
        {
            base.ClearGraph();
        }

        public void StartPathFind(object sender, EventArgs e)
        {
            PathFind();
        }

        public void CreateNewGraph(object sender, EventArgs e)
        {
            CreateNewGraph();

        }

        protected virtual void OnDispose()
        {
            return;
        }

        private void PrepareWindow(Form window)
        {
            Window = window;
            Window.Show();
        }
    }
}
