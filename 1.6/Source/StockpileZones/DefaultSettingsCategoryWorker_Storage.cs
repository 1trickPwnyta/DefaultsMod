using Defaults.StockpileZones.Shelves;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.StockpileZones
{
    public class DefaultSettingsCategoryWorker_Storage : DefaultSettingsCategoryWorker
    {
        private List<ZoneType> defaultStockpileZones;
        private ZoneType defaultShelfSettings;

        public DefaultSettingsCategoryWorker_Storage(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public ZoneType DefaultStockpileZone => defaultStockpileZones.FirstOrDefault(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Resources));

        public ZoneType DefaultDumpingStockpileZone => defaultStockpileZones.FirstOrDefault(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Dumping));

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_StockpileZones(def));
        }

        protected override bool GetCategorySetting(string key, out object value)
        {
            switch (key)
            {
                case Settings.STOCKPILE_ZONES:
                    value = defaultStockpileZones;
                    return true;
                case Settings.SHELF_SETTINGS:
                    value = defaultShelfSettings;
                    return true;
                default:
                    return base.GetCategorySetting(key, out value);
            }
        }

        protected override bool SetCategorySetting(string key, object value)
        {
            switch (key)
            {
                case Settings.STOCKPILE_ZONES:
                    defaultStockpileZones = value as List<ZoneType>;
                    return true;
                case Settings.SHELF_SETTINGS:
                    defaultShelfSettings = value as ZoneType;
                    return true;
                default:
                    return base.SetCategorySetting(key, value);
            }
        }

        public override void HandleNewDefs(IEnumerable<Def> defs)
        {
            foreach (ZoneType zone in defaultStockpileZones)
            {
                if (!zone.locked)
                {
                    foreach (ThingDef def in defs.OfType<ThingDef>())
                    {
                        switch (zone.Preset)
                        {
                            case StorageSettingsPreset.DumpingStockpile:
                                if (ThingCategoryDefOf.Corpses.DescendantThingDefs.Union(ThingCategoryDefOf.Chunks.DescendantThingDefs).Contains(def) || (ModsConfig.BiotechActive && def == ThingDefOf.Wastepack))
                                {
                                    zone.filter.SetAllow(def, true);
                                }
                                break;
                            case StorageSettingsPreset.CorpseStockpile:
                                if (ThingCategoryDefOf.Corpses.DescendantThingDefs.Contains(def))
                                {
                                    zone.filter.SetAllow(def, true);
                                }
                                break;
                            case StorageSettingsPreset.DefaultStockpile:
                            default:
                                if (ThingCategoryDefOf.Foods.DescendantThingDefs.Union(ThingCategoryDefOf.Manufactured.DescendantThingDefs).Union(ThingCategoryDefOf.ResourcesRaw.DescendantThingDefs).Union(ThingCategoryDefOf.Items.DescendantThingDefs).Union(ThingCategoryDefOf.Buildings.DescendantThingDefs).Union(ThingCategoryDefOf.Weapons.DescendantThingDefs).Union(ThingCategoryDefOf.Apparel.DescendantThingDefs).Union(ThingCategoryDefOf.BodyParts.DescendantThingDefs).Contains(def) && (!ModsConfig.BiotechActive || def != ThingDefOf.Wastepack))
                                {
                                    zone.filter.SetAllow(def, true);
                                }
                                break;
                        }
                    }

                    foreach (SpecialThingFilterDef def in defs.OfType<SpecialThingFilterDef>())
                    {
                        if (!def.allowedByDefault)
                        {
                            zone.filter.SetAllow(def, false);
                        }
                    }
                }
            }

            if (!defaultShelfSettings.locked)
            {
                foreach (ThingDef def in defs.OfType<ThingDef>())
                {
                    if (ThingCategoryDefOf.Foods.DescendantThingDefs.Union(ThingCategoryDefOf.Manufactured.DescendantThingDefs).Union(ThingCategoryDefOf.ResourcesRaw.DescendantThingDefs).Union(ThingCategoryDefOf.Items.DescendantThingDefs).Union(ThingCategoryDefOf.Weapons.DescendantThingDefs).Union(ThingCategoryDefOf.Apparel.DescendantThingDefs).Union(ThingCategoryDefOf.BodyParts.DescendantThingDefs).Contains(def) && (!ModsConfig.BiotechActive || def != ThingDefOf.Wastepack))
                    {
                        defaultShelfSettings.filter.SetAllow(def, true);
                    }
                }

                foreach (SpecialThingFilterDef def in defs.OfType<SpecialThingFilterDef>())
                {
                    if (!def.allowedByDefault)
                    {
                        defaultShelfSettings.filter.SetAllow(def, false);
                    }
                }
            }
        }

        protected override void ResetCategorySettings(bool forced)
        {
            if (forced || defaultStockpileZones == null)
            {
                defaultStockpileZones = new List<ZoneType>();
            }
            if (!defaultStockpileZones.Any(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Dumping)))
            {
                defaultStockpileZones.Insert(0, ZoneType.MakeBuiltInDumpingStockpileZone());
            }
            if (!defaultStockpileZones.Any(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Resources)))
            {
                defaultStockpileZones.Insert(0, ZoneType.MakeBuiltInStockpileZone());
            }
            if (forced || defaultShelfSettings == null)
            {
                defaultShelfSettings = ZoneType.MakeBuiltInShelfSettings();
            }
        }

        protected override void ExposeCategorySettings()
        {
            Scribe_Collections.Look(ref defaultStockpileZones, Settings.STOCKPILE_ZONES);
            Scribe_Deep.Look(ref defaultShelfSettings, Settings.SHELF_SETTINGS);

            if (defaultShelfSettings != null && !(defaultShelfSettings is ZoneType_Shelf))
            {
                defaultShelfSettings = new ZoneType_Shelf(defaultShelfSettings);
            }
        }
    }
}
