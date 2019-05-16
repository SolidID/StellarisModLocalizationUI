using Ionic.Zip;
using ModLocalization.Core.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModLocalization.Core.Data
{
    public class ModRepository
    {
        public string BasePath { get; }
        private static readonly Regex _languageFile = new Regex("(.*)_l_(.*).yml", RegexOptions.Compiled);
        private static readonly Regex _modeNameParser = new Regex(@"^\s*name=""(.*)""\s*$", RegexOptions.Compiled | RegexOptions.Multiline);

        private List<Mod> _cache;

        public ModRepository(string basePath)
        {
            BasePath = basePath;
            _cache = LoadMods();
        }

        public void Refresh()
        {
            _cache = LoadMods();
        }

        private List<Mod> LoadMods()
        {
            var dir = new DirectoryInfo(BasePath);
            var mods = dir.GetFiles("*.zip", SearchOption.AllDirectories);

            ConcurrentBag<Mod> result = new ConcurrentBag<Mod>();

            Parallel.ForEach(mods, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, file =>
            {
                string modName = String.Empty;

                var item = new Mod()
                {
                    ID = Int32.Parse(file.Directory?.Name ?? "-1"),
                    Location = file.FullName,
                };

                // open zip
                using (var zip = ZipFile.Read(file.FullName))
                {
                    // getting the name form .mod file
                    using (var mem = new MemoryStream())
                    {
                        zip.Entries.FirstOrDefault(it => !it.IsDirectory && it.FileName.EndsWith(".mod"))?.Extract(mem);
                        mem.Seek(0, SeekOrigin.Begin);
                        using (StreamReader rdr = new StreamReader(mem))
                        {
                            item.Name = ParseNameFromFileContent(rdr.ReadToEnd());
                        }
                    }

                    zip.Entries
                        .Where(it => !it.IsDirectory && _languageFile.IsMatch(it.FileName))
                        .ForEach(it => item.LocalizationFiles.Add(new LocalizationFile()
                        {
                            FileName = Path.GetFileName(it.FileName),
                            RelativePath = Path.GetFullPath(it.FileName)
                        }));
                }
                result.Add(item);
            });

            return result.ToList();
        }

        public IEnumerable<Mod> GetAllMods()
        {
            return _cache;
        }

        public Mod GetMod(int id)
        {
            return _cache.FirstOrDefault(it => it.ID == id);
        }

        public LocalizationFile GetLocalizationOfMod(int id, string culture)
        {
            return GetMod(id).LocalizationFiles.FirstOrDefault(it => it.FileName.Contains(culture));
        }

        private string ParseNameFromFileContent(string fileContent)
        {
            return _modeNameParser.Match(fileContent).Groups[1].Value;
        }
    }
}