using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TelegramBot2.Configuration;
using TelegramBot2.Controllers;
using TelegramBot2.Services;

namespace TelegramBot2
{
    public class Program
    {




        static AppSettings BuildAppSettings()
        {
            return new AppSettings {
                BotToken = "5158490376:AAEMo4M6didw6xBPdJ99xNz1SERGRim-sE8",
                DownloadsFolder = "C:\\Telegram",
                AudioFileName = "audio",
                InputAudioFormat = "ogg",
                OutputAudioFormat = "wav",
            };
        }

        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {

            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(appSettings);

            services.AddTransient<TextMessageController>();
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<VoiceMessageController>();

            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<IFileHandler, AudioFileHandler>();



            

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));

            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
    }
}