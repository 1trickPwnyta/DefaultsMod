using Defaults.Medicine;
using Defaults.Policies;
using Defaults.StockpileZones.Buildings;
using Defaults.WorkbenchBills;
using Defaults.WorldSettings;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults
{
    public static class BackwardCompatibilityUtility
    {
        private abstract class Compatibility_Policy<T> : Policy where T : Policy
        {
            public List<ThingDef> allowed;
            public FloatRange allowedHitPointsPercents;
            public QualityRange allowedQualityLevels;
            public List<SpecialThingFilterDef> disallowedSpecialFilters;
            public bool? locked;
            public ThingFilter filter;

            protected abstract ThingFilter GetFilter(T policy);

            public void Migrate()
            {
                if (filter != null)
                {
                    T policy = PolicyUtility.NewDefaultPolicy<T>(label);
                    GetFilter(policy).CopyAllowancesFrom(filter);
                }
                if (locked != null)
                {
                    T policy = PolicyUtility.GetDefaultPolicies<T>().First(p => p.label == label);
                    policy.SetLocked(locked.Value);
                }
            }

            public override void ExposeData()
            {
                base.ExposeData();
                Scribe_Values.Look(ref locked, "locked");
                ThingFilter compatibilityFilter = LoadExposedThingFilter(ref allowed, ref allowedHitPointsPercents, ref allowedQualityLevels, ref disallowedSpecialFilters);
                if (compatibilityFilter != null)
                {
                    filter = compatibilityFilter;
                }
            }

            public override void CopyFrom(Policy other)
            {
            }
        }

        private class Compatibility_ApparelPolicy : Compatibility_Policy<ApparelPolicy>
        {
            protected override string LoadKey => "Compatibility_ApparelPolicy";

            protected override ThingFilter GetFilter(ApparelPolicy policy) => policy.filter;
        }

        private class Compatibility_FoodPolicy : Compatibility_Policy<FoodPolicy>
        {
            protected override string LoadKey => "Compatibility_FoodPolicy";

            protected override ThingFilter GetFilter(FoodPolicy policy) => policy.filter;
        }

        private class Compatibility_ReadingPolicy : Compatibility_Policy<ReadingPolicy>
        {
            protected override string LoadKey => "Compatibility_ReadingPolicy";

            protected override ThingFilter GetFilter(ReadingPolicy policy) => null;
        }

        private static List<Compatibility_ApparelPolicy> compatibility_apparelPolicies;
        private static List<Compatibility_FoodPolicy> compatibility_foodPolicies;
        private static List<Compatibility_ReadingPolicy> compatibility_readingPolicies;
        private static ZoneType_Building compatibility_shelfSettings;

        public static ThingFilter LoadExposedThingFilter(ref List<ThingDef> allowed, ref FloatRange allowedHitPointsPercents, ref QualityRange allowedQualityLevels, ref List<SpecialThingFilterDef> disallowedSpecialFilters)
        {
            if (Scribe.mode != LoadSaveMode.Saving)
            {
                Scribe_Collections.Look(ref allowed, "Allowed", LookMode.Def);
                Scribe_Values.Look(ref allowedHitPointsPercents, "AllowedHitPointsPercents");
                Scribe_Values.Look(ref allowedQualityLevels, "AllowedQualityLevels");
                Scribe_Collections.Look(ref disallowedSpecialFilters, "DisallowedSpecialFilters", LookMode.Def);
            }

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (allowed != null)
                {
                    ThingFilter filter = new ThingFilter();
                    foreach (ThingDef def in allowed)
                    {
                        if (def != null)
                        {
                            filter.SetAllow(def, true);
                        }
                    }
                    filter.AllowedHitPointsPercents = allowedHitPointsPercents;
                    filter.AllowedQualityLevels = allowedQualityLevels;
                    foreach (SpecialThingFilterDef def in disallowedSpecialFilters)
                    {
                        if (def != null)
                        {
                            filter.SetAllow(def, false);
                        }
                    }
                    return filter;
                }
            }

            return null;
        }

        public static void MigrateAnomalyPlaystyleSettings(Difficulty difficultyValues)
        {
            if (ModsConfig.AnomalyActive && Scribe.mode == LoadSaveMode.LoadingVars)
            {
                AnomalyPlaystyleDef compatibility_anomalyPlaystyleDef = default;
                Scribe_Defs.Look(ref compatibility_anomalyPlaystyleDef, "DefaultAnomalyPlaystyle");
                if (compatibility_anomalyPlaystyleDef != null)
                {
                    difficultyValues.AnomalyPlaystyleDef = compatibility_anomalyPlaystyleDef;
                }
            }
        }

        public static void MigratePlanetOptions(ref PlanetOptions options)
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars && options == null)
            {
                options = new PlanetOptions();
                Scribe_Values.Look(ref options.DefaultPlanetCoverage, "DefaultPlanetCoverage", 0.3f);
                Scribe_Values.Look(ref options.DefaultOverallRainfall, "DefaultOverallRainfall", OverallRainfall.Normal);
                Scribe_Values.Look(ref options.DefaultOverallTemperature, "DefaultOverallTemperature", OverallTemperature.Normal);
                Scribe_Values.Look(ref options.DefaultOverallPopulation, "DefaultOverallPopulation", OverallPopulation.Normal);
                Scribe_Values.Look(ref options.DefaultPollution, "DefaultPollution", 0.05f);
            }
        }

        public static void MigrateMapOptions(ref MapOptions options)
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars && options == null)
            {
                options = new MapOptions();
                Scribe_Values.Look(ref options.DefaultMapSize, "DefaultMapSize", 250);
                Scribe_Values.Look(ref options.DefaultStartingSeason, "DefaultStartingSeason", Season.Undefined);
            }
        }

        public static void MigrateMedicineOptions(ref MedicineOptions options)
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars && options == null)
            {
                options = new MedicineOptions();
                Scribe_Values.Look(ref options.DefaultCareForColonist, "DefaultCareForColonist", MedicalCareCategory.Best);
                Scribe_Values.Look(ref options.DefaultCareForPrisoner, "DefaultCareForPrisoner", MedicalCareCategory.HerbalOrWorse);
                Scribe_Values.Look(ref options.DefaultCareForSlave, "DefaultCareForSlave", MedicalCareCategory.HerbalOrWorse);
                Scribe_Values.Look(ref options.DefaultCareForGhouls, "DefaultCareForGhouls", MedicalCareCategory.NoMeds);
                Scribe_Values.Look(ref options.DefaultCareForTamedAnimal, "DefaultCareForTamedAnimal", MedicalCareCategory.HerbalOrWorse);
                Scribe_Values.Look(ref options.DefaultCareForFriendlyFaction, "DefaultCareForFriendlyFaction", MedicalCareCategory.HerbalOrWorse);
                Scribe_Values.Look(ref options.DefaultCareForNeutralFaction, "DefaultCareForNeutralFaction", MedicalCareCategory.HerbalOrWorse);
                Scribe_Values.Look(ref options.DefaultCareForHostileFaction, "DefaultCareForHostileFaction", MedicalCareCategory.HerbalOrWorse);
                Scribe_Values.Look(ref options.DefaultCareForNoFaction, "DefaultCareForNoFaction", MedicalCareCategory.HerbalOrWorse);
                Scribe_Values.Look(ref options.DefaultCareForWildlife, "DefaultCareForWildlife", MedicalCareCategory.HerbalOrWorse);
                Scribe_Values.Look(ref options.DefaultCareForEntities, "DefaultCareForEntities", MedicalCareCategory.NoMeds);
            }
        }

        public static void MigrateGlobalBillOptions(ref GlobalBillOptions options)
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars && options == null)
            {
                options = new GlobalBillOptions();
                Scribe_Values.Look(ref options.DefaultBillIngredientSearchRadius, "DefaultBillIngredientSearchRadius", 999f);
                Scribe_Values.Look(ref options.DefaultBillAllowedSkillRange, "DefaultBillAllowedSkillRange", new IntRange(0, 20));
                Scribe_Defs.Look(ref options.DefaultBillStoreMode, "DefaultBillStoreMode");
            }
        }

        public static void MigrateApparelPolicies(List<ApparelPolicy> policies)
        {
            if (Scribe.mode != LoadSaveMode.Saving)
            {
                Scribe_Collections.Look(ref compatibility_apparelPolicies, "DefaultApparelPolicies", LookMode.Deep);
                if (compatibility_apparelPolicies != null)
                {
                    if (compatibility_apparelPolicies.Any(p => p.filter != null))
                    {
                        policies.Clear();
                    }
                    foreach (Compatibility_ApparelPolicy policy in compatibility_apparelPolicies)
                    {
                        policy.Migrate();
                    }
                }
            }
        }

        public static void MigrateFoodPolicies(List<FoodPolicy> policies)
        {
            if (Scribe.mode != LoadSaveMode.Saving)
            {
                Scribe_Collections.Look(ref compatibility_foodPolicies, "DefaultFoodPolicies", LookMode.Deep);
                if (compatibility_foodPolicies != null)
                {
                    if (compatibility_foodPolicies.Any(p => p.filter != null))
                    {
                        policies.Clear();
                    }
                    foreach (Compatibility_FoodPolicy policy in compatibility_foodPolicies)
                    {
                        policy.Migrate();
                    }
                }
            }
        }

        public static void MigrateReadingPolicies()
        {
            if (Scribe.mode != LoadSaveMode.Saving)
            {
                Scribe_Collections.Look(ref compatibility_readingPolicies, "DefaultReadingPolicies", LookMode.Deep);
                if (compatibility_readingPolicies != null)
                {
                    foreach (Compatibility_ReadingPolicy policy in compatibility_readingPolicies)
                    {
                        policy.Migrate();
                    }
                }
            }
        }

        public static void MigrateDefaultShelfSettings(ref Dictionary<ThingDef, ZoneType_Building> defaultBuildingStorageSettings)
        {
            if (Scribe.mode != LoadSaveMode.Saving)
            {
                Scribe_Deep.Look(ref compatibility_shelfSettings, "DefaultShelfSettings");
                if (compatibility_shelfSettings != null && Scribe.mode == LoadSaveMode.PostLoadInit)
                {
                    if (defaultBuildingStorageSettings == null)
                    {
                        defaultBuildingStorageSettings = new Dictionary<ThingDef, ZoneType_Building>();
                    }
                    defaultBuildingStorageSettings[ThingDefOf.Shelf] = new ZoneType_Building(ThingDefOf.Shelf, compatibility_shelfSettings);
                    defaultBuildingStorageSettings[ThingDefOf.ShelfSmall] = new ZoneType_Building(ThingDefOf.ShelfSmall, compatibility_shelfSettings);
                }
            }
        }

        public static void CleanFactions(List<FactionDef> factions)
        {
            factions.RemoveAll(f => f == null || !f.displayInFactionSelection);
        }
    }
}

namespace Defaults.StockpileZones.Shelves
{
    public class ZoneType_Shelf : ZoneType_Building
    {
        public List<ThingDef> allowed;
        public FloatRange allowedHitPointsPercents;
        public QualityRange allowedQualityLevels;
        public List<SpecialThingFilterDef> disallowedSpecialFilters;

        public override void ExposeData()
        {
            base.ExposeData();
            ThingFilter compatibilityFilter = BackwardCompatibilityUtility.LoadExposedThingFilter(ref allowed, ref allowedHitPointsPercents, ref allowedQualityLevels, ref disallowedSpecialFilters);
            if (compatibilityFilter != null)
            {
                filter = compatibilityFilter;
            }
        }
    }
}
