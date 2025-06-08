using Verse;

namespace Defaults.ResourceCategories
{
    public class DefaultSettingsCategoryWorker_ResourceCategories : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_ResourceCategories(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_ResourceCategories());
        }
    }
}
