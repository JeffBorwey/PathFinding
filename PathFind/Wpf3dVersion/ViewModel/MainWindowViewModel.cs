﻿using GraphLib.GraphField;
using GraphLib.Vertex;
using GraphViewModel;
using GraphViewModel.Interfaces;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Media3D;
using Wpf3dVersion.Infrastructure;
using Wpf3dVersion.Model;
using Wpf3dVersion.Resources;
using Wpf3dVersion.View;

namespace Wpf3dVersion.ViewModel
{
    internal class MainWindowViewModel : MainModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Window Window { get; set; }

        private string graphParametres;
        public override string GraphParametres
        {
            get { return graphParametres; }
            set { graphParametres = value; OnPropertyChanged(); }
        }

        private string statistics;
        public override string PathFindingStatistics
        {
            get { return statistics; }
            set { statistics = value; OnPropertyChanged(); }
        }

        private IGraphField graphField;
        public override IGraphField GraphField
        {
            get { return graphField; }
            set
            {
                graphField = value;
                var currentWindow = (Application.Current.MainWindow as MainWindow);
                currentWindow?.GraphField?.Children?.Clear();
                currentWindow?.GraphField?.Children?.Add(graphField as ModelVisual3D);
                OnPropertyChanged();
            }
        }

        public RelayCommand StartPathFindCommand { get; }
        public RelayCommand CreateNewGraphCommand { get; }
        public RelayCommand ClearGraphCommand { get; }
        public RelayCommand SaveGraphCommand { get; }
        public RelayCommand LoadGraphCommand { get; }
        public RelayCommand ChangeOpacityCommand { get; }

        public MainWindowViewModel()
        {
            GraphField = new Wpf3dGraphField();
            VertexEventHolder = new Wpf3dVertexEventHolder();
            FieldFactory = new Wpf3DGraphFieldFactory();
            DtoConverter = (dto) => new Wpf3dVertex(dto);

            StartPathFindCommand = new RelayCommand(ExecuteStartPathFindCommand, CanExecuteStartFindPathCommand);
            CreateNewGraphCommand = new RelayCommand(ExecuteCreateNewGraphCommand, AlwaysExecutable);
            ClearGraphCommand = new RelayCommand(ExecuteClearGraphCommand, CanExecuteGraphOperation);
            SaveGraphCommand = new RelayCommand(ExecuteSaveGraphCommand, CanExecuteGraphOperation);
            LoadGraphCommand = new RelayCommand(ExecuteLoadGraphCommand, AlwaysExecutable);           
            ChangeOpacityCommand = new RelayCommand(ExecuteChangeOpacity, CanExecuteGraphOperation);

            graphParamFormat = Resource.GraphParamFormat;
        }

        private bool AlwaysExecutable(object param) => true;

        private bool CanExecuteGraphOperation(object param) => !Graph.IsDefault;

        public override void FindPath()
        {
            PrepareWindow(new PathFindingViewModel(this), new PathFindWindow());
        }

        public override void CreateNewGraph()
        {
            PrepareWindow(new GraphCreatingViewModel(this), new GraphCreateWindow());
        }

        private void AxisValueChanged(Action<double, Wpf3dGraphField> func, double sliderNewValue)
        {
            var field = graphField as Wpf3dGraphField;
            func(sliderNewValue, field);
            field.SetDistanceBetweenVertices(Graph);
            field.CenterGraph(Graph);
        }

        public void XAxisSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AxisValueChanged((sliderValue, field) => field.DistanceBetweenAtXAxis = sliderValue, e.NewValue);
        }

        public void YAxisSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AxisValueChanged((sliderValue, field) => field.DistanceBetweenAtYAxis = sliderValue, e.NewValue);
        }

        public void ZAxisSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AxisValueChanged((sliderValue, field) => field.DistanceBetweenAtZAxis = sliderValue, e.NewValue);
        }

        private void ChangeVerticesOpacity()
        {
            PrepareWindow(new OpacityChangeViewModel(this), new OpacityChangeWindow());
        }

        public void Dispose() => OnDispose();

        private void ExecuteSaveGraphCommand(object param) => base.SaveGraph();
        private void ExecuteChangeOpacity(object param) => ChangeVerticesOpacity();

        private bool CanExecuteStartFindPathCommand(object param)
        {
            return !ReferenceEquals(Graph.End, new DefaultVertex())
                && !ReferenceEquals(Graph.Start, new DefaultVertex())
                && Graph.Any() && !Graph.Start.IsVisited;
        }

        private void ExecuteLoadGraphCommand(object param)
        {
            base.LoadGraph();
        }

        private void ExecuteClearGraphCommand(object param) => base.ClearGraph();

        private void ExecuteStartPathFindCommand(object param)
        {
            FindPath();
        }

        private void ExecuteCreateNewGraphCommand(object param)
        {
            CreateNewGraph();            
        }

        protected virtual void OnDispose()
        {
            return;
        }

        private void PrepareWindow(IModel model, Window window)
        {
            Window = window;
            Window.DataContext = model;
            Window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Window.Show();
        }

        private string GetPath(FileDialog dialog)
        {
            return dialog.ShowDialog() == true
                ? dialog.FileName
                : string.Empty;
        }

        protected override string GetSavingPath() => GetPath(new SaveFileDialog());
        protected override string GetLoadingPath() => GetPath(new OpenFileDialog());
    }
}