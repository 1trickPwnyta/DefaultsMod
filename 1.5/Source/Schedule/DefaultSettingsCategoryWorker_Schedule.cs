using Verse;

namespace Defaults.Schedule
{
    public class DefaultSettingsCategoryWorker_Schedule : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_Schedule(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_ScheduleSettings());
        }
    }
}
