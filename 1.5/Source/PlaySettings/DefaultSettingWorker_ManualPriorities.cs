namespace Defaults.PlaySettings
{
    public class DefaultSettingWorker_ManualPriorities : DefaultSettingWorker_Checkbox
    {
        public DefaultSettingWorker_ManualPriorities(DefaultSettingDef def) : base(def)
        {
        }

        public override bool Enabled { get => DefaultsSettings.DefaultManualPriorities; set => DefaultsSettings.DefaultManualPriorities = value; }
    }
}
