using Verse;

namespace Defaults.WorkbenchBills
{
    public class DefaultSettingsCategoryWorker_WorkbenchBills : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_WorkbenchBills(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_WorkbenchBills(def));
        }
    }
}
