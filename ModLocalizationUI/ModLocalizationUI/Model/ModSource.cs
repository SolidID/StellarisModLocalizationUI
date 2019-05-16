using ModLocalization.Core.Data;
using ModLocalization.Core.Model;
using System.Collections.ObjectModel;

namespace ModLocalization.UI.Model
{
    internal class ModSource
    {
        public string ModsLocation { get; set; } = @"C:\Games\Steam\steamapps\workshop\content\281990";
        public ObservableCollection<Mod> Mods => new ObservableCollection<Mod>(new ModRepository(ModsLocation).GetAllMods());
    }
}