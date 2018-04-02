﻿using Sakuno.KanColle.Amatsukaze.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sakuno.KanColle.Amatsukaze.UWP
{
    internal sealed partial class MainView : Page
    {
        private readonly MainWindowVM ViewModel;
        private readonly Shell Shell;
        public MainView(MainWindowVM viewModel, Shell shell)
        {
            ViewModel = viewModel;
            Shell = shell;
            this.InitializeComponent();
            Window.Current.SetTitleBar(DraggableTitle);
        }
    }
}
