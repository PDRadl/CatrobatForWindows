﻿using System.IO;
using System.Windows.Media;
using Catrobat.Core;
using Catrobat.Core.Objects;
using Catrobat.Core.Objects.Bricks;
using Catrobat.Core.Objects.Costumes;
using Catrobat.Core.Objects.Sounds;
using Catrobat.IDEWindowsPhone.Views.Editor.Scripts;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Catrobat.IDEWindowsPhone.Misc;
using Catrobat.IDEWindowsPhone.Views.Editor.Costumes;
using GalaSoft.MvvmLight.Messaging;
using Catrobat.IDECommon.Resources.Editor;
using System.Windows;
using Catrobat.IDEWindowsPhone.Views.Editor.Sounds;
using Catrobat.IDEWindowsPhone.Views.Editor.Sprites;
using Catrobat.IDEWindowsPhone.Views.Main;
using Catrobat.IDEWindowsPhone.Views.Editor;
using Catrobat.IDEWindowsPhone.Controls.Buttons;
using System.Collections.Generic;
using System;
using Catrobat.IDEWindowsPhone.Controls.ReorderableListbox;

namespace Catrobat.IDEWindowsPhone.ViewModel
{
    public class EditorViewModel : ViewModelBase
    {
        #region Private Members
        
        private readonly ICatrobatContext _catrobatContext;
        private Sprite _selectedSprite;
        private readonly ScriptBrickCollection _scriptBricks;
        private Costume _messageBoxCostume;
        private Sound _messageBoxSound;
        private Sprite _messageBoxSprite;
        private SoundPlayer _soundPlayer;
        private Sound _sound;
        private ListBoxViewPort _listBoxViewPort;

        #endregion

        # region Properties

        public Project CurrentProject
        {
            get
            {
                return _catrobatContext.CurrentProject;
            }
        }

        public ObservableCollection<Sprite> Sprites
        {
            get
            {
                return CurrentProject.SpriteList.Sprites;
            }
        }

        public Sprite SelectedSprite
        {
            get
            {
                return _selectedSprite;
            }
            set
            {
                _selectedSprite = value;

                if (_scriptBricks != null && _scriptBricks.Count == 0 && _listBoxViewPort == null)
                    ListBoxViewPort = new ListBoxViewPort(0, 0);

                _scriptBricks.Update(_selectedSprite);

                if(_scriptBricks.Count > 0 && ListBoxViewPort.FirstVisibleIndex == 0 && ListBoxViewPort.LastVisibleIndex == 0)
                    ListBoxViewPort = new ListBoxViewPort(1, 2);

                RaisePropertyChanged("SelectedSprite");
                RaisePropertyChanged("Sounds");
                RaisePropertyChanged("Costumes");
                RaisePropertyChanged("ScriptBricks");
            }
        }

        public ScriptBrickCollection ScriptBricks
        {
            get
            {
                return _scriptBricks;
            }
        }

        public ObservableCollection<Sound> Sounds
        {
            get
            {
                if (_selectedSprite != null)
                    return _selectedSprite.Sounds.Sounds;
                else
                    return null;
            }
            set
            {
                if (value == _selectedSprite.Sounds.Sounds) return;
                _selectedSprite.Sounds.Sounds = value;
                
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    RaisePropertyChanged("Sounds");
                });
            }
        }

        public ObservableCollection<Costume> Costumes
        {
            get
            {
                if (_selectedSprite != null)
                    return _selectedSprite.Costumes.Costumes;
                else
                    return null;
            }
        }

        public ListBoxViewPort ListBoxViewPort
        {
            get { return _listBoxViewPort; }
            set
            {
                if (value == _listBoxViewPort) return;
                _listBoxViewPort = value;
                RaisePropertyChanged("ListBoxViewPort");
            }
        }

        public DataObject SelectedBrick { get; set; }

        # endregion

        #region Commands

        public ICommand AddNewSpriteCommand
        {
            get;
            private set;
        }

        public ICommand EditSpriteCommand
        {
            get;
            private set;
        }

        public ICommand CopySpriteCommand
        {
            get;
            private set;
        }

        public ICommand DeleteSpriteCommand
        {
            get;
            private set;
        }


        public ICommand AddBroadcastMessageCommand
        {
            get;
            private set;
        }


        public ICommand AddNewScriptBrickCommand
        {
            get;
            private set;
        }

        public ICommand CopyScriptBrickCommand
        {
            get;
            private set;
        }

        public ICommand DeleteScriptBrickCommand
        {
            get;
            private set;
        }


        public ICommand AddNewCostumeCommand
        {
            get;
            private set;
        }

        public ICommand EditCostumeCommand
        {
            get;
            private set;
        }

        public ICommand CopyCostumeCommand
        {
            get;
            private set;
        }

        public ICommand DeleteCostumeCommand
        {
            get;
            private set;
        }


        public ICommand AddNewSoundCommand
        {
            get;
            private set;
        }

        public ICommand EditSoundCommand
        {
            get;
            private set;
        }

        public ICommand DeleteSoundCommand
        {
            get;
            private set;
        }


        public ICommand PlaySoundCommand
        {
            get;
            private set;
        }

        public ICommand StopSoundCommand
        {
            get;
            private set;
        }


        public ICommand StartPlayerCommand
        {
            get;
            private set;
        }

        public ICommand GoToMainViewCommand
        {
            get;
            private set;
        }

        public ICommand ProjectSettingsCommand
        {
            get;
            private set;
        }


        public ICommand UndoCommand
        {
            get;
            private set;
        }

        public ICommand RedoCommand
        {
            get;
            private set;
        }

        public ICommand NothingItemHackCommand
        {
            get;
            private set;
        }

        public RelayCommand ResetViewModelCommand
        {
            get;
            private set;
        }

        # endregion

        #region Actions

        private void AddNewScriptBrickAction()
        {
            GenericMessage<Sprite> message1 = new GenericMessage<Sprite>(SelectedSprite);
            Messenger.Default.Send<GenericMessage<Sprite>>(message1, ViewModelMessagingToken.SelectedSpriteListener);

            List<Object> objects = new List<object>{ScriptBricks, ListBoxViewPort};

            GenericMessage<List<Object>> message2 = new GenericMessage<List<Object>>(objects);
            Messenger.Default.Send<GenericMessage<List<Object>>>(message2, ViewModelMessagingToken.ScriptBrickCollectionListener);

            Navigation.NavigateTo(typeof(AddNewScriptView));
        }

        private void CopyScriptBrickAction(DataObject scriptBrick)
        {
            if (scriptBrick != null)
            {
                if (scriptBrick is Script)
                {
                    DataObject copy = (scriptBrick as Script).Copy((scriptBrick as Script).Sprite);
                    ScriptBricks.Insert(ScriptBricks.ScriptIndexOf((Script)scriptBrick), copy);
                }

                if (scriptBrick is Brick)
                {
                    DataObject copy = (scriptBrick as Brick).Copy((scriptBrick as Brick).Sprite);
                    ScriptBricks.Insert(ScriptBricks.IndexOf(scriptBrick), copy);
                }
            }
        }

        private void DeleteScriptBrickAction(DataObject scriptBrick)
        {
            if (scriptBrick != null)
                ScriptBricks.Remove(scriptBrick);
        }

        private void ReceiveSelectedBrickMessageAction(GenericMessage<DataObject> message)
        {
            SelectedBrick = message.Content;
            RaisePropertyChanged("SelectedBrick");
        }
        

        private void AddBroadcastMessageAction(DataObject broadcastObject)
        {
            GenericMessage<DataObject> message = new GenericMessage<DataObject>(broadcastObject);
            Messenger.Default.Send<GenericMessage<DataObject>>(message, ViewModelMessagingToken.BroadcastObjectListener);

            Navigation.NavigateTo(typeof(NewBroadcastMessageView));
        }

        private void ReceiveNewBroadcastMessageAction(GenericMessage<string> message)
        {
            if (!CurrentProject.BroadcastMessages.Contains(message.Content))
            {
                CurrentProject.BroadcastMessages.Add(message.Content);
                RaisePropertyChanged("BroadcastMessages");
            }
        }


        private void AddNewSpriteAction()
        {
            GenericMessage<ObservableCollection<Sprite>> message = new GenericMessage<ObservableCollection<Sprite>>(Sprites);
            Messenger.Default.Send<GenericMessage<ObservableCollection<Sprite>>>(message, ViewModelMessagingToken.SpriteListListener);

            Navigation.NavigateTo(typeof(AddNewSpriteView));
        }

        private void EditSpriteAction(Sprite sprite)
        {
            GenericMessage<Sprite> message = new GenericMessage<Sprite>(sprite);
            Messenger.Default.Send<GenericMessage<Sprite>>(message, ViewModelMessagingToken.SpriteNameListener);

            Navigation.NavigateTo(typeof(ChangeSpriteView));
        }

        private void CopySpriteAction(Sprite sprite)
        {
            Sprite newSprite = sprite.Copy() as Sprite;
            Sprites.Add(newSprite);
        }

        private void DeleteSpriteAction(Sprite sprite)
        {
            _messageBoxSprite = sprite;
            string name = _messageBoxSprite.Name;

            var message = new DialogMessage(EditorResources.MessageBoxDeleteSpriteText1 + name + EditorResources.MessageBoxDeleteSpriteText2,
                DeleteSpriteMessageBoxResult)
            {
                Button = MessageBoxButton.OKCancel,
                Caption = EditorResources.MessageBoxDeleteSpriteHeader
            };
            Messenger.Default.Send(message);
        }


        private void AddNewSoundAction()
        {
            GenericMessage<Sprite> message = new GenericMessage<Sprite>(SelectedSprite);
            Messenger.Default.Send<GenericMessage<Sprite>>(message, ViewModelMessagingToken.SelectedSpriteListener);

            Navigation.NavigateTo(typeof(AddNewSoundView));
        }

        private void EditSoundAction(Sound sound)
        {
            GenericMessage<Sound> message = new GenericMessage<Sound>(sound);
            Messenger.Default.Send<GenericMessage<Sound>>(message, ViewModelMessagingToken.SoundNameListener);

            Navigation.NavigateTo(typeof(ChangeSoundView));
        }

        private void DeleteSoundAction(Sound sound)
        {
            _messageBoxSound = sound;
            string name = _messageBoxSound.Name;

            var message = new DialogMessage(EditorResources.MessageBoxDeleteSoundText1 + name + EditorResources.MessageBoxDeleteSoundText2,
                DeleteSoundMessageBoxResult)
            {
                Button = MessageBoxButton.OKCancel,
                Caption = EditorResources.MessageBoxDeleteSoundHeader
            };
            Messenger.Default.Send(message);
        }
        

        private void AddNewCostumeAction()
        {
            GenericMessage<Sprite> message = new GenericMessage<Sprite>(SelectedSprite);
            Messenger.Default.Send<GenericMessage<Sprite>>(message, ViewModelMessagingToken.SelectedSpriteListener);

            Navigation.NavigateTo(typeof(AddNewCostumeView));
        }

        private void EditCostumeAction(Costume costume)
        {
            GenericMessage<Costume> message = new GenericMessage<Costume>(costume);
            Messenger.Default.Send<GenericMessage<Costume>>(message, ViewModelMessagingToken.CostumeNameListener);

            Navigation.NavigateTo(typeof(ChangeCostumeView));
        }

        private void CopyCostumeAction(Costume costume)
        {
            Costume newCostume = costume.Copy(SelectedSprite) as Costume;
            Costumes.Add(newCostume);
        }

        private void DeleteCostumeAction(Costume costume)
        {
            _messageBoxCostume = costume;
            string name = _messageBoxCostume.Name;

            var message = new DialogMessage(EditorResources.MessageBoxDeleteCostumeText1 + name + EditorResources.MessageBoxDeleteCostumeText2,
                DeleteCostumeMessageBoxResult)
            {
                Button = MessageBoxButton.OKCancel,
                Caption = EditorResources.MessageBoxDeleteCostumeHeader
            };
            Messenger.Default.Send(message);
        }


        private void PlaySoundAction(List<Object> parameter)
        {
            StopSoundCommand.Execute(null);

            if (_soundPlayer == null)
            {
                _soundPlayer = new SoundPlayer();
                _soundPlayer.SoundStateChanged += SoundPlayerStateChanged;
            }

            var state = (PlayButtonState)parameter[0];
            var sound = (Sound)parameter[1];

            if (state == PlayButtonState.Play)
            {
                if (_sound != sound)
                {
                    _sound = sound;
                    _soundPlayer.SetSound(_sound);
                }
                _soundPlayer.Play();
            }
            else
            {
                _soundPlayer.Pause();
            }
        }

        private void StopSoundAction()
        {
            if (_soundPlayer != null)
            {
                _soundPlayer.Pause();
            }
        }

        private async void StartPlayerAction()
        {
            await PlayerLauncher.LaunchPlayer(CurrentProject.ProjectName);
        }

        private void GoToMainViewAction()
        {
            ResetViewModel();
            Navigation.NavigateTo(typeof(MainView));
        }

        private void ProjectSettingsAction()
        {
            GenericMessage<Project> message = new GenericMessage<Project>(CurrentProject);
            Messenger.Default.Send<GenericMessage<Project>>(message, ViewModelMessagingToken.ProjectNameListener);

            Navigation.NavigateTo(typeof(ProjectSettingsView));
        }


        private void UndoAction()
        {
            CurrentProject.Undo();
        }

        private void RedoAction()
        {
            CurrentProject.Redo();
        }

        private void NothingItemHackAction(object attachedObject)
        {
            // Pretty hack-y, but oh well...
            if (attachedObject is BroadcastScript)
            {
                ((BroadcastScript)attachedObject).ReceivedMessage = null;
            }
            else if (attachedObject is PointToBrick)
            {
                ((PointToBrick)attachedObject).PointedSprite = null;
            }
            else if (attachedObject is PlaySoundBrick)
            {
                ((PlaySoundBrick)attachedObject).Sound = null;
            }
            else if (attachedObject is SetCostumeBrick)
            {
                ((SetCostumeBrick)attachedObject).Costume = null;
            }
            else if (attachedObject is BroadcastBrick)
            {
                ((BroadcastBrick)attachedObject).BroadcastMessage = null;
            }
            else if (attachedObject is BroadcastWaitBrick)
            {
                ((BroadcastWaitBrick)attachedObject).BroadcastMessage = null;
            }
        }

        private void ResetViewModelAction()
        {
            ResetViewModel();
        }

        #endregion
        
        public EditorViewModel()
        {
            AddNewScriptBrickCommand = new RelayCommand(AddNewScriptBrickAction);
            CopyScriptBrickCommand = new RelayCommand<DataObject>(CopyScriptBrickAction);
            DeleteScriptBrickCommand = new RelayCommand<DataObject>(DeleteScriptBrickAction);
            
            AddBroadcastMessageCommand = new RelayCommand<DataObject>(AddBroadcastMessageAction);

            AddNewSpriteCommand = new RelayCommand(AddNewSpriteAction);
            EditSpriteCommand = new RelayCommand<Sprite>(EditSpriteAction);
            CopySpriteCommand = new RelayCommand<Sprite>(CopySpriteAction);
            DeleteSpriteCommand = new RelayCommand<Sprite>(DeleteSpriteAction);

            AddNewSoundCommand = new RelayCommand(AddNewSoundAction);
            EditSoundCommand = new RelayCommand<Sound>(EditSoundAction);
            DeleteSoundCommand = new RelayCommand<Sound>(DeleteSoundAction);

            AddNewCostumeCommand = new RelayCommand(AddNewCostumeAction);
            EditCostumeCommand = new RelayCommand<Costume>(EditCostumeAction);
            CopyCostumeCommand = new RelayCommand<Costume>(CopyCostumeAction);
            DeleteCostumeCommand = new RelayCommand<Costume>(DeleteCostumeAction);

            PlaySoundCommand = new RelayCommand<List<Object>>(PlaySoundAction);
            StopSoundCommand = new RelayCommand(StopSoundAction);
            StartPlayerCommand = new RelayCommand(StartPlayerAction);
            GoToMainViewCommand = new RelayCommand(GoToMainViewAction);
            ProjectSettingsCommand = new RelayCommand(ProjectSettingsAction);

            UndoCommand = new RelayCommand(UndoAction);
            RedoCommand = new RelayCommand(RedoAction);

            NothingItemHackCommand = new RelayCommand<object>(NothingItemHackAction);

            ResetViewModelCommand = new RelayCommand(ResetViewModelAction);


            if (IsInDesignMode)
            {
                _catrobatContext = new CatrobatContextDesign();
                _selectedSprite = _catrobatContext.CurrentProject.SpriteList.Sprites[0];
            }
            else
            {
                _catrobatContext = CatrobatContext.GetContext();
            }

            _scriptBricks = new ScriptBrickCollection();

            Messenger.Default.Register<GenericMessage<string>>(this, ViewModelMessagingToken.BroadcastMessageListener, ReceiveNewBroadcastMessageAction);
            Messenger.Default.Register<GenericMessage<DataObject>>(this, ViewModelMessagingToken.SelectedBrickListener, ReceiveSelectedBrickMessageAction);
        }


        #region MessageBoxResult

        private void DeleteCostumeMessageBoxResult(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                _messageBoxCostume.Delete();
                Costumes.Remove(_messageBoxCostume);
                CatrobatContext.GetContext().CleanUpCostumeReferences(_messageBoxCostume, _messageBoxCostume.Sprite);
            }
        }

        private void DeleteSoundMessageBoxResult(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                _messageBoxSound.Delete();
                Sounds.Remove(_messageBoxSound);
                CatrobatContext.GetContext().CleanUpSoundReferences(_messageBoxSound, _messageBoxSound.Sprite);
            }
        }

        private void DeleteSpriteMessageBoxResult(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                _messageBoxSprite.Delete();
                Sprites.Remove(_messageBoxSprite);
                CatrobatContext.GetContext().CleanUpSpriteReferences(_messageBoxSprite);
            }
        }

        #endregion


        private void SoundPlayerStateChanged(Misc.SoundState soundState, Misc.SoundState newState)
        {
            if (newState == Misc.SoundState.Stopped)
            {
                if (Sounds != null)
                {
                    var tempSounds = new ObservableCollection<Sound>();
                    foreach (Sound sound in Sounds)
                        tempSounds.Add(sound);

                    Sounds = null;
                    Sounds = tempSounds;
                }
            }
        }

        private void ResetViewModel()
        {
            _messageBoxCostume = null;

            SelectedSprite = null;
            SelectedBrick = null;
            ListBoxViewPort = null;

            if (_soundPlayer != null)
            {
                _soundPlayer.Stop();
                _soundPlayer.SoundStateChanged -= SoundPlayerStateChanged;
            }
            _soundPlayer = null;
            _sound = null;
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}