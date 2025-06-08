namespace Defaults.Medicine
{
    public class DefaultSettingWorker_MedCarryGuest : DefaultSettingWorker_Checkbox
    {
        public DefaultSettingWorker_MedCarryGuest(DefaultSettingDef def) : base(def)
        {
        }

        public override bool Enabled { get => DefaultsSettings.GuestsCarryMedicine; set => DefaultsSettings.GuestsCarryMedicine = value; }
    }
}
