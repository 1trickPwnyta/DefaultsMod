using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.StockpileZones
{
    public class ZoneType : IExposable, IRenameable
    {
        public string Name;
        public StoragePriority Priority = StoragePriority.Normal;
        public ThingFilter filter;
        public StorageSettingsPreset Preset;

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

        public string RenamableLabel { get => Name; set => Name = value; }

        public string BaseLabel { get => RenamableLabel; }

        public string InspectLabel { get => RenamableLabel; }

        public void ExposeData()
        {
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
