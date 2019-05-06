using System.Collections.Generic;

namespace ModLocalization.Core.Model
{
    public class Mod
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public IEnumerable<LocalizationFile> LocalizationFiles { get; set; }
    }
}