using Defaults.Compatibility;
using Defaults.Defs;
using Defaults.Workers;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.WorldSettings
{
    public class DefaultSettingsCategoryWorker_World : DefaultSettingsCategoryWorker
    {
        private MapOptions defaultMapOptions;
        private List<FactionDef> defaultFactions;
        private bool? defaultFactionsLock;

        public DefaultSettingsCategoryWorker_World(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_WorldSettings(def));
        }

        protected override bool GetCategorySetting(string key, out object value)
        {
            switch (key)
            {
                case Settings.MAP:
                    value = defaultMapOptions;
                    return true;
                case Settings.FACTIONS:
                    value = defaultFactions;
                    return true;
                case Settings.FACTIONS_LOCK:
                    value = defaultFactionsLock.Value;
                    return true;
                default:
                    return base.GetCategorySetting(key, out value);
            }
        }

        protected override bool SetCategorySetting(string key, object value)
        {
            switch (key)
            {
                case Settings.MAP:
                    defaultMapOptions = value as MapOptions;
                    return true;
                case Settings.FACTIONS:
                    defaultFactions = value as List<FactionDef>;
                    return true;
                case Settings.FACTIONS_LOCK:
                    defaultFactionsLock = (bool)value;
                    return true;
                default:
                    return base.SetCategorySetting(key, value);
            }
        }

        public override void HandleNewDefs(IEnumerable<Def> defs)
        {
            if (!defaultFactionsLock.Value)
            {
                defaultFactions.AddRange(FactionsUtility.GetDefaultSelectableFactions().Where(f => defs.Contains(f)));
                defaultFactions.RemoveAll(f => defs.OfType<FactionDef>().Select(d => d.replacesFaction).Contains(f));
            }
        }

        protected override void ResetCategorySettings(bool forced)
        {
            if (forced || defaultMapOptions == null)
            {
                defaultMapOptions = new MapOptions();
            }
            if (forced || defaultFactions == null)
            {
                defaultFactions = FactionsUtility.GetDefaultSelectableFactions();
            }
            if (forced || defaultFactionsLock == null)
            {
                defaultFactionsLock = false;
            }

            BackwardCompatibilityUtility.CleanFactions(defaultFactions);
        }

        protected override void ExposeCategorySettings()
        {
            Scribe_Deep.Look(ref defaultMapOptions, Settings.MAP);
            if (!ModCompatibilityUtility_FactionXenotypeRandomizer.ScribeDefaultFactions(ref defaultFactions))
            {
                Scribe_Collections_Silent.Look(ref defaultFactions, Settings.FACTIONS);
            }
            Scribe_Values.Look(ref defaultFactionsLock, Settings.FACTIONS_LOCK);
            BackwardCompatibilityUtility.MigrateMapOptions(ref defaultMapOptions);
        }

        protected override void PostExposeData()
        {
            BackwardCompatibilityUtility.MigratePlanetOptions();
        }
    }
}
