using System.Collections.Generic;

namespace ModLocalization.Core.Model
{
    public class Mod
    {
        public Mod()
        {
            LocalizationFiles = new List<LocalizationFile>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public IList<LocalizationFile> LocalizationFiles { get; set; }
        public string Location { get; set; }
    }
}