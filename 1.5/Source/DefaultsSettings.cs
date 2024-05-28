using Defaults.Medicine;
using Defaults.ResourceCategories;
using Defaults.Rewards;
using Defaults.Schedule;
using Defaults.StockpileZones;
using Defaults.Storyteller;
using Defaults.WorldSettings;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults
{
    public class DefaultsSettings : ModSettings
    {
        public static List<Schedule.Schedule> DefaultSchedules;
        private static int NextScheduleIndex = Mathf.Abs(Rand.Int);
        public static MedicalCareCategory DefaultCareForColonist = MedicalCareCategory.Best;
        public static MedicalCareCategory DefaultCareForPrisoner = MedicalCareCategory.HerbalOrWorse;
        public static MedicalCareCategory DefaultCareForSlave = MedicalCareCategory.HerbalOrWorse;
        public static MedicalCareCategory DefaultCareForGhouls = MedicalCareCategory.NoMeds;
        public static MedicalCareCategory DefaultCareForTamedAnimal = MedicalCareCategory.HerbalOrWorse;
        public static MedicalCareCategory DefaultCareForFriendlyFaction = MedicalCareCategory.HerbalOrWorse;
        public static MedicalCareCategory DefaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;
        public static MedicalCareCategory DefaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;
        public static MedicalCareCategory DefaultCareForNoFaction = MedicalCareCategory.HerbalOrWorse;
        public static MedicalCareCategory DefaultCareForWildlife = MedicalCareCategory.HerbalOrWorse;
        public static MedicalCareCategory DefaultCareForEntities = MedicalCareCategory.NoMeds;
        public static string DefaultMedicineToCarry;
        public static Dictionary<string, RewardPreference> DefaultRewardPreferences;
        public static HostilityResponseMode DefaultHostilityResponse = HostilityResponseMode.Flee;
        public static string DefaultPlantType;
        public static bool DefaultAutoRebuild = false;
        public static bool DefaultAutoHomeArea = true;
        public static List<string> DefaultExpandedResourceCategories;
        public static string DefaultStoryteller;
        public static string DefaultDifficulty;
        public static DifficultySub DefaultDifficultyValues;
        public static string DefaultAnomalyPlaystyle;
        public static bool DefaultPermadeath = false;
        public static List<ZoneType> DefaultStockpileZones;
        public static bool DefaultManualPriorities = false;
        public static float DefaultPlanetCoverage = 0.3f;
        public static OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;
        public static OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;
        public static OverallPopulation DefaultOverallPopulation = OverallPopulation.Normal;
        public static float DefaultPollution = 0.05f;
        public static List<string> DefaultFactions;

        public static ZoneType DefaultStockpileZone
        {
            get
            {
                return DefaultStockpileZones.Where(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Resources)).FirstOrDefault();
            }
        }

        public static ZoneType DefaultDumpingStockpileZone
        {
            get
            {
                return DefaultStockpileZones.Where(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Dumping)).FirstOrDefault();
            }
        }

        static DefaultsSettings()
        {
            InitializeDefaultSchedules();
            InitializeDefaultMedicineToCarry();
            InitializeDefaultRewardPreferences();
            InitializeDefaultPlantType();
            InitializeDefaultExpandedResourceCategories();
            InitializeDefaultStorytellerSettings();
            InitializeDefaultStockpileZones();
            InitializeDefaultFactions();
        }

        public static Schedule.Schedule GetNextDefaultSchedule()
        {
            if (DefaultSchedules.Any(s => s.use))
            {
                if (NextScheduleIndex >= DefaultSchedules.Count)
                {
                    NextScheduleIndex %= DefaultSchedules.Count;
                }
                while (!DefaultSchedules[NextScheduleIndex].use)
                {
                    NextScheduleIndex++;
                    if (NextScheduleIndex >= DefaultSchedules.Count)
                    {
                        NextScheduleIndex %= DefaultSchedules.Count;
                    }
                }
                Schedule.Schedule nextSchedule = DefaultSchedules[NextScheduleIndex++];
                LongEventHandler.ExecuteWhenFinished(DefaultsMod.Settings.Write);
                return nextSchedule;
            } 
            else
            {
                return null;
            }
        }

        private static void InitializeDefaultSchedules()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultSchedules == null)
                {
                    DefaultSchedules = new[] { new Schedule.Schedule("Defaults_ScheduleName".Translate(1)) }.ToList();
                }
            });
        }

        private static void InitializeDefaultMedicineToCarry()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultMedicineToCarry == null)
                {
                    DefaultMedicineToCarry = ThingDefOf.MedicineIndustrial.defName;
                }
            });
        }

        private static void InitializeDefaultRewardPreferences()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultRewardPreferences == null)
                {
                    DefaultRewardPreferences = new Dictionary<string, RewardPreference>();
                    IEnumerable<FactionDef> allFactionDefs = DefDatabase<FactionDef>.AllDefs;
                    foreach (FactionDef def in allFactionDefs)
                    {
                        DefaultRewardPreferences.Add(def.defName, new RewardPreference());
                    }
                }
            });
        }

        private static void InitializeDefaultPlantType()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultPlantType == null)
                {
                    DefaultPlantType = ThingDefOf.Plant_Potato.defName;
                }
            });
        }

        private static void InitializeDefaultExpandedResourceCategories()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultExpandedResourceCategories == null)
                {
                    DefaultExpandedResourceCategories = new List<string>
                    {
                        ThingCategoryDefOf.Foods.defName
                    };
                }
            });
        }

        private static void InitializeDefaultStorytellerSettings()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultStoryteller == null)
                {
                    DefaultStoryteller = StorytellerDefOf.Cassandra.defName;
                }
                if (DefaultDifficulty == null)
                {
                    DefaultDifficulty = DifficultyDefOf.Rough.defName;
                }
                if (DefaultDifficultyValues == null)
                {
                    DefaultDifficultyValues = new DifficultySub();
                }
                if (DefaultAnomalyPlaystyle == null)
                {
                    DefaultAnomalyPlaystyle = AnomalyPlaystyleDefOf.Standard.defName;
                }
            });
        }

        private static void InitializeDefaultStockpileZones()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultStockpileZones == null)
                {
                    DefaultStockpileZones = new List<ZoneType>();
                }
                if (!DefaultStockpileZones.Any(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Dumping)))
                {
                    DefaultStockpileZones.Insert(0, ZoneType.MakeBuiltInDumpingStockpileZone());
                }
                if (!DefaultStockpileZones.Any(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Resources)))
                {
                    DefaultStockpileZones.Insert(0, ZoneType.MakeBuiltInStockpileZone());
                }
            });
        }

        private static void InitializeDefaultFactions()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultFactions == null)
                {
                    DefaultFactions = FactionsUtility.GetDefaultSelectableFactions().Select(f => f.defName).ToList();
                }
            });
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            if (listingStandard.ButtonTextLabeledPct("Defaults_Schedules".Translate(), "Defaults_SetDefaults".Translate(), 0.75f))
            {
                Find.WindowStack.Add(new Dialog_ScheduleSettings());
            }

            if (listingStandard.ButtonTextLabeledPct("Defaults_Medicine".Translate(), "Defaults_SetDefaults".Translate(), 0.75f))
            {
                Find.WindowStack.Add(new Dialog_MedicineSettings());
            }

            Rect medCarryRect = listingStandard.GetRect(30f);
            Widgets.Label(medCarryRect, "Defaults_MedicineToCarry".Translate());
            medCarryRect.x += medCarryRect.width - 32;
            medCarryRect.width = 32;
            MedicineUtility.DrawMedicineButton(medCarryRect);

            if (listingStandard.ButtonTextLabeledPct("Defaults_Rewards".Translate(), "Defaults_SetDefaults".Translate(), 0.75f))
            {
                Find.WindowStack.Add(new Dialog_RewardsSettings());
            }

            Rect hostilityResponseRect = listingStandard.GetRect(30f);
            Widgets.Label(hostilityResponseRect, "Defaults_HostilityResponse".Translate());
            hostilityResponseRect.x += hostilityResponseRect.width - 24;
            hostilityResponseRect.width = 24;
            HostilityResponse.HostilityResponseModeUtility.DrawResponseButton(hostilityResponseRect);

            Rect plantTypeRect = listingStandard.GetRect(30f);
            Widgets.Label(plantTypeRect, "Defaults_PlantType".Translate());
            plantTypeRect.x += plantTypeRect.width - 24;
            plantTypeRect.width = 24;
            PlantType.PlantTypeUtility.DrawPlantButton(plantTypeRect);

            Rect autoRebuildRect = listingStandard.GetRect(30f);
            Widgets.Label(autoRebuildRect, "Defaults_AutoRebuild".Translate());
            autoRebuildRect.x += autoRebuildRect.width - 24;
            autoRebuildRect.width = 24;
            autoRebuildRect.y += 3;
            autoRebuildRect.height = 24;
            PlaySettings.PlaySettingsUtility.DrawAutoRebuildButton(autoRebuildRect);

            Rect autoHomeAreaRect = listingStandard.GetRect(30f);
            Widgets.Label(autoHomeAreaRect, "Defaults_AutoHomeArea".Translate());
            autoHomeAreaRect.x += autoHomeAreaRect.width - 24;
            autoHomeAreaRect.width = 24;
            autoHomeAreaRect.y += 3;
            autoHomeAreaRect.height = 24;
            PlaySettings.PlaySettingsUtility.DrawAutoHomeAreaButton(autoHomeAreaRect);

            if (listingStandard.ButtonTextLabeledPct("Defaults_ResourceCategories".Translate(), "Defaults_SetDefaults".Translate(), 0.75f))
            {
                Find.WindowStack.Add(new Dialog_ResourceCategories());
            }

            if (listingStandard.ButtonTextLabeledPct("Defaults_Storyteller".Translate(), "Defaults_SetDefaults".Translate(), 0.75f))
            {
                Find.WindowStack.Add(new Dialog_Storyteller());
            }

            if (listingStandard.ButtonTextLabeledPct("Defaults_StockpileZones".Translate(), "Defaults_SetDefaults".Translate(), 0.75f))
            {
                Find.WindowStack.Add(new Dialog_StockpileZones());
            }

            listingStandard.CheckboxLabeled("Defaults_ManualPriorities".Translate(), ref DefaultManualPriorities);

            if (listingStandard.ButtonTextLabeledPct("Defaults_WorldSettings".Translate(), "Defaults_SetDefaults".Translate(), 0.75f))
            {
                Find.WindowStack.Add(new Dialog_WorldSettings());
            }

            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref DefaultSchedules, "DefaultSchedules");
            Scribe_Values.Look(ref NextScheduleIndex, "NextScheduleIndex", Mathf.Abs(Rand.Int));
            Scribe_Values.Look(ref DefaultCareForColonist, "DefaultCareForColonist", MedicalCareCategory.Best);
            Scribe_Values.Look(ref DefaultCareForPrisoner, "DefaultCareForPrisoner", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForSlave, "DefaultCareForSlave", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForGhouls, "DefaultCareForGhouls", MedicalCareCategory.NoMeds);
            Scribe_Values.Look(ref DefaultCareForTamedAnimal, "DefaultCareForTamedAnimal", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForFriendlyFaction, "DefaultCareForFriendlyFaction", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForNeutralFaction, "DefaultCareForNeutralFaction", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForHostileFaction, "DefaultCareForHostileFaction", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForNoFaction, "DefaultCareForNoFaction", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForWildlife, "DefaultCareForWildlife", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForEntities, "DefaultCareForEntities", MedicalCareCategory.NoMeds);
            Scribe_Values.Look(ref DefaultMedicineToCarry, "DefaultMedicineToCarry");
            Scribe_Collections.Look(ref DefaultRewardPreferences, "DefaultRewardPreferences");
            Scribe_Values.Look(ref DefaultHostilityResponse, "DefaultHostilityResponse", HostilityResponseMode.Flee);
            Scribe_Values.Look(ref DefaultPlantType, "DefaultPlantType");
            Scribe_Values.Look(ref DefaultAutoRebuild, "DefaultAutoRebuild", false);
            Scribe_Values.Look(ref DefaultAutoHomeArea, "DefaultAutoHomeArea", true);
            Scribe_Collections.Look(ref DefaultExpandedResourceCategories, "DefaultExpandedResourceCategories");
            Scribe_Values.Look(ref DefaultStoryteller, "DefaultStoryteller");
            Scribe_Values.Look(ref DefaultDifficulty, "DefaultDifficulty");
            Scribe_Deep.Look(ref DefaultDifficultyValues, "DefaultDifficultyValues");
            Scribe_Values.Look(ref DefaultAnomalyPlaystyle, "DefaultAnomalyPlaystyle");
            Scribe_Values.Look(ref DefaultPermadeath, "DefaultPermadeath", false);
            Scribe_Collections.Look(ref DefaultStockpileZones, "DefaultStockpileZones");
            Scribe_Values.Look(ref DefaultManualPriorities, "DefaultManualPriorities", false);
            Scribe_Values.Look(ref DefaultPlanetCoverage, "DefaultPlanetCoverage", 0.3f);
            Scribe_Values.Look(ref DefaultOverallRainfall, "DefaultOverallRainfall", OverallRainfall.Normal);
            Scribe_Values.Look(ref DefaultOverallTemperature, "DefaultOverallTemperature", OverallTemperature.Normal);
            Scribe_Values.Look(ref DefaultOverallPopulation, "DefaultOverallPopulation", OverallPopulation.Normal);
            Scribe_Values.Look(ref DefaultPollution, "DefaultPollution", 0.05f);
            Scribe_Collections.Look(ref DefaultFactions, "DefaultFactions");
        }
    }
}
