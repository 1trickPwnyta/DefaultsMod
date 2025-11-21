using Defaults.Compatibility;
using Defaults.Defs;
using Defaults.Workers;
using RimWorld;
using Verse;

namespace Defaults.Storyteller
{
    public class DefaultSettingsCategoryWorker_Storyteller : DefaultSettingsCategoryWorker
    {
        private Difficulty defaultDifficultyValues;
        private bool? defaultPermadeath;
        private NoPauseOptions defaultNoPauseOptions;

        public DefaultSettingsCategoryWorker_Storyteller(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_Storyteller());
        }

        protected override bool GetCategorySetting(string key, out object value)
        {
            switch (key)
            {
                case Settings.DIFFICULTY_VALUES:
                    value = defaultDifficultyValues;
                    return true;
                case Settings.PERMADEATH:
                    value = defaultPermadeath.Value;
                    return true;
                case Settings.NO_PAUSE_OPTIONS:
                    value = defaultNoPauseOptions;
                    return true;
                default:
                    return base.GetCategorySetting(key, out value);
            }
        }

        protected override bool SetCategorySetting(string key, object value)
        {
            switch (key)
            {
                case Settings.DIFFICULTY_VALUES:
                    defaultDifficultyValues = value as Difficulty;
                    return true;
                case Settings.PERMADEATH:
                    defaultPermadeath = (bool)value;
                    return true;
                case Settings.NO_PAUSE_OPTIONS:
                    defaultNoPauseOptions = value as NoPauseOptions;
                    return true;
                default:
                    return base.SetCategorySetting(key, value);
            }
        }

        protected override void ResetCategorySettings(bool forced)
        {
            if (forced || defaultDifficultyValues == null)
            {
                defaultDifficultyValues = new Difficulty();
                defaultDifficultyValues.CopyFrom(DifficultyDefOf.Rough);
            }
            if (forced || defaultPermadeath == null)
            {
                defaultPermadeath = false;
            }
            ModCompatibilityUtility_NoPause.ResetNoPauseOptions(ref defaultNoPauseOptions, forced);
        }

        protected override void ExposeCategorySettings()
        {
            Scribe_Deep.Look(ref defaultDifficultyValues, Settings.DIFFICULTY_VALUES);
            Scribe_Values.Look(ref defaultPermadeath, Settings.PERMADEATH);
            ModCompatibilityUtility_NoPause.WriteNoPauseOptions(ref defaultNoPauseOptions);
            BackwardCompatibilityUtility.MigrateAnomalyPlaystyleSettings(defaultDifficultyValues);
        }
    }
}
