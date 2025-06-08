using Verse;

namespace Defaults.MapSettings
{
    public class DefaultSettingsCategoryWorker_Map : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_Map(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_MapSettings());
        }
    }
}
