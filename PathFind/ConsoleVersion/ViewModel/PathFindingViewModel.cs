﻿using ConsoleVersion.InputClass;
using System;
using GraphLib.ViewModel;
using System.Threading;
using System.Linq;
using GraphLib.Coordinates;
using GraphLib.Graphs;
using Algorithm.AlgorithmCreating;
using GraphViewModel.Interfaces;
using GraphLib.Extensions;
using Common.ValueRanges;
using ConsoleVersion.Model;
using Algorithm.EventArguments;

namespace ConsoleVersion.ViewModel
{
    internal class PathFindingViewModel : PathFindingModel
    {
        public Tuple<string, string, string> Messages { get; set; }

        public PathFindingViewModel(IMainModel model) : base(model)
        {
            maxAlgorithmValue = AlgorithmFactory.AlgorithmKeys.Count();
            minAlgorithmValue = 1;
            pauseProvider.PauseEvent += () => { };
        }

        public override void FindPath()
        {
            if (!graph.Any())
                return;

            mainViewModel.ClearGraph();
            (mainViewModel as MainViewModel).DisplayGraph();
            ChooseExtremeVertex();
            (mainViewModel as MainViewModel).DisplayGraph();
            AlgorithmKey = AlgorithmFactory.AlgorithmKeys.ElementAt(GetAlgorithmKeyIndex());

            DelayTime = Input.InputNumber(
                ConsoleVersionResources.DelayTimeMsg,
                Range.DelayValueRange.UpperRange,
                Range.DelayValueRange.LowerRange);

            base.FindPath();
        }

        protected override void OnAlgorithmStarted(object sender, AlgorithmEventArgs e)
        {
            base.OnAlgorithmStarted(sender, e);

            thread = new Thread(DisplayGraphIndefinitly);
            thread.Start();
        }

        protected override void OnAlgorithmFinished(object sender, AlgorithmEventArgs e)
        {
            thread.Abort();

            base.OnAlgorithmFinished(sender, e);
        }

        private int GetAlgorithmKeyIndex()
        {
            return Input.InputNumber(
                Messages.Item3,
                maxAlgorithmValue, 
                minAlgorithmValue) - 1;
        }

        private void ChooseExtremeVertex()
        {
            var chooseMessages = new string[] { Messages.Item1, Messages.Item2 };

            for (int i = 0; i < chooseMessages.Length; i++)
            {
                var point = ChoosePoint(chooseMessages[i]);
                (mainViewModel.Graph[point] as ConsoleVertex).SetAsExtremeVertex();
            }
        }

        private Coordinate2D ChoosePoint(string message)
        {
            Console.WriteLine(message);

            var upperPosibleXValue = (graph as Graph2D).Width;
            var upperPosibleYValue = (graph as Graph2D).Length;

            var point = Input.InputPoint(upperPosibleXValue, upperPosibleYValue);

            while (!graph[point].IsValidToBeExtreme())
            {
                point = Input.InputPoint(upperPosibleXValue, upperPosibleYValue);
            }

            return point;
        }

        private void DisplayGraphIndefinitly()
        { 
            while (true)
            {
                Thread.Sleep(millisecondsTimeout: 135);
                (mainViewModel as MainViewModel).DisplayGraph();
            }
        }

        private Thread thread;

        private readonly int maxAlgorithmValue;
        private readonly int minAlgorithmValue;
    }
}
