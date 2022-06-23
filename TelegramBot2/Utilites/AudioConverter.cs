using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFMpegCore;
using TelegramBot2.Extensions;


namespace TelegramBot2.Utilites
{
    internal static class AudioConverter
    {
        public static void TryConvert(string inputFile, string outputFile)
        {
            // Задаём путь, где лежит вспомогательная программа - конвертер
            GlobalFFOptions.Configure(options => options.BinaryFolder = Path.Combine(DirectoryExtension.GetSolutionRoot(), "ffmpeg-win64", "bin"));

            // Вызываем Ffmpeg, передав требуемые аргументы.
            FFMpegArguments
              .FromFileInput(inputFile)
              .OutputToFile(outputFile, true, options => options
                .WithFastStart())
              .ProcessSynchronously();
        }
    }
}
