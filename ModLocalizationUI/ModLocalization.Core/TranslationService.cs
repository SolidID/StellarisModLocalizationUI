using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModLocalization.Core
{
    public class TranslationService
    {
        /// <summary>
        /// Searches within a Directory structure for translation files. It creates a copy of the <paramref name="originalCulture"/> for each of the given cultures of <paramref name="targetCultures"/>.
        /// </summary>
        /// <param name="baseDir">The start directory for the file lookup.</param>
        /// <param name="originalCulture">If a translation file of that culture is found it is used as copy source for the new cultures.</param>
        /// <param name="targetCultures">The original culture file is copied for each of these cultures.</param>
        public void Multiply(string baseDir, string originalCulture, IEnumerable<string> targetCultures)
        {
            var targetLanguages = targetCultures.ToList();

            if (targetLanguages.Count == 0)
            {
                targetCultures = GetDefaultLanguages();
            }

            var dir = new DirectoryInfo(baseDir);
            foreach (var file in dir.GetFiles("*_l_*.yml", SearchOption.AllDirectories).Where(file => file.Name.Contains(originalCulture)))
            {
                // parse original language
                //var orgLanguage = file.Name.Substring(file.Name.IndexOf("_l_", StringComparison.Ordinal) + 3).Replace(".yml", String.Empty);
                foreach (var targetLanguage in targetLanguages)
                {
                    // skip if the target equals the original language
                    if (targetLanguage.Equals(originalCulture, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    var targetFileName = Path.Combine(Path.GetDirectoryName(file.FullName), file.Name.Replace(originalCulture, targetLanguage));
                    // skip copy process if the target file already exists
                    if (File.Exists(targetFileName))
                    {
                        continue;
                    }

                    // modify language definition in file
                    var originalContent = File.ReadAllText(file.FullName);

                    File.WriteAllText(targetFileName, originalContent.Replace($"l_{originalCulture}", $"l_{targetLanguage}"), Encoding.UTF8);
                }
            }
        }

        private static List<string> GetDefaultLanguages()
        {
            // taken form https://stellaris.paradoxwikis.com/Localisation_modding
            return new[] { "braz_por", "english", "french", "german", "polish", "russian", "spanish" }.ToList();
        }
    }
}
