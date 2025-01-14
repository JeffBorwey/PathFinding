﻿using GraphViewModel.Interfaces;
using System;
using System.Windows;
using WPFVersion.Attributes;

namespace WPFVersion
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    [AppWindow]
    public partial class MainWindow : Window
    {
        public MainWindow(IMainModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            (DataContext as IDisposable)?.Dispose();
            base.OnClosed(e);
        }
    }
}
