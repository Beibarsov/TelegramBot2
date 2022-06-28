using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot2.Services;
using TelegramBot2.Configuration;

namespace TelegramBot2.Controllers
{
    public class VoiceMessageController
    {
        //private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramClient;
        private readonly IFileHandler _audioFileHandler;
        private readonly IStorage _memoryStorage;

        public VoiceMessageController(ITelegramBotClient telegramBotClient, IFileHandler audioFileHandler, IStorage memoryStorage)
        {
           // _appSettings = appSettings;
            _telegramClient = telegramBotClient;
            _audioFileHandler = audioFileHandler;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null) return;

            await _audioFileHandler.Download(fileId, ct);
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено голосовое сообщение, загружено", cancellationToken: ct);


            string userLanguageCode = _memoryStorage.GetSession(message.Chat.Id).LanguageCode;
            var result = _audioFileHandler.Process(userLanguageCode);
            Console.WriteLine(result);
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Текст: " + result, cancellationToken: ct);


        }
    }
}
