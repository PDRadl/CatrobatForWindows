﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Catrobat.IDE.Core.Models;
using Catrobat.IDE.Core.Services;
using System.IO;
using Catrobat.IDE.Core.Utilities.Helpers;
using Catrobat.IDE.Core.ViewModels.Editor.Sounds;

namespace Catrobat.IDE.Phone.Services
{
    public class SoundServicePhone : ISoundService
    {
        private readonly Semaphore _semaphore = new Semaphore(0, 1);
        private SoundServiceResult _result;

        public void Finished(SoundServiceResult result)
        {
            _result = result;

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            WaveHeaderHelper.WriteHeader(writer.BaseStream, ServiceLocator.SoundRecorderService.SampleRate);
            var dataBuffer = ServiceLocator.SoundRecorderService.GetSoundAsStream().ToArray();
            writer.Write(dataBuffer, 0, (int)ServiceLocator.SoundRecorderService.GetSoundAsStream().Length);
            writer.Flush();
            //writer.Dispose();

            stream.Seek(0, SeekOrigin.Begin);

            //var readStream = new MemoryStream();


            _result.Result = stream;

            //var message = new GenericMessage<Stream>(stream);
            //Messenger.Default.Send(message, ViewModelMessagingToken.SoundStreamListener);

            _semaphore.Release(1);
        }

        public Task<SoundServiceResult> CreateSoundFromRecorder(Sprite sprite)
        {
            var task = Task.Run(() =>
            {
                _semaphore.WaitOne();
                return _result;
            });

            ServiceLocator.NavigationService.NavigateTo<SoundRecorderViewModel>();

            return task;
        }

        public async Task<SoundServiceResult> CreateSoundFromMediaLibrary(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}