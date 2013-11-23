﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Catrobat.IDE.Core.Services.Storage;

namespace Catrobat.IDE.Core.Services
{
    public enum TypeCreationMode { Lazy, Normal }

    public class ServiceLocator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static INavigationService NavigationService
        { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static ISystemInformationService SystemInformationService
        { get { return GetInstance<ISystemInformationService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static ICultureService CulureService
        { get { return GetInstance<ICultureService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IImageResizeService ImageResizeService
        { get { return GetInstance<IImageResizeService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IPlayerLauncherService PlayerLauncherService
        { get { return GetInstance<IPlayerLauncherService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IResourceLoaderFactory ResourceLoaderFactory
        { get { return GetInstance<IResourceLoaderFactory>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IStorageFactory StorageFactory
        { get { return GetInstance<IStorageFactory>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IServerCommunicationService ServerCommunicationService
        { get { return GetInstance<IServerCommunicationService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IImageSourceConversionService ImageSourceConversionService
        { get { return GetInstance<IImageSourceConversionService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IProjectImporterService ProjectImporterService
        { get { return GetInstance<IProjectImporterService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static ISoundPlayerService SoundPlayerService
        { get { return GetInstance<ISoundPlayerService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static ISoundRecorderService SoundRecorderService
        { get { return GetInstance<ISoundRecorderService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IPictureService PictureService
        { get { return GetInstance<IPictureService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static INotificationService NotifictionService
        { get { return GetInstance<INotificationService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IColorConversionService ColorConversionService
        { get { return GetInstance<IColorConversionService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IShareService ShareService
        { get { return GetInstance<IShareService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IDispatcherService DispatcherService
        { get { return GetInstance<IDispatcherService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IPortableUIElementConversionService PortableUIElementConversionService
        { get { return GetInstance<IPortableUIElementConversionService>(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1822:MarkMembersAsStatic",
        Justification = "This non-static member is needed for data binding purposes.")]
        public static IActionTemplateService ActionTemplateService
        { get { return GetInstance<IActionTemplateService>(); } }



        public static object ViewModelLocator { get; set; }

        public static object ThemeChooser { get; set; }

        public static object LocalizedStrings { get; set; }

        

        private static readonly Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static void Register<T>(TypeCreationMode mode)
        {
            lock (Instances)
            {
                if (mode == TypeCreationMode.Lazy)
                {
                    Instances.Add(typeof(T), null);
                }
                else if (mode == TypeCreationMode.Normal)
                {
                    if(!Instances.ContainsKey(typeof(T)))
                        Instances.Add(typeof(T), Activator.CreateInstance<T>());
                }
            }
        }

        public static object GetInstance(Type type)
        {
            lock (Instances)
            {
                object instance = null;
                bool isInDictionary = false;

                foreach (var pair in Instances)
                {
                    if (pair.Key.GetTypeInfo().BaseType == type
                        || pair.Key == type
                        || pair.Key.GetTypeInfo().ImplementedInterfaces.Contains(type))
                    {
                        instance = pair.Value;

                        isInDictionary = instance != null;

                        if (!isInDictionary)
                            instance = Activator.CreateInstance(pair.Key);

                        break;
                    }
                }

                if (instance == null)
                    throw new Exception("Type " + type.GetTypeInfo().Name + " is not registered.");

                if (!isInDictionary)
                    Instances[type] = instance;

                return instance;
            }
        }

        public static T GetInstance<T>()
        {
            return (T) GetInstance(typeof (T));
        }

        public static void UnRegisterAll()
        {
            Instances.Clear();
        }
    }
}