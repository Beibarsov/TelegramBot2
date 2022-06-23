using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot2.Services;

namespace TelegramBot2.Controllers
{
    internal class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _storage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage storage)
        {
            _telegramClient = telegramBotClient;
            _storage = storage;
        }

        public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _storage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

            // Генерим информационное сообщение
            string languageText = callbackQuery.Data switch
            {
                "ru" => " Русский",
                "en" => " Английский",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);

            //Console.WriteLine($"Контроллер {GetType().Name} заподозрил нажатие на кнопку");
            //await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Нажата кнопка" + callbackQuery.Data, cancellationToken: ct);
        }
    }
}
