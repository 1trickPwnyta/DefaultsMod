using Defaults.Policies.ApparelPolicies;
using Defaults.Policies.DrugPolicies;
using Defaults.MapSettings;
using Defaults.Medicine;
using Defaults.ResourceCategories;
using Defaults.Rewards;
using Defaults.Schedule;
using Defaults.StockpileZones;
using Defaults.Storyteller;
using Defaults.WorldSettings;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Defaults.Policies;

namespace Defaults
{
    public class DefaultsSettings : ModSettings
    {
        public static List<Schedule.Schedule> DefaultSchedules;
        private static int NextScheduleIndex = Mathf.Abs(Rand.Int);
        public static MedicalCareCategory DefaultCareForColonist;
        public static MedicalCareCategory DefaultCareForPrisoner;
        public static MedicalCareCategory DefaultCareForSlave;
        public static MedicalCareCategory DefaultCareForGhouls;
        public static MedicalCareCategory DefaultCareForTamedAnimal;
        public static MedicalCareCategory DefaultCareForFriendlyFaction;
        public static MedicalCareCategory DefaultCareForNeutralFaction;
        public static MedicalCareCategory DefaultCareForHostileFaction;
        public static MedicalCareCategory DefaultCareForNoFaction;
        public static MedicalCareCategory DefaultCareForWildlife;
        public static MedicalCareCategory DefaultCareForEntities;
        public static string DefaultMedicineToCarry;
        public static Dictionary<string, RewardPreference> DefaultRewardPreferences;
        public static HostilityResponseMode DefaultHostilityResponse;
        public static string DefaultPlantType;
        public static bool DefaultAutoRebuild;
        public static bool DefaultAutoHomeArea;
        public static List<string> DefaultExpandedResourceCategories;
        public static string DefaultStoryteller;
        public static string DefaultDifficulty;
        public static DifficultySub DefaultDifficultyValues;
        public static string DefaultAnomalyPlaystyle;
        public static bool DefaultPermadeath;
        public static List<ZoneType> DefaultStockpileZones;
        public static bool DefaultManualPriorities;
        public static float DefaultPlanetCoverage;
        public static OverallRainfall DefaultOverallRainfall;
        public static OverallTemperature DefaultOverallTemperature;
        public static OverallPopulation DefaultOverallPopulation;
        public static float DefaultPollution;
        public static List<string> DefaultFactions;
        public static int DefaultMapSize;
        public static Season DefaultStartingSeason;
        public static List<Policies.ApparelPolicies.ApparelPolicy> DefaultApparelPolicies;
        public static List<DrugPolicy> DefaultDrugPolicies;

        static DefaultsSettings()
        {
            ResetAllSettings();
        }

        public static void ResetAllSettings()
        {
            DefaultSchedules = null;
            DefaultCareForColonist = MedicalCareCategory.Best;
            DefaultCareForPrisoner = MedicalCareCategory.HerbalOrWorse;
            DefaultCareForSlave = MedicalCareCategory.HerbalOrWorse;
            DefaultCareForGhouls = MedicalCareCategory.NoMeds;
            DefaultCareForTamedAnimal = MedicalCareCategory.HerbalOrWorse;
            DefaultCareForFriendlyFaction = MedicalCareCategory.HerbalOrWorse;
            DefaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;
            DefaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;
            DefaultCareForNoFaction = MedicalCareCategory.HerbalOrWorse;
            DefaultCareForWildlife = MedicalCareCategory.HerbalOrWorse;
            DefaultCareForEntities = MedicalCareCategory.NoMeds;
            DefaultMedicineToCarry = null;
            DefaultRewardPreferences = null;
            DefaultHostilityResponse = HostilityResponseMode.Flee;
            DefaultPlantType = null;
            DefaultAutoRebuild = false;
            DefaultAutoHomeArea = true;
            DefaultExpandedResourceCategories = null;
            DefaultStoryteller = null;
            DefaultDifficulty = null;
            DefaultDifficultyValues = null;
            DefaultAnomalyPlaystyle = null;
            DefaultPermadeath = false;
            DefaultStockpileZones = null;
            DefaultManualPriorities = false;
            DefaultPlanetCoverage = 0.3f;
            DefaultOverallRainfall = OverallRainfall.Normal;
            DefaultOverallTemperature = OverallTemperature.Normal;
            DefaultOverallPopulation = OverallPopulation.Normal;
            DefaultPollution = 0.05f;
            DefaultFactions = null;
            DefaultMapSize = 250;
            DefaultStartingSeason = Season.Undefined;
            DefaultApparelPolicies = null;

            InitializeDefaultSchedules();
            InitializeDefaultMedicineToCarry();
            InitializeDefaultRewardPreferences();
            InitializeDefaultPlantType();
            InitializeDefaultExpandedResourceCategories();
            InitializeDefaultStorytellerSettings();
            InitializeDefaultStockpileZones();
            InitializeDefaultFactions();
            InitializeDefaultApparelPolicies();
            InitializeDefaultDrugPolicies();
        }

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
                if (ModsConfig.AnomalyActive && DefaultAnomalyPlaystyle == null)
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

        private static void InitializeDefaultApparelPolicies()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultApparelPolicies == null || DefaultApparelPolicies.Empty())
                {
                    DefaultApparelPolicies = new List<Policies.ApparelPolicies.ApparelPolicy>();

                    DefaultApparelPolicies.Add(new Policies.ApparelPolicies.ApparelPolicy(0, "OutfitAnything".Translate()));

                    Policies.ApparelPolicies.ApparelPolicy workerPolicy = new Policies.ApparelPolicies.ApparelPolicy(0, "OutfitWorker".Translate());
                    workerPolicy.filter.SetDisallowAll();
                    workerPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
                    foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        if (thingDef.apparel != null && thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Worker"))
                        {
                            workerPolicy.filter.SetAllow(thingDef, true);
                        }
                    }
                    DefaultApparelPolicies.Add(workerPolicy);

                    Policies.ApparelPolicies.ApparelPolicy soldierPolicy = new Policies.ApparelPolicies.ApparelPolicy(0, "OutfitSoldier".Translate());
                    soldierPolicy.filter.SetDisallowAll();
                    soldierPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
                    foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        if (thingDef.apparel != null && thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Soldier"))
                        {
                            soldierPolicy.filter.SetAllow(thingDef, true);
                        }
                    }
                    DefaultApparelPolicies.Add(soldierPolicy);

                    Policies.ApparelPolicies.ApparelPolicy nudistPolicy = new Policies.ApparelPolicies.ApparelPolicy(0, "OutfitNudist".Translate());
                    nudistPolicy.filter.SetDisallowAll();
                    nudistPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
                    foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        if (thingDef.apparel != null && (thingDef.apparel.defaultOutfitTags.NotNullAndContains("Nudist") || (!thingDef.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) && !thingDef.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))))
                        {
                            nudistPolicy.filter.SetAllow(thingDef, true);
                        }
                    }
                    DefaultApparelPolicies.Add(nudistPolicy);

                    if (ModsConfig.IdeologyActive)
                    {
                        Policies.ApparelPolicies.ApparelPolicy slavePolicy = new Policies.ApparelPolicies.ApparelPolicy(0, "OutfitSlave".Translate());
                        slavePolicy.filter.SetDisallowAll();
                        slavePolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
                        foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                        {
                            if (thingDef.apparel != null && thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Slave"))
                            {
                                slavePolicy.filter.SetAllow(thingDef, true);
                            }
                        }
                        DefaultApparelPolicies.Add(slavePolicy);
                    }
                }
            });
        }

        private static void InitializeDefaultDrugPolicies()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultDrugPolicies == null || DefaultDrugPolicies.Empty())
                {
                    DefaultDrugPolicies = new List<DrugPolicy>();

                    foreach (DrugPolicyDef def in DefDatabase<DrugPolicyDef>.AllDefs)
                    {
                        PolicyUtility.NewDrugPolicyFromDef(def);
                    }
                }
            });
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_StandardHighlight listing = new Listing_StandardHighlight();
            listing.Begin(inRect);

            if (listing.ButtonTextLabeledPct("Defaults_Storyteller".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_Storyteller());
            }

            if (listing.ButtonTextLabeledPct("Defaults_WorldSettings".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_WorldSettings());
            }

            if (listing.ButtonTextLabeledPct("Defaults_MapSettings".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_MapSettings());
            }

            if (listing.ButtonTextLabeledPct("Defaults_Schedules".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_ScheduleSettings());
            }

            if (listing.ButtonTextLabeledPct("Defaults_ApparelPolicies".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_ApparelPolicies(DefaultApparelPolicies.First()));
            }

            if (listing.ButtonTextLabeledPct("Defaults_DrugPolicies".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_DrugPolicies(DefaultDrugPolicies.First()));
            }

            if (listing.ButtonTextLabeledPct("Defaults_Medicine".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_MedicineSettings());
            }

            if (listing.ButtonTextLabeledPct("Defaults_Rewards".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_RewardsSettings());
            }

            if (listing.ButtonTextLabeledPct("Defaults_ResourceCategories".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_ResourceCategories());
            }

            if (listing.ButtonTextLabeledPct("Defaults_StockpileZones".Translate(), "Defaults_SetDefaults".Translate(), 0.75f, TextAnchor.MiddleLeft))
            {
                Find.WindowStack.Add(new Dialog_StockpileZones());
            }
            
            Rect hostilityResponseRect = listing.GetRect(30f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(hostilityResponseRect, "Defaults_HostilityResponse".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            hostilityResponseRect.x += hostilityResponseRect.width - 24;
            hostilityResponseRect.width = 24;
            HostilityResponse.HostilityResponseModeUtility.DrawResponseButton(hostilityResponseRect);

            Rect medCarryRect = listing.GetRect(30f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(medCarryRect, "Defaults_MedicineToCarry".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            medCarryRect.x += medCarryRect.width - 32;
            medCarryRect.width = 32;
            MedicineUtility.DrawMedicineButton(medCarryRect);

            Rect autoRebuildRect = listing.GetRect(30f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(autoRebuildRect, "Defaults_AutoRebuild".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            autoRebuildRect.x += autoRebuildRect.width - 24;
            autoRebuildRect.width = 24;
            autoRebuildRect.y += 3;
            autoRebuildRect.height = 24;
            PlaySettings.PlaySettingsUtility.DrawAutoRebuildButton(autoRebuildRect);

            Rect autoHomeAreaRect = listing.GetRect(30f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(autoHomeAreaRect, "Defaults_AutoHomeArea".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            autoHomeAreaRect.x += autoHomeAreaRect.width - 24;
            autoHomeAreaRect.width = 24;
            autoHomeAreaRect.y += 3;
            autoHomeAreaRect.height = 24;
            PlaySettings.PlaySettingsUtility.DrawAutoHomeAreaButton(autoHomeAreaRect);

            listing.CheckboxLabeled("Defaults_ManualPriorities".Translate(), ref DefaultManualPriorities);

            Rect plantTypeRect = listing.GetRect(30f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(plantTypeRect, "Defaults_PlantType".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            plantTypeRect.x += plantTypeRect.width - 24;
            plantTypeRect.width = 24;
            PlantType.PlantTypeUtility.DrawPlantButton(plantTypeRect);

            if (listing.ButtonText("Defaults_ResetAllSettings".Translate(), null, 0.5f))
            {
                Find.WindowStack.Add(new Dialog_MessageBox("Defaults_ConfirmResetAllSettings".Translate(), "Confirm".Translate(), ResetAllSettings, "GoBack".Translate(), null, null, true, ResetAllSettings, null, WindowLayer.Dialog));
            }

            listing.End();
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
            Scribe_Values.Look(ref DefaultMapSize, "DefaultMapSize", 250);
            Scribe_Values.Look(ref DefaultStartingSeason, "DefaultStartingSeason", Season.Undefined);
            Scribe_Collections.Look(ref DefaultApparelPolicies, "DefaultApparelPolicies", LookMode.Deep);
            Scribe_Collections.Look(ref DefaultDrugPolicies, "DefaultDrugPolicies", LookMode.Deep);
        }

        public static void ScribeThingFilter(ThingFilter filter)
        {
            List<string> allowed = filter.AllowedThingDefs.Select(d => d.defName).ToList();
            FloatRange allowedHitPointsPercents = filter.AllowedHitPointsPercents;
            QualityRange allowedQualities = filter.AllowedQualityLevels;
            List<string> disallowedSpecialFilters = ((List<SpecialThingFilterDef>)typeof(ThingFilter).Field("disallowedSpecialFilters").GetValue(filter)).Select(f => f.defName).ToList();
            Scribe_Collections.Look(ref allowed, "Allowed");
            Scribe_Values.Look(ref allowedHitPointsPercents, "AllowedHitPointsPercents");
            Scribe_Values.Look(ref allowedQualities, "AllowedQualityLevels");
            Scribe_Collections.Look(ref disallowedSpecialFilters, "DisallowedSpecialFilters");

            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                LongEventHandler.ExecuteWhenFinished(delegate
                {
                    filter.SetDisallowAll();
                    foreach (string allowedThingDef in allowed)
                    {
                        ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(allowedThingDef);
                        if (def != null)
                        {
                            filter.SetAllow(def, true);
                        }
                    }
                    filter.AllowedHitPointsPercents = allowedHitPointsPercents;
                    filter.AllowedQualityLevels = allowedQualities;
                    foreach (string disallowedSpecialFilter in disallowedSpecialFilters)
                    {
                        SpecialThingFilterDef def = DefDatabase<SpecialThingFilterDef>.GetNamedSilentFail(disallowedSpecialFilter);
                        if (def != null)
                        {
                            filter.SetAllow(def, false);
                        }
                    }
                });
            }
        }
    }
}
