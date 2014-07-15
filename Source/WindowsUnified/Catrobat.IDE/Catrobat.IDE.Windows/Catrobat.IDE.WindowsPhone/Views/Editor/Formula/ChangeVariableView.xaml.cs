﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.ViewModels;
using Catrobat.IDE.Core.ViewModels.Editor.Formula;

namespace Catrobat.IDE.WindowsPhone.Views.Editor.Formula
{
    public partial class ChangeVariableView
    {
        private readonly ChangeVariableViewModel _viewModel = 
            ServiceLocator.ViewModelLocator.ChangeVariableViewModel;

        protected override ViewModelBase GetViewModel() { return _viewModel; }

        public ChangeVariableView()
        {
            InitializeComponent();

            ServiceLocator.DispatcherService.RunOnMainThread(() =>
            {
                //TextBoxVariableName.Focus(FocusState.Keyboard);
                TextBoxVariableName.SelectAll();
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _viewModel.InitializeCommand.Execute(null);
            base.OnNavigatedTo(e);
        }

        private void TextBoxVariableName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.UserVariableName = TextBoxVariableName.Text;
        }
    }
}