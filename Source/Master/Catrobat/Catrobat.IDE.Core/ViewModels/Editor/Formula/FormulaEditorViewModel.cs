﻿using System.Collections.ObjectModel;
using Catrobat.IDE.Core.CatrobatObjects;
using Catrobat.IDE.Core.Formulas;
using Catrobat.IDE.Core.Formulas.Editor;
using Catrobat.IDE.Core.Models.Formulas.FormulaToken;
using Catrobat.IDE.Core.Models.Formulas.FormulaTree;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.UI;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Catrobat.IDE.Core.ViewModels.Editor.Formula
{
    public delegate void ErrorOccurred();
    public delegate void Reset();
    public delegate void Evaluated(object value);

    public class FormulaEditorViewModel : ViewModelBase
    {
        #region Events

        public event ErrorOccurred ErrorOccurred;
        private void RaiseKeyError()
        {
            if (ErrorOccurred != null)
                ErrorOccurred.Invoke();
        }

        public event Reset Reset;
        private void RaiseReset()
        {
            if (Reset != null)
                Reset.Invoke();
        }

        #endregion

        #region Members

        private readonly FormulaEditor _editor = new FormulaEditor();
        private Sprite _selectedSprite;
        private Project _currentProject;
        private VariableConteiner _variableContainer;
        
        #endregion

        #region Properties

        public Project CurrentProject
        {
            get { return _currentProject; }
            private set
            {
                _currentProject = value; 
                RaisePropertyChanged(() => CurrentProject);
            }
        }

        public Sprite SelectedSprite
        {
            get { return _selectedSprite; }
            set
            {
                _selectedSprite = value;
                RaisePropertyChanged(() => SelectedSprite);
            }
        }

        public IFormulaTree Formula
        {
            get { return _editor.Formula; }
            set { _editor.Formula = value; }
        }

        public ObservableCollection<IFormulaToken> Tokens
        {
            get { return _editor.Tokens; }
        }

        public bool IsTokensEmpty
        {
            get { return _editor.IsTokensEmpty; }
        }

        public int CaretIndex
        {
            get { return _editor.CaretIndex; }
            set { _editor.CaretIndex = value; }
        }

        public int SelectionStart
        {
            get { return _editor.SelectionStart; }
            set { _editor.SelectionStart = value; }
        }

        public int SelectionLength
        {
            get { return _editor.SelectionLength; }
            set { _editor.SelectionLength = value; }
        }

        public ParsingError ParsingError
        {
            get { return _editor.ParsingError; }
        }

        public bool CanDelete
        {
            get { return _editor.CanDelete; }
        }

        public bool CanUndo
        {
            get { return _editor.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _editor.CanRedo; }
        }

        public bool HasError
        {
            get { return _editor.HasError; }
        }

        public bool CanEvaluate
        {
            get { return !HasError; }
        }

        #endregion

        #region Commands

        public RelayCommand<FormulaKeyEventArgs> KeyPressedCommand { get; private set; }
        private void KeyPressedAction(FormulaKeyEventArgs e)
        {
            if (!_editor.HandleKey(e.Key, e.UserVariable)) RaiseKeyError();
        }

        public RelayCommand EvaluatePressedCommand { get; private set; }

        private void EvaluatePressedAction()
        {
            var value = FormulaEvaluator.Evaluate(Formula);
            var message = value == null ? string.Empty : value.ToString();
            ServiceLocator.NotifictionService.ShowToastNotification("", message, ToastNotificationTime.Medeum);
        }

        public RelayCommand ShowErrorPressedCommand { get; private set; }

        private void ShowErrorPressedAction()
        {
            // TODO: pretty up toast notification
            CaretIndex = ParsingError.Index;
            SelectionLength = ParsingError.Length;
            ServiceLocator.NotifictionService.ShowToastNotification(
                title: "",
                message: ParsingError.Message,
                timeTillHide: ToastNotificationTime.Medeum);
        }

        public RelayCommand UndoCommand { get; private set; }
        private void UndoAction()
        {
            if (!_editor.Undo()) RaiseKeyError();
        }

        public RelayCommand RedoCommand { get; private set; }
        private void RedoAction()
        {
            if (!_editor.Redo()) RaiseKeyError();
        }

        public RelayCommand StartSensorsCommand { get; private set; }
        private void StartSensorsAction()
        {
            ServiceLocator.SensorService.Start();
        }

        public RelayCommand StopSensorsCommand { get; private set; }
        private void StopSensorsAction()
        {
            ServiceLocator.SensorService.Stop();
        }

        public RelayCommand<int> CompleteTokenCommand { get; private set; }
        private void CompleteTokenAction(int index)
        {
            var selection = FormulaInterpreter.Complete(Tokens, index);
            SelectionStart = selection.Start;
            SelectionLength = selection.Length;
        }

        #endregion

        #region MessageActions

        private void CurrentProjectChangedMessageAction(GenericMessage<Project> message)
        {
            CurrentProject = message.Content;
        }

        private void SelectedSpriteChangedMessageAction(GenericMessage<Sprite> message)
        {
            SelectedSprite = message.Content;
        }

        #endregion

        public FormulaEditorViewModel()
        {
            KeyPressedCommand = new RelayCommand<FormulaKeyEventArgs>(KeyPressedAction);
            EvaluatePressedCommand = new RelayCommand(EvaluatePressedAction);
            ShowErrorPressedCommand = new RelayCommand(ShowErrorPressedAction);
            UndoCommand = new RelayCommand(UndoAction, () => _editor.CanUndo);
            RedoCommand = new RelayCommand(RedoAction, () => _editor.CanRedo);
            StartSensorsCommand = new RelayCommand(StartSensorsAction);
            StopSensorsCommand = new RelayCommand(StopSensorsAction);
            CompleteTokenCommand = new RelayCommand<int>(CompleteTokenAction);
            
            Messenger.Default.Register<GenericMessage<Sprite>>(this, ViewModelMessagingToken.CurrentSpriteChangedListener, SelectedSpriteChangedMessageAction);
            Messenger.Default.Register<GenericMessage<Project>>(this, ViewModelMessagingToken.CurrentProjectChangedListener, CurrentProjectChangedMessageAction);

            InitEditorBindings();
        }

        private void InitEditorBindings()
        {
            _editor.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == GetPropertyName(() => _editor.Formula)) RaisePropertyChanged(() => Formula);
                if (e.PropertyName == GetPropertyName(() => _editor.Tokens)) RaisePropertyChanged(() => Tokens);
                if (e.PropertyName == GetPropertyName(() => _editor.IsTokensEmpty)) RaisePropertyChanged(() => IsTokensEmpty);
                if (e.PropertyName == GetPropertyName(() => _editor.CaretIndex)) RaisePropertyChanged(() => CaretIndex);
                if (e.PropertyName == GetPropertyName(() => _editor.SelectionStart)) RaisePropertyChanged(() => SelectionStart);
                if (e.PropertyName == GetPropertyName(() => _editor.SelectionLength)) RaisePropertyChanged(() => SelectionLength);
                if (e.PropertyName == GetPropertyName(() => _editor.CanUndo)) RaisePropertyChanged(() => CanUndo);
                if (e.PropertyName == GetPropertyName(() => _editor.CanRedo)) RaisePropertyChanged(() => CanRedo);
                if (e.PropertyName == GetPropertyName(() => _editor.CanDelete)) RaisePropertyChanged(() => CanDelete);
                if (e.PropertyName == GetPropertyName(() => _editor.HasError)) RaisePropertyChanged(() => HasError);
                if (e.PropertyName == GetPropertyName(() => _editor.HasError)) RaisePropertyChanged(() => CanEvaluate);
                if (e.PropertyName == GetPropertyName(() => _editor.ParsingError)) RaisePropertyChanged(() => ParsingError);
            };
        }

        public override void Cleanup()
        {
            StopSensorsCommand.Execute(null);
            RaiseReset();
            _editor.ResetViewModel();
            base.Cleanup();
        }
    }
}