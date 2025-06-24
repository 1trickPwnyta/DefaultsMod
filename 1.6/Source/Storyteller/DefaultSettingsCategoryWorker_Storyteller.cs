using Verse;

namespace Defaults.Storyteller
{
    public class DefaultSettingsCategoryWorker_Storyteller : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_Storyteller(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_Storyteller(def));
        }
    }
}
