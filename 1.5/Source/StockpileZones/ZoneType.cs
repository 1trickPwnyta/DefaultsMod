using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.StockpileZones
{
    public class ZoneType : IExposable, IRenameable
    {
        private Type designatorType = typeof(Designator_ZoneAddStockpile_Custom);
        public string Name;
        public StoragePriority Priority = StoragePriority.Normal;
        public ThingFilter filter;
        public StorageSettingsPreset Preset;

        public static ZoneType MakeBuiltInStockpileZone()
        {
            ZoneType zoneType = new ZoneType(StorageSettingsPreset.DefaultStockpile.PresetName(), StorageSettingsPreset.DefaultStockpile);
            zoneType.designatorType = typeof(Designator_ZoneAddStockpile_Resources);
            return zoneType;
        }

        public static ZoneType MakeBuiltInDumpingStockpileZone()
        {
            ZoneType zoneType = new ZoneType(StorageSettingsPreset.DumpingStockpile.PresetName(), StorageSettingsPreset.DumpingStockpile);
            zoneType.designatorType = typeof(Designator_ZoneAddStockpile_Dumping);
            return zoneType;
        }

        public ZoneType()
        {

        }

        public ZoneType(string name, StorageSettingsPreset preset)
        {
            Name = name;
            filter = new ThingFilter();
            filter.SetFromPreset(preset);
            Preset = preset;
        }

        public ZoneType(string name, ZoneType other)
        {
            Name = name;
            Priority = other.Priority;
            filter = new ThingFilter();
            filter.CopyAllowancesFrom(other.filter);
            Preset = other.Preset;
        }

        public Type DesignatorType
        {
            get => designatorType;
        }

        public string RenamableLabel { get => Name; set => Name = value; }

        public string BaseLabel { get => RenamableLabel; }

        public string InspectLabel { get => RenamableLabel; }

        public string Desc
        {
            get
            {
                switch (Preset)
                {
                    case StorageSettingsPreset.DefaultStockpile: return "DesignatorZoneCreateStorageResourcesDesc".Translate();
                    case StorageSettingsPreset.DumpingStockpile: return "DesignatorZoneCreateStorageDumpingDesc".Translate();
                    case StorageSettingsPreset.CorpseStockpile: return "Defaults_CorpseStockpileZoneDesc".Translate();
                    default: return null;
                }
            }
        }

        public Texture2D Icon
        {
            get
            {
                switch (Preset)
                {
                    case StorageSettingsPreset.DefaultStockpile: return Dialog_StockpileZones.DefaultStockpileIcon;
                    case StorageSettingsPreset.DumpingStockpile: return Dialog_StockpileZones.DumpingStockpileIcon;
                    case StorageSettingsPreset.CorpseStockpile: return Dialog_StockpileZones.CorpseStockpileIcon;
                    default: return null;
                }
            }
        }

        public SoundDef Sound
        {
            get
            {
                switch (Preset)
                {
                    case StorageSettingsPreset.DefaultStockpile: return SoundDefOf.Designate_ZoneAdd_Stockpile;
                    case StorageSettingsPreset.DumpingStockpile: return SoundDefOf.Designate_ZoneAdd_Dumping;
                    case StorageSettingsPreset.CorpseStockpile: return SoundDefOf.Designate_ZoneAdd_Dumping;
                    default: return null;
                }
            }
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref designatorType, "designatorType");
            Scribe_Values.Look(ref Name, "Name");
            Scribe_Values.Look(ref Priority, "Priority");
            Scribe_Values.Look(ref Preset, "Preset");

            List<string> allowed = null;
            FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;
            QualityRange allowedQualities = QualityRange.All;
            List<string> disallowedSpecialFilters = null;
            if (filter != null)
            {
                allowed = filter.AllowedThingDefs.Select(d => d.defName).ToList();
                allowedHitPointsPercents = filter.AllowedHitPointsPercents;
                allowedQualities = filter.AllowedQualityLevels;
                disallowedSpecialFilters = ((List<SpecialThingFilterDef>)typeof(ThingFilter).Field("disallowedSpecialFilters").GetValue(filter)).Select(f => f.defName).ToList();
            }
            Scribe_Collections.Look(ref allowed, "Allowed");
            Scribe_Values.Look(ref allowedHitPointsPercents, "AllowedHitPointsPercents");
            Scribe_Values.Look(ref allowedQualities, "AllowedQualityLevels");
            Scribe_Collections.Look(ref disallowedSpecialFilters, "DisallowedSpecialFilters");
            
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                LongEventHandler.ExecuteWhenFinished(delegate
                {
                    filter = new ThingFilter();
                    foreach (string allowedThingDef in allowed)
                    {
                        filter.SetAllow(DefDatabase<ThingDef>.GetNamed(allowedThingDef), true);
                    }
                    filter.AllowedHitPointsPercents = allowedHitPointsPercents;
                    filter.AllowedQualityLevels = allowedQualities;
                    foreach (string disallowedSpecialFilter in disallowedSpecialFilters)
                    {
                        filter.SetAllow(DefDatabase<SpecialThingFilterDef>.GetNamed(disallowedSpecialFilter), false);
                    }
                });
            }
        }
    }
}
