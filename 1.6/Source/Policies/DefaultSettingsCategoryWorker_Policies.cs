using Verse;

namespace Defaults.Policies
{
    public class DefaultSettingsCategoryWorker_Policies : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_Policies(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_Policies(def));
        }
    }
}
