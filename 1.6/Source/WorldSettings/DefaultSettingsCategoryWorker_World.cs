using Verse;

namespace Defaults.WorldSettings
{
    public class DefaultSettingsCategoryWorker_World : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_World(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_WorldSettings());
        }
    }
}
