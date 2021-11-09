﻿using Algorithm.Factory;
using Common.Interface;
using EnumerationValues.Realizations;
using GalaSoft.MvvmLight.Messaging;
using GraphLib.Base;
using GraphLib.Extensions;
using GraphLib.Interfaces;
using GraphLib.Interfaces.Factories;
using GraphViewModel;
using GraphViewModel.Interfaces;
using Logging.Interface;
using NullObject.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPFVersion3D.Axes;
using WPFVersion3D.Enums;
using WPFVersion3D.Extensions;
using WPFVersion3D.Infrastructure;
using WPFVersion3D.Interface;
using WPFVersion3D.Messages;
using WPFVersion3D.Model;
using WPFVersion3D.View;

namespace WPFVersion3D.ViewModel
{
    internal class MainWindowViewModel : MainModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string graphParametres;
        public override string GraphParametres { get => graphParametres; set { graphParametres = value; OnPropertyChanged(); } }

        private IGraphField graphField;
        public override IGraphField GraphField { get => graphField; set { graphField = value; OnPropertyChanged(); } }

        private bool IsAllAlgorithmsFinished { get; set; } = true;

        public Tuple<string, IAnimationSpeed>[] AnimationSpeeds { get; }

        public ICommand StartPathFindCommand { get; }
        public ICommand CreateNewGraphCommand { get; }
        public ICommand ClearGraphCommand { get; }
        public ICommand SaveGraphCommand { get; }
        public ICommand LoadGraphCommand { get; }
        public ICommand ChangeOpacityCommand { get; }
        public ICommand AnimatedAxisRotateCommand { get; }
        public ICommand InterruptAlgorithmCommand { get; }
        public ICommand ClearVerticesColorCommand { get; }

        public MainWindowViewModel(IGraphFieldFactory fieldFactory,
            IVertexEventHolder eventHolder,
            ISaveLoadGraph saveLoad,
            IEnumerable<IGraphAssemble> graphAssembles,
            BaseEndPoints endPoints,
            IEnumerable<IAlgorithmFactory> algorithmFactories,
            ILog log)
            : base(fieldFactory, eventHolder, saveLoad, graphAssembles, endPoints, algorithmFactories, log)
        {
            ClearVerticesColorCommand = new RelayCommand(ExecuteClearVerticesColors, CanExecuteClearGraphOperation);
            StartPathFindCommand = new RelayCommand(ExecuteStartPathFindCommand, CanExecuteStartFindPathCommand);
            CreateNewGraphCommand = new RelayCommand(ExecuteCreateNewGraphCommand, CanExecuteOperation);
            ClearGraphCommand = new RelayCommand(ExecuteClearGraphCommand, CanExecuteClearGraphOperation);
            SaveGraphCommand = new RelayCommand(ExecuteSaveGraphCommand, CanExecuteGraphOperation);
            LoadGraphCommand = new RelayCommand(ExecuteLoadGraphCommand, CanExecuteOperation);
            ChangeOpacityCommand = new RelayCommand(ExecuteChangeOpacity, CanExecuteGraphOperation);
            AnimatedAxisRotateCommand = new RelayCommand(ExecuteAnimatedAxisRotateCommand);
            InterruptAlgorithmCommand = new RelayCommand(ExecuteInterruptAlgorithmCommand, CanExecuteInterruptAlgorithmCommand);
            AnimationSpeeds = new EnumValuesWithoutIgnored<AnimationSpeeds>().ToAnimationSpeedTuples();
            Messenger.Default.Register<IsAllAlgorithmsFinishedMessage>(this, MessageTokens.MainModel, OnIsAllAlgorithmsFinished);
            Messenger.Default.Register<GraphCreatedMessage>(this, MessageTokens.MainModel, SetGraph);
        }

        public override void FindPath()
        {
            try
            {
                var viewModel = new PathFindingViewModel(log, Graph, endPoints, algorithmFactories);
                var window = new PathFindWindow();
                PrepareWindow(viewModel, window);
            }
            catch (SystemException ex)
            {
                log.Warn(ex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public override void CreateNewGraph()
        {
            try
            {
                var model = new GraphCreatingViewModel(log, graphAssembles);
                var window = new GraphCreateWindow();
                PrepareWindow(model, window);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public override void ConnectNewGraph(IGraph graph)
        {
            base.ConnectNewGraph(graph);
            Messenger.Default.Send(new ClearStatisticsMessage(), MessageTokens.AlgorithmStatisticsModel);
            (graphField as GraphField3D)?.CenterGraph();
        }

        public void StretchAlongXAxis(object sender, RoutedPropertyChangedEventArgs<double> e)
            => (GraphField as GraphField3D)?.StretchAlongAxis(new Abscissa(), e.NewValue, 1, 0, 0);

        public void StretchAlongYAxis(object sender, RoutedPropertyChangedEventArgs<double> e)
            => (GraphField as GraphField3D)?.StretchAlongAxis(new Ordinate(), e.NewValue, 0, 1, 0);

        public void StretchAlongZAxis(object sender, RoutedPropertyChangedEventArgs<double> e)
            => (GraphField as GraphField3D)?.StretchAlongAxis(new Applicate(), e.NewValue, 0, 0, 1);

        private void ChangeVerticesOpacity()
        {
            var model = new OpacityChangeViewModel();
            var window = new OpacityChangeWindow();
            PrepareWindow(model, window);
        }

        private void ExecuteClearVerticesColors(object param) => ClearColors();
        private void ExecuteSaveGraphCommand(object param) => base.SaveGraph();
        private void ExecuteChangeOpacity(object param) => ChangeVerticesOpacity();
        private void ExecuteLoadGraphCommand(object param) => base.LoadGraph();
        private void ExecuteStartPathFindCommand(object param) => FindPath();
        private void ExecuteCreateNewGraphCommand(object param) => CreateNewGraph();
        private void ExecuteAnimatedAxisRotateCommand(object param) => (param as IAnimatedAxisRotator)?.RotateAxis();

        private void ExecuteClearGraphCommand(object param)
        {
            base.ClearGraph();
            var message = new ClearStatisticsMessage();
            Messenger.Default.Send(message, MessageTokens.AlgorithmStatisticsModel);
        }

        private void ExecuteInterruptAlgorithmCommand(object param)
        {
            var message = new InterruptAllAlgorithmsMessage();
            Messenger.Default.Send(message, MessageTokens.AlgorithmStatisticsModel);
        }

        private bool CanExecuteStartFindPathCommand(object param) => !endPoints.HasIsolators();
        private bool CanExecuteClearGraphOperation(object param) => CanExecuteGraphOperation(param) && CanExecuteOperation(param);
        private bool CanExecuteGraphOperation(object param) => !Graph.IsNull();
        private bool CanExecuteOperation(object param) => IsAllAlgorithmsFinished;
        private bool CanExecuteInterruptAlgorithmCommand(object sender) => !IsAllAlgorithmsFinished;

        private void PrepareWindow(IViewModel model, Window window)
        {
            model.WindowClosed += (sender, args) => window.Close();
            window.DataContext = model;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }

        private void OnIsAllAlgorithmsFinished(IsAllAlgorithmsFinishedMessage message)
            => IsAllAlgorithmsFinished = message.IsAllAlgorithmsFinished;
        private void SetGraph(GraphCreatedMessage message) => ConnectNewGraph(message.Graph);
    }
}