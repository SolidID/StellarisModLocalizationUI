using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ionic.Zip;
using ModLocalization.Core.Model;

namespace ModLocalization.Core
{
    internal class ModRepository
    {
        private readonly string _basePath;

        public ModRepository(string basePath)
        {
            _basePath = basePath;
        }

        public IEnumerable<Mod> GetAllMods()
        {
            var dir = new DirectoryInfo(_basePath);
            var mods = dir.GetFiles("*.zip");

            ConcurrentBag<Mod> result = new ConcurrentBag<Mod>();

            Parallel.ForEach(mods, file =>
            {
                string modName = String.Empty;

                // open zip
                using (var zip = ZipFile.Read(file.FullName))
                {
                    foreach (var zipEntry in zip.Entries)
                    {
                        zipEntry.FileName.Dump();
                    }
                }
                result.Add(new Mod()
                {
                    ID = Int32.Parse(file.Directory?.Name ?? "-1"),

                });
            });

            return result.ToList();
        }
    }
}