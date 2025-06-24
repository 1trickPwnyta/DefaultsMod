using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.BabyFeeding
{
    public class DefaultSettingsCategoryWorker_BabyFeeding : DefaultSettingsCategoryWorker
    {
        private BabyFeedingOptions defaultBabyFeedingOptions;

        public DefaultSettingsCategoryWorker_BabyFeeding(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_BabyFeedingSettings(def));
        }

        protected override bool GetCategorySetting(string key, out object value)
        {
            switch (key)
            {
                case Settings.BABY_FEEDING:
                    value = defaultBabyFeedingOptions;
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
                case Settings.BABY_FEEDING:
                    defaultBabyFeedingOptions = value as BabyFeedingOptions;
                    return true;
                default:
                    return false;
            }
        }

        public override void HandleNewDefs(IEnumerable<Def> defs)
        {
            if (!defaultBabyFeedingOptions.locked)
            {
                foreach (ThingDef def in defs.OfType<ThingDef>())
                {
                    if (ITab_Pawn_Feeding.BabyConsumableFoods.Contains(def))
                    {
                        defaultBabyFeedingOptions.AllowedConsumables.Add(def);
                    }
                }
            }
        }

        public override void ResetSettings()
        {
            base.ResetSettings();
            defaultBabyFeedingOptions = new BabyFeedingOptions();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref defaultBabyFeedingOptions, Settings.BABY_FEEDING);
        }
    }
}
