﻿using Common.Interface;
using GalaSoft.MvvmLight.Messaging;
using GraphLib.Extensions;
using GraphLib.Interfaces.Factories;
using GraphLib.ViewModel;
using GraphViewModel.Interfaces;
using Logging.Interface;
using System;
using System.Collections.Generic;
using WindowsFormsVersion.Messeges;

namespace WindowsFormsVersion.ViewModel
{
    internal class GraphCreatingViewModel : GraphCreatingModel, IModel, IViewModel
    {
        public event EventHandler WindowClosed;

        public GraphCreatingViewModel(ILog log, IEnumerable<IGraphAssemble> graphAssembles)
            : base(log, graphAssembles)
        {

        }

        public override async void CreateGraph()
        {
            try
            {
                var graph = await SelectedGraphAssemble.AssembleGraphAsync(ObstaclePercent, GraphParametres);
                var message = new GraphCreatedMessage(graph);
                Messenger.Default.Send(message, MessageTokens.MainModel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void CreateGraph(object sender, EventArgs e)
        {
            if (CanExecuteConfirmGraphAssembleChoice())
            {
                CreateGraph();
                WindowClosed?.Invoke(this, EventArgs.Empty);
                WindowClosed = null;
            }
        }

        public void CancelCreateGraph(object sender, EventArgs e)
        {
            WindowClosed?.Invoke(this, EventArgs.Empty);
            WindowClosed = null;
        }

        private bool CanExecuteConfirmGraphAssembleChoice()
        {
            return SelectedGraphAssemble != null
                && Constants.GraphWidthValueRange.Contains(Width)
                && Constants.GraphLengthValueRange.Contains(Length);
        }
    }
}
