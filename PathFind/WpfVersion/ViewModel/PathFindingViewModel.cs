﻿using Algorithm.AlgorithmCreating;
using Algorithm.EventArguments;
using GraphLib.ViewModel;
using GraphViewModel.Interfaces;
using System.Linq;
using System.Windows;
using WpfVersion.Infrastructure;

namespace WpfVersion.ViewModel
{
    internal class PathFindingViewModel : PathFindingModel
    {
        public RelayCommand ConfirmPathFindAlgorithmChoice { get; }
        public RelayCommand CancelPathFindAlgorithmChoice { get; }

        public PathFindingViewModel(IMainModel model) : base(model)
        {
            ConfirmPathFindAlgorithmChoice = new RelayCommand(
                ExecuteConfirmPathFindAlgorithmChoice,
                CanExecuteConfirmPathFindAlgorithmChoice);

            CancelPathFindAlgorithmChoice = new RelayCommand(obj => CloseWindow(), obj => true);
            pauseProvider.PauseEvent += () => System.Windows.Forms.Application.DoEvents();
        }

        private void CloseWindow()
        {
            (mainViewModel as MainWindowViewModel)?.Window?.Close();
        }

        private void ExecuteConfirmPathFindAlgorithmChoice(object param)
        {
            CloseWindow();
            base.FindPath();
        }

        private bool CanExecuteConfirmPathFindAlgorithmChoice(object param)
        {
            return AlgorithmFactory.AlgorithmKeys.Any(algo => algo == AlgorithmKey);
        }
    }
}
