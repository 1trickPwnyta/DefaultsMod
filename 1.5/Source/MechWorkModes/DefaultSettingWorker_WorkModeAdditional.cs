using Verse;

namespace Defaults.MechWorkModes
{
    public class DefaultSettingWorker_WorkModeAdditional : DefaultSettingWorker_MechWorkMode
    {
        public DefaultSettingWorker_WorkModeAdditional(DefaultSettingDef def) : base(def)
        {
        }

        public override MechWorkModeDef Default => DefaultsSettings.DefaultWorkModeAdditional;

        public override void SetDefault(MechWorkModeDef def)
        {
            DefaultsSettings.DefaultWorkModeAdditional = def;
        }
    }
}
