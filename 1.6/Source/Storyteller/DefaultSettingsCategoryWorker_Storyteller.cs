using Defaults.Compatibility;
using Defaults.Defs;
using Defaults.Workers;
using RimWorld;
using Verse;

namespace Defaults.Storyteller
{
    public class DefaultSettingsCategoryWorker_Storyteller : DefaultSettingsCategoryWorker
    {
        private StorytellerDef defaultStoryteller;
        private DifficultyDef defaultDifficulty;
        private Difficulty defaultDifficultyValues;
        private bool? defaultPermadeath;
        private NoPauseOptions defaultNoPauseOptions;

        public DefaultSettingsCategoryWorker_Storyteller(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_Storyteller(def));
        }

        protected override bool GetCategorySetting(string key, out object value)
        {
            switch (key)
            {
                case Settings.STORYTELLER:
                    value = defaultStoryteller;
                    return true;
                case Settings.DIFFICULTY:
                    value = defaultDifficulty;
                    return true;
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
                case Settings.STORYTELLER:
                    defaultStoryteller = value as StorytellerDef;
                    return true;
                case Settings.DIFFICULTY:
                    defaultDifficulty = value as DifficultyDef;
                    return true;
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
            if (forced || defaultStoryteller == null)
            {
                defaultStoryteller = StorytellerDefOf.Cassandra;
            }
            if (forced || defaultDifficulty == null)
            {
                defaultDifficulty = DifficultyDefOf.Rough;
            }
            if (forced || defaultDifficultyValues == null)
            {
                defaultDifficultyValues = new Difficulty();
                defaultDifficultyValues.CopyFrom(defaultDifficulty);
            }
            if (forced || defaultPermadeath == null)
            {
                defaultPermadeath = false;
            }
            ModCompatibilityUtility_NoPause.ResetNoPauseOptions(ref defaultNoPauseOptions, forced);
        }

        protected override void ExposeCategorySettings()
        {
            Scribe_Defs_Silent.Look(ref defaultStoryteller, Settings.STORYTELLER);
            Scribe_Defs_Silent.Look(ref defaultDifficulty, Settings.DIFFICULTY);
            Scribe_Deep.Look(ref defaultDifficultyValues, Settings.DIFFICULTY_VALUES);
            Scribe_Values.Look(ref defaultPermadeath, Settings.PERMADEATH);
            ModCompatibilityUtility_NoPause.WriteNoPauseOptions(ref defaultNoPauseOptions);
            BackwardCompatibilityUtility.MigrateAnomalyPlaystyleSettings(defaultDifficultyValues);
        }
    }
}
