using System;
using System.IO;
using System.Linq;
using ModLocalization.Core;
using ModLocalization.Core.Data;

namespace ModLocalization.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseCulture = "english";
            var targetCulture = "german";

            if (args.Length >= 1)
                baseCulture = args[0];

            if (args.Length >= 2)
                targetCulture = args[1];

            var modRepository = new ModRepository(@"App_Data");

            var modsWithoutGerman = modRepository
                .GetAllMods()
                .Where(it =>
                    it.LocalizationFiles.Count > 0
                    && !it.LocalizationFiles.Any(lf =>
                        lf.FileName.Contains(targetCulture, StringComparison.OrdinalIgnoreCase)));

            var zip = new ZipService();

            foreach (var mod in modsWithoutGerman)
            {
                System.Console.WriteLine("--------------");
                System.Console.WriteLine($"{mod.ID} - {mod.Name}");
                foreach (var localizationFile in mod.LocalizationFiles)
                {
                    System.Console.WriteLine($"\t{localizationFile.FileName}");
                }

                var tempFolder = Path.Combine(Path.GetDirectoryName(mod.Location), "temp");
                zip.Extract(mod.Location, tempFolder);
                new TranslationService().Multiply(tempFolder, baseCulture, new []{targetCulture});
                zip.Compress(tempFolder, mod.Location, ZipService.CompressMode.Backup);

                Directory.Delete(tempFolder, true);
                System.Console.WriteLine("--------------");
            }
        }
    }
}
