using Verse;

namespace Defaults.BabyFeeding
{
    public class DefaultSettingsCategoryWorker_BabyFeeding : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_BabyFeeding(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_BabyFeedingSettings());
        }
    }
}
