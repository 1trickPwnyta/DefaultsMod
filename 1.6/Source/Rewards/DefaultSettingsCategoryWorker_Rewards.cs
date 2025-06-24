using Verse;

namespace Defaults.Rewards
{
    public class DefaultSettingsCategoryWorker_Rewards : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_Rewards(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_RewardsSettings(def));
        }
    }
}
