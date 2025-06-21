using Verse;

namespace Defaults.Medicine
{
    public class DefaultSettingsCategoryWorker_Medicine : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_Medicine(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_MedicineSettings());
        }
    }
}
