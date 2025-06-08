namespace Defaults.TargetTemperature
{
    public class DefaultSettingWorker_TargetTempCooler : DefaultSettingWorker_TargetTemp
    {
        public DefaultSettingWorker_TargetTempCooler(DefaultSettingDef def) : base(def)
        {
        }

        public override float Target { get => DefaultsSettings.DefaultTargetTemperatureCooler; set => DefaultsSettings.DefaultTargetTemperatureCooler = value; }
    }
}
