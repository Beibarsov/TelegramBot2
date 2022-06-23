using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot2.Configuration;
//using TelegramBot2.

namespace TelegramBot2.Services
{
    public class AudioFileHandler : IFileHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;


        public AudioFileHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _telegramBotClient = telegramBotClient;
        }

        public async Task Download(string fileId, CancellationToken ct)
        {
            string inputAudioFilePath = Path.Combine(_appSettings.DownloadsFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");

            using (FileStream fs = File.Create(inputAudioFilePath))
            {
                var file = await _telegramBotClient.GetFileAsync(fileId, ct);
                if (file.FilePath == null) return;

                await _telegramBotClient.DownloadFileAsync(file.FilePath, fs, ct);
            }
        }

        public string Process(string param)
        {
            throw new NotImplementedException();
        }
    }
}
