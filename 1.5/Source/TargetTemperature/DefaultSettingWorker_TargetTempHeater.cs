namespace Defaults.TargetTemperature
{
    public class DefaultSettingWorker_TargetTempHeater : DefaultSettingWorker_TargetTemp
    {
        public DefaultSettingWorker_TargetTempHeater(DefaultSettingDef def) : base(def)
        {
        }

        public override float Target { get => DefaultsSettings.DefaultTargetTemperatureHeater; set => DefaultsSettings.DefaultTargetTemperatureHeater = value; }
    }
}
