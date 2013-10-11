﻿using Catrobat.IDE.Core.Services.Storage;
using Catrobat.IDE.Core.Utilities.Helpers;
using Cirrious.MvvmCross.Plugins.PictureChooser;

namespace Catrobat.IDE.Core.Services
{
    public class ServiceLocator
    {
        public static INavigationService NavigationService { get; private set; }

        public static ISystemInformationService SystemInformationService { get; private set; }

        public static ICultureService CulureService { get; private set; }

        public static IImageResizeService ImageResizeService { get; private set; }

        public static IPlayerLauncherService PlayerLauncherService { get; private set; }

        public static IResourceLoaderFactory ResourceLoaderFactory { get; private set; }

        public static IStorageFactory StorageFactory { get; private set; }

        public static IServerCommunicationService ServerCommunicationService { get; private set; }

        public static IImageSourceConversionService ImageSourceConversionService { get; private set; }

        public static IProjectImporterService ProjectImporterService { get; private set; }

        public static ISoundPlayerService SoundPlayerService { get; private set; }

        public static ISoundRecorderService SoundRecorderService { get; private set; }

        public static IMvxPictureChooserTask MvxPictureChooserTask { get; private set; }

        public static void SetServices(
            INavigationService navigationService,
            ISystemInformationService systemInformationService,
            ICultureService culureService,
            IImageResizeService imageResizeService,
            IPlayerLauncherService playerLauncherService,
            IResourceLoaderFactory resourceLoaderFactory,
            IStorageFactory storageFactory,
            IServerCommunicationService serverCommunicationService,
            IImageSourceConversionService imageSourceConversionService,
            IProjectImporterService projectImporterService,
            ISoundPlayerService soundPlayerService,
            ISoundRecorderService soundRecorderService
            )
        {
            NavigationService = navigationService;
            SystemInformationService = systemInformationService;
            CulureService = culureService;
            ImageResizeService = imageResizeService;
            PlayerLauncherService = playerLauncherService;
            ResourceLoaderFactory = resourceLoaderFactory;
            StorageFactory = storageFactory;
            ServerCommunicationService = serverCommunicationService;
            ImageSourceConversionService = imageSourceConversionService;
            ProjectImporterService = projectImporterService;
            SoundPlayerService = soundPlayerService;
            SoundRecorderService = soundRecorderService;
        }

        public static void SetMvxServices(IMvxPictureChooserTask mvxPictureChooserTask)
        {
            MvxPictureChooserTask = mvxPictureChooserTask;
        }
    }
}