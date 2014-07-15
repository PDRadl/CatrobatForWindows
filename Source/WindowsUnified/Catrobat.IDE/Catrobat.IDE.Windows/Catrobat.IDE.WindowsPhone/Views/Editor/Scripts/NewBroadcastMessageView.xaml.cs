﻿using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.ViewModels;
using Catrobat.IDE.Core.ViewModels.Editor.Scripts;
using Windows.UI.Xaml.Controls;

namespace Catrobat.IDE.WindowsPhone.Views.Editor.Scripts
{
    public partial class NewBroadcastMessageView
    {
        private readonly NewBroadcastMessageViewModel _viewModel =
            ServiceLocator.ViewModelLocator.NewBroadcastMessageViewModel;

        protected override ViewModelBase GetViewModel() { return _viewModel; }

        public NewBroadcastMessageView()
        {
            InitializeComponent();

            ServiceLocator.DispatcherService.RunOnMainThread(() =>
            {
                //TextBoxBroadcastMessage.Focus(FocusState.Keyboard);
                TextBoxBroadcastMessage.SelectAll();
            });
        }

        private void TextBoxBroadcastMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.BroadcastMessage = new BroadcastMessage {Content = TextBoxBroadcastMessage.Text};
        }
    }
}