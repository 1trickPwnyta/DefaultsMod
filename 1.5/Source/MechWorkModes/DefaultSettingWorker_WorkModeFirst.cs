namespace Defaults.MechWorkModes
{
    public class DefaultSettingWorker_WorkModeFirst : DefaultSettingWorker_MechWorkMode
    {
        public DefaultSettingWorker_WorkModeFirst(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.MECH_WORK_MODE_FIRST;
    }
}
