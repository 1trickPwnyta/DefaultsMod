using Verse;

namespace Defaults.MechWorkModes
{
    public class DefaultSettingWorker_WorkModeFirst : DefaultSettingWorker_MechWorkMode
    {
        public DefaultSettingWorker_WorkModeFirst(DefaultSettingDef def) : base(def)
        {
        }

        public override MechWorkModeDef Default => DefaultsSettings.DefaultWorkModeFirst;

        public override void SetDefault(MechWorkModeDef def)
        {
            DefaultsSettings.DefaultWorkModeFirst = def;
        }
    }
}
