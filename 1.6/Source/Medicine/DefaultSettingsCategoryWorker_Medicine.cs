using Verse;

namespace Defaults.Medicine
{
    public class DefaultSettingsCategoryWorker_Medicine : DefaultSettingsCategoryWorker
    {
        private MedicineOptions defaultMedicineOptions;

        public DefaultSettingsCategoryWorker_Medicine(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_MedicineSettings());
        }

        protected override bool GetCategorySetting(string key, out object value)
        {
            switch (key)
            {
                case Settings.MEDICINE:
                    value = defaultMedicineOptions;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }

        protected override bool SetCategorySetting(string key, object value)
        {
            switch (key)
            {
                case Settings.MEDICINE:
                    defaultMedicineOptions = value as MedicineOptions;
                    return true;
                default:
                    return false;
            }
        }

        public override void ResetSettings()
        {
            base.ResetSettings();
            defaultMedicineOptions = new MedicineOptions();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref defaultMedicineOptions, Settings.MEDICINE);
        }
    }
}
