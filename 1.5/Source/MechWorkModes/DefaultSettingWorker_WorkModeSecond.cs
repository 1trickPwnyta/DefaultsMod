using Verse;

namespace Defaults.MechWorkModes
{
    public class DefaultSettingWorker_WorkModeSecond : DefaultSettingWorker_MechWorkMode
    {
        public DefaultSettingWorker_WorkModeSecond(DefaultSettingDef def) : base(def)
        {
        }

        public override MechWorkModeDef Default => DefaultsSettings.DefaultWorkModeSecond;

        public override void SetDefault(MechWorkModeDef def)
        {
            DefaultsSettings.DefaultWorkModeSecond = def;
        }
    }
}
