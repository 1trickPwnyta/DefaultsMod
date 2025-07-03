using Defaults.Defs;
using Defaults.Workers;
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
                    return base.GetCategorySetting(key, out value);
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
                    return base.SetCategorySetting(key, value);
            }
        }

        protected override void ResetCategorySettings(bool forced)
        {
            if (forced || defaultMedicineOptions == null)
            {
                defaultMedicineOptions = new MedicineOptions();
            }
        }

        protected override void ExposeCategorySettings()
        {
            Scribe_Deep.Look(ref defaultMedicineOptions, Settings.MEDICINE);
            BackwardCompatibilityUtility.MigrateMedicineOptions(ref defaultMedicineOptions);
        }
    }
}
