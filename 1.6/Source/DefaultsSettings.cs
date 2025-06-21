using Defaults.Rewards;
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
using System;
using Defaults.StockpileZones.Shelves;
using Defaults.WorkbenchBills;
using Defaults.BabyFeeding;

namespace Defaults
{
    public class DefaultsSettings : ModSettings
    {
        public static List<Schedule.Schedule> DefaultSchedules;
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
        public static Dictionary<string, RewardPreference> DefaultRewardPreferences;
        
        public static List<string> DefaultExpandedResourceCategories;
        public static string DefaultStoryteller;
        public static string DefaultDifficulty;
        public static DifficultySub DefaultDifficultyValues;
        public static string DefaultAnomalyPlaystyle;
        public static bool DefaultPermadeath;
        public static List<ZoneType> DefaultStockpileZones;
        public static ZoneType DefaultShelfSettings;
        public static float DefaultPlanetCoverage;
        public static OverallRainfall DefaultOverallRainfall;
        public static OverallTemperature DefaultOverallTemperature;
        public static OverallPopulation DefaultOverallPopulation;
        public static float DefaultPollution;
        public static List<string> DefaultFactions;
        public static bool DefaultFactionsLock;
        public static int DefaultMapSize;
        public static Season DefaultStartingSeason;
        public static List<Policies.ApparelPolicies.ApparelPolicy> DefaultApparelPolicies;
        public static List<Policies.FoodPolicies.FoodPolicy> DefaultFoodPolicies;
        public static List<DrugPolicy> DefaultDrugPolicies;
        public static List<Policies.ReadingPolicies.ReadingPolicy> DefaultReadingPolicies;
        public static List<WorkbenchBillStore> DefaultWorkbenchBills;
        public static float DefaultBillIngredientSearchRadius;
        public static IntRange DefaultBillAllowedSkillRange;
        public static BillStoreModeDef DefaultBillStoreMode;
        public static BabyFeedingOptions DefaultBabyFeedingOptions;

        private static int NextScheduleIndex = Mathf.Abs(Rand.Int);
        private static List<string> PreviousFactionDefs;
        private static List<string> PreviousThingDefs;
        private static List<string> PreviousSpecialThingFilterDefs;

        private static readonly Vector2 settingsCategoryButtonSize = new Vector2(150f, 120f);
        private static readonly float settingsCategoryButtonMargin = 30f;
        private static readonly List<DefaultSettingsCategoryDef> categories = DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading;

        private static float y;
        private static Vector2 scrollPosition;
        private static QuickSearchWidget search = new QuickSearchWidget();

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
            DefaultRewardPreferences = null;
            
            DefaultExpandedResourceCategories = null;
            DefaultStoryteller = null;
            DefaultDifficulty = null;
            DefaultDifficultyValues = null;
            DefaultAnomalyPlaystyle = null;
            DefaultPermadeath = false;
            DefaultStockpileZones = null;
            DefaultShelfSettings = null;
            DefaultPlanetCoverage = 0.3f;
            DefaultOverallRainfall = OverallRainfall.Normal;
            DefaultOverallTemperature = OverallTemperature.Normal;
            DefaultOverallPopulation = OverallPopulation.Normal;
            DefaultPollution = 0.05f;
            DefaultFactions = null;
            DefaultFactionsLock = false;
            DefaultMapSize = 250;
            DefaultStartingSeason = Season.Undefined;
            DefaultApparelPolicies = null;
            DefaultFoodPolicies = null;
            DefaultDrugPolicies = null;
            DefaultReadingPolicies = null;
            DefaultWorkbenchBills = null;
            DefaultBillIngredientSearchRadius = 999f;
            DefaultBillAllowedSkillRange = new IntRange(0, 20);
            DefaultBillStoreMode = null;
            DefaultBabyFeedingOptions = null;

            InitializeDefaultSchedules();
            InitializeDefaultRewardPreferences();
            InitializeDefaultExpandedResourceCategories();
            InitializeDefaultStorytellerSettings();
            InitializeDefaultStockpileZones();
            InitializeDefaultFactions();
            InitializeDefaultApparelPolicies();
            InitializeDefaultFoodPolicies();
            InitializeDefaultDrugPolicies();
            InitializeDefaultReadingPolicies();
            InitializeDefaultWorkbenchBills();
            InitializeDefaultBabyFeedingOptions();

            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.SetDefaults();
            }
        }

        public static void CheckForNewContent()
        {
            ExpansionDef newDLC = ModsConfig.LastInstalledExpansion;
            if (newDLC != null && ModsConfig.IsExpansionNew(newDLC.linkedMod))
            {
                switch (newDLC.defName)
                {
                    case "Ideology":
                        PromptToAddPolicies(newDLC, new Policy[]
                        {
                            VanillaPolicyStore.GetVanillaApparelPolicy("OutfitSlave".Translate()), 
                            VanillaPolicyStore.GetVanillaFoodPolicy("FoodRestrictionVegetarian".Translate()), 
                            VanillaPolicyStore.GetVanillaFoodPolicy("FoodRestrictionCarnivore".Translate()), 
                            VanillaPolicyStore.GetVanillaFoodPolicy("FoodRestrictionCannibal".Translate()), 
                            VanillaPolicyStore.GetVanillaFoodPolicy("FoodRestrictionInsectMeat".Translate())
                        });
                        break;
                    case "Biotech":
                        //PromptToRemoveHemogenPacksFromFoodPolicies();
                        break;
                    case "Anomaly":
                        PromptToAddPolicies(newDLC, new Policy[]
                        {
                            VanillaPolicyStore.GetVanillaReadingPolicy("TomePolicy".Translate())
                        });
                        break;
                    case "Odyssey":
                        PromptToAddPolicies(newDLC, new Policy[]
                        {
                            VanillaPolicyStore.GetVanillaApparelPolicy("OutfitSpacefarer".Translate()),
                            VanillaPolicyStore.GetVanillaReadingPolicy("MapPolicy".Translate())
                        });
                        break;
                }
            }

            HandleNewDefs<FactionDef>(ref PreviousFactionDefs, newFactionDefs =>
            {
                if (!DefaultFactionsLock)
                {
                    DefaultFactions.AddRange(FactionsUtility.GetDefaultSelectableFactions().Where(f => newFactionDefs.Contains(f.defName)).Select(f => f.defName));
                    DefaultFactions.RemoveAll(f => newFactionDefs.Select(d => DefDatabase<FactionDef>.GetNamed(d).replacesFaction?.defName).Contains(f));
                }
            });

            HandleNewDefs<ThingDef>(ref PreviousThingDefs, newThingDefs =>
            {
                foreach (Policies.ApparelPolicies.ApparelPolicy policy in DefaultApparelPolicies)
                {
                    if (!policy.locked)
                    {
                        foreach (string defName in newThingDefs)
                        {
                            ThingDef def = DefDatabase<ThingDef>.GetNamed(defName);
                            if (ThingCategoryDefOf.Apparel.DescendantThingDefs.Contains(def))
                            {
                                policy.filter.SetAllow(def, true);
                            }
                        }
                    }
                }

                foreach (Policies.FoodPolicies.FoodPolicy policy in DefaultFoodPolicies)
                {
                    if (!policy.locked)
                    {
                        foreach (string defName in newThingDefs)
                        {
                            ThingDef def = DefDatabase<ThingDef>.GetNamed(defName);
                            if (def.GetStatValueAbstract(StatDefOf.Nutrition, null) > 0f)
                            {
                                policy.filter.SetAllow(def, true);
                            }
                            if (ModsConfig.BiotechActive && def == ThingDefOf.HemogenPack)
                            {
                                policy.filter.SetAllow(def, false);
                            }
                        }
                    }
                }

                foreach (Policies.ReadingPolicies.ReadingPolicy policy in DefaultReadingPolicies)
                {
                    if (!policy.locked)
                    {
                        foreach (string defName in newThingDefs)
                        {
                            ThingDef def = DefDatabase<ThingDef>.GetNamed(defName);
                            if (def.HasComp<CompBook>())
                            {
                                policy.defFilter.SetAllow(def, true);
                            }
                        }
                    }
                }

                foreach (ZoneType zone in DefaultStockpileZones)
                {
                    if (!zone.locked)
                    {
                        foreach (string defName in newThingDefs)
                        {
                            ThingDef def = DefDatabase<ThingDef>.GetNamed(defName);
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
                    }
                }

                if (!DefaultShelfSettings.locked)
                {
                    foreach (string defName in newThingDefs)
                    {
                        ThingDef def = DefDatabase<ThingDef>.GetNamed(defName);
                        if (ThingCategoryDefOf.Foods.DescendantThingDefs.Union(ThingCategoryDefOf.Manufactured.DescendantThingDefs).Union(ThingCategoryDefOf.ResourcesRaw.DescendantThingDefs).Union(ThingCategoryDefOf.Items.DescendantThingDefs).Union(ThingCategoryDefOf.Weapons.DescendantThingDefs).Union(ThingCategoryDefOf.Apparel.DescendantThingDefs).Union(ThingCategoryDefOf.BodyParts.DescendantThingDefs).Contains(def) && (!ModsConfig.BiotechActive || def != ThingDefOf.Wastepack))
                        {
                            DefaultShelfSettings.filter.SetAllow(def, true);
                        }
                    }
                }

                foreach (BillTemplate bill in DefaultWorkbenchBills.SelectMany(s => s.bills))
                {
                    if (!bill.locked)
                    {
                        foreach (string defName in newThingDefs)
                        {
                            ThingDef def = DefDatabase<ThingDef>.GetNamed(defName);
                            bill.ingredientFilter.SetAllow(def, true);
                        }
                    }
                }

                if (!DefaultBabyFeedingOptions.locked)
                {
                    foreach (string defName in newThingDefs)
                    {
                        ThingDef def = DefDatabase<ThingDef>.GetNamed(defName);
                        if (ITab_Pawn_Feeding.BabyConsumableFoods.Contains(def))
                        {
                            DefaultBabyFeedingOptions.AllowedConsumables.Add(def);
                        }
                    }
                }
            });

            HandleNewDefs<SpecialThingFilterDef>(ref PreviousSpecialThingFilterDefs, newSpecialThingFilterDefs =>
            {
                foreach (Policies.ApparelPolicies.ApparelPolicy policy in DefaultApparelPolicies)
                {
                    if (!policy.locked)
                    {
                        foreach (string defName in newSpecialThingFilterDefs)
                        {
                            SpecialThingFilterDef def = DefDatabase<SpecialThingFilterDef>.GetNamed(defName);
                            if (!def.allowedByDefault)
                            {
                                policy.filter.SetAllow(def, false);
                            }
                        }
                    }
                }

                foreach (Policies.FoodPolicies.FoodPolicy policy in DefaultFoodPolicies)
                {
                    if (!policy.locked)
                    {
                        foreach (string defName in newSpecialThingFilterDefs)
                        {
                            SpecialThingFilterDef def = DefDatabase<SpecialThingFilterDef>.GetNamed(defName);
                            if (!def.allowedByDefault)
                            {
                                policy.filter.SetAllow(def, false);
                            }
                        }
                    }
                }

                foreach (Policies.ReadingPolicies.ReadingPolicy policy in DefaultReadingPolicies)
                {
                    if (!policy.locked)
                    {
                        foreach (string defName in newSpecialThingFilterDefs)
                        {
                            SpecialThingFilterDef def = DefDatabase<SpecialThingFilterDef>.GetNamed(defName);
                            if (!def.allowedByDefault && ThingCategoryDefOf.BookEffects.DescendantSpecialThingFilterDefs.Contains(def))
                            {
                                policy.effectFilter.SetAllow(def, false);
                            }
                        }
                    }
                }

                foreach (ZoneType zone in DefaultStockpileZones)
                {
                    if (!zone.locked)
                    {
                        foreach (string defName in newSpecialThingFilterDefs)
                        {
                            SpecialThingFilterDef def = DefDatabase<SpecialThingFilterDef>.GetNamed(defName);
                            if (!def.allowedByDefault)
                            {
                                zone.filter.SetAllow(def, false);
                            }
                        }
                    }
                }

                if (!DefaultShelfSettings.locked)
                {
                    foreach (string defName in newSpecialThingFilterDefs)
                    {
                        SpecialThingFilterDef def = DefDatabase<SpecialThingFilterDef>.GetNamed(defName);
                        if (!def.allowedByDefault)
                        {
                            DefaultShelfSettings.filter.SetAllow(def, false);
                        }
                    }
                }

                foreach (BillTemplate bill in DefaultWorkbenchBills.SelectMany(s => s.bills))
                {
                    if (!bill.locked)
                    {
                        foreach (string defName in newSpecialThingFilterDefs)
                        {
                            SpecialThingFilterDef def = DefDatabase<SpecialThingFilterDef>.GetNamed(defName);
                            bill.ingredientFilter.SetAllow(def, false);
                        }
                    }
                }
            });

            LongEventHandler.ExecuteWhenFinished(DefaultsMod.Settings.Write);
        }

        private static void PromptToAddPolicies(ExpansionDef dlc, IEnumerable<Policy> newPolicies)
        {
            List<Policy> missingPolicies = newPolicies.Where(p => 
                (p is ApparelPolicy && !DefaultApparelPolicies.Any(a => a.label.EqualsIgnoreCase(p.label))) 
                || (p is FoodPolicy && !DefaultFoodPolicies.Any(a => a.label.EqualsIgnoreCase(p.label)))
                || (p is DrugPolicy && !DefaultDrugPolicies.Any(a => a.label.EqualsIgnoreCase(p.label)))
                || (p is ReadingPolicy && !DefaultReadingPolicies.Any(a => a.label.EqualsIgnoreCase(p.label)))).ToList();

            if (missingPolicies.Any())
            {
                Find.WindowStack.Add(new Dialog_PickMany("Defaults_NewDLC".Translate(dlc.LabelCap), "Defaults_NewPoliciesDLC".Translate(dlc.LabelCap), new[]
                {
                    new Tuple<string, IEnumerable<TaggedString>>("Defaults_ApparelPolicies", missingPolicies.Where(p => p is ApparelPolicy).Select(p => (TaggedString)p.label)),
                    new Tuple<string, IEnumerable<TaggedString>>("Defaults_FoodPolicies", missingPolicies.Where(p => p is FoodPolicy).Select(p => (TaggedString)p.label)),
                    new Tuple<string, IEnumerable<TaggedString>>("Defaults_DrugPolicies", missingPolicies.Where(p => p is DrugPolicy).Select(p => (TaggedString)p.label)),
                    new Tuple<string, IEnumerable<TaggedString>>("Defaults_ReadingPolicies", missingPolicies.Where(p => p is ReadingPolicy).Select(p => (TaggedString)p.label))
                }, true, results =>
                {
                    foreach (Tuple<string, TaggedString> result in results)
                    {
                        switch (result.Item1)
                        {
                            case "Defaults_ApparelPolicies":
                                Policies.ApparelPolicies.ApparelPolicy apparelPolicy = new Policies.ApparelPolicies.ApparelPolicy(0, result.Item2);
                                apparelPolicy.filter.CopyAllowancesFrom(VanillaPolicyStore.GetVanillaApparelPolicy(result.Item2).filter);
                                DefaultApparelPolicies.Add(apparelPolicy);
                                break;
                            case "Defaults_FoodPolicies":
                                Policies.FoodPolicies.FoodPolicy foodPolicy = new Policies.FoodPolicies.FoodPolicy(0, result.Item2);
                                foodPolicy.filter.CopyAllowancesFrom(VanillaPolicyStore.GetVanillaFoodPolicy(result.Item2).filter);
                                DefaultFoodPolicies.Add(foodPolicy);
                                break;
                            case "Defaults_DrugPolicies":
                                DefaultDrugPolicies.Add(VanillaPolicyStore.GetVanillaDrugPolicy(result.Item2));
                                break;
                            case "Defaults_ReadingPolicies":
                                Policies.ReadingPolicies.ReadingPolicy readingPolicy = new Policies.ReadingPolicies.ReadingPolicy(0, result.Item2);
                                readingPolicy.CopyFrom(VanillaPolicyStore.GetVanillaReadingPolicy(result.Item2));
                                DefaultReadingPolicies.Add(readingPolicy);
                                break;
                        }
                    }
                    LongEventHandler.ExecuteWhenFinished(DefaultsMod.Settings.Write);
                }));
            }
        }

        private static void HandleNewDefs<T>(ref List<string> previousDefs, Action<List<string>> newDefHandler) where T : Def
        {
            List<string> currentDefs = DefDatabase<T>.AllDefsListForReading.Select(d => d.defName).ToList();
            if (previousDefs != null)
            {
                List<string> newDefs = currentDefs.Except(previousDefs).ToList();
                if (newDefs.Count > 0)
                {
                    LongEventHandler.ExecuteWhenFinished(delegate
                    {
                        newDefHandler(newDefs);
                    });
                }
            }
            previousDefs = currentDefs;
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
                if (DefaultShelfSettings == null)
                {
                    DefaultShelfSettings = ZoneType.MakeBuiltInShelfSettings();
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

                // Remove any non-selectable factions from the default settings that got in due to bug
                DefaultFactions.RemoveAll(f => 
                {
                    FactionDef d = DefDatabase<FactionDef>.GetNamedSilentFail(f);
                    return d == null || !d.displayInFactionSelection;
                });
            });
        }

        private static void InitializeDefaultApparelPolicies()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultApparelPolicies == null || DefaultApparelPolicies.Empty())
                {
                    DefaultApparelPolicies = new List<Policies.ApparelPolicies.ApparelPolicy>();

                    Policies.ApparelPolicies.ApparelPolicy anythingPolicy = new Policies.ApparelPolicies.ApparelPolicy(0, "OutfitAnything".Translate());
                    anythingPolicy.locked = false;
                    DefaultApparelPolicies.Add(anythingPolicy);

                    Policies.ApparelPolicies.ApparelPolicy workerPolicy = new Policies.ApparelPolicies.ApparelPolicy(0, "OutfitWorker".Translate());
                    workerPolicy.filter.SetDisallowAll();
                    workerPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
                    foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        if (thingDef.apparel != null && ((thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Worker")) || thingDef.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
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
                        if (thingDef.apparel != null && ((thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Soldier")) || thingDef.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
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

                    if (ModsConfig.OdysseyActive)
                    {
                        Policies.ApparelPolicies.ApparelPolicy spacefarerPolicy = new Policies.ApparelPolicies.ApparelPolicy(0, "OutfitSpacefarer".Translate());
                        spacefarerPolicy.filter.SetDisallowAll();
                        spacefarerPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
                        foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                        {
                            if (thingDef.apparel != null && ((thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Spacefarer")) || thingDef.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
                            {
                                spacefarerPolicy.filter.SetAllow(thingDef, true);
                            }
                        }
                        DefaultApparelPolicies.Add(spacefarerPolicy);
                    }
                }
            });
        }

        private static void InitializeDefaultFoodPolicies()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultFoodPolicies == null || DefaultFoodPolicies.Empty())
                {
                    DefaultFoodPolicies = new List<Policies.FoodPolicies.FoodPolicy>();

                    Policies.FoodPolicies.FoodPolicy lavishPolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionLavish".Translate());
                    lavishPolicy.locked = false;
                    DefaultFoodPolicies.Add(lavishPolicy);

                    Policies.FoodPolicies.FoodPolicy finePolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionFine".Translate());
                    foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        if (thingDef.ingestible != null && thingDef.ingestible.preferability >= FoodPreferability.MealLavish && thingDef != ThingDefOf.InsectJelly)
                        {
                            finePolicy.filter.SetAllow(thingDef, false);
                        }
                    }
                    if (ModsConfig.BiotechActive)
                    {
                        finePolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
                    }
                    DefaultFoodPolicies.Add(finePolicy);

                    Policies.FoodPolicies.FoodPolicy simplePolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionSimple".Translate());
                    foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        if (thingDef.ingestible != null && thingDef.ingestible.preferability >= FoodPreferability.MealFine && thingDef != ThingDefOf.InsectJelly)
                        {
                            simplePolicy.filter.SetAllow(thingDef, false);
                        }
                    }
                    if (ModsConfig.BiotechActive)
                    {
                        simplePolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
                    }
                    DefaultFoodPolicies.Add(simplePolicy);

                    Policies.FoodPolicies.FoodPolicy pastePolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionPaste".Translate());
                    foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        if (thingDef.ingestible != null && thingDef.ingestible.preferability >= FoodPreferability.MealSimple && thingDef != ThingDefOf.MealNutrientPaste && thingDef != ThingDefOf.InsectJelly)
                        {
                            pastePolicy.filter.SetAllow(thingDef, false);
                        }
                    }
                    if (ModsConfig.BiotechActive)
                    {
                        pastePolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
                    }
                    DefaultFoodPolicies.Add(pastePolicy);

                    Policies.FoodPolicies.FoodPolicy rawPolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionRaw".Translate());
                    foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                    {
                        if (thingDef.ingestible != null && thingDef.ingestible.preferability >= FoodPreferability.MealAwful)
                        {
                            rawPolicy.filter.SetAllow(thingDef, false);
                        }
                    }
                    rawPolicy.filter.SetAllow(ThingDefOf.Chocolate, false);
                    if (ModsConfig.BiotechActive)
                    {
                        rawPolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
                    }
                    DefaultFoodPolicies.Add(rawPolicy);

                    Policies.FoodPolicies.FoodPolicy nothingPolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionNothing".Translate());
                    nothingPolicy.filter.SetDisallowAll(null, null);
                    DefaultFoodPolicies.Add(nothingPolicy);

                    if (ModsConfig.IdeologyActive)
                    {
                        Policies.FoodPolicies.FoodPolicy vegetarianPolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionVegetarian".Translate());
                        foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                        {
                            if (FoodUtility.UnacceptableVegetarian(thingDef))
                            {
                                vegetarianPolicy.filter.SetAllow(thingDef, false);
                            }
                        }
                        vegetarianPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCarnivore, false);
                        vegetarianPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCannibal, false);
                        vegetarianPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowInsectMeat, false);
                        if (ModsConfig.BiotechActive)
                        {
                            vegetarianPolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
                        }
                        DefaultFoodPolicies.Add(vegetarianPolicy);

                        Policies.FoodPolicies.FoodPolicy carnivorePolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionCarnivore".Translate());
                        foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                        {
                            if (!FoodUtility.UnacceptableCarnivore(thingDef) && FoodUtility.GetMeatSourceCategory(thingDef) != MeatSourceCategory.Humanlike)
                            {
                                if (!thingDef.IsCorpse)
                                {
                                    continue;
                                }
                                ThingDef sourceDef = thingDef.ingestible.sourceDef;
                                bool flag;
                                if (sourceDef == null)
                                {
                                    flag = false;
                                }
                                else
                                {
                                    RaceProperties race = sourceDef.race;
                                    bool? flag2 = (race != null) ? new bool?(race.Humanlike) : null;
                                    bool flag3 = true;
                                    flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
                                }
                                if (!flag)
                                {
                                    continue;
                                }
                            }
                            carnivorePolicy.filter.SetAllow(thingDef, false);
                        }
                        carnivorePolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowVegetarian, false);
                        carnivorePolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCannibal, false);
                        carnivorePolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowInsectMeat, false);
                        if (ModsConfig.BiotechActive)
                        {
                            carnivorePolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
                        }
                        DefaultFoodPolicies.Add(carnivorePolicy);

                        Policies.FoodPolicies.FoodPolicy cannibalPolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionCannibal".Translate());
                        foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                        {
                            if (!FoodUtility.MaybeAcceptableCannibalDef(thingDef))
                            {
                                cannibalPolicy.filter.SetAllow(thingDef, false);
                            }
                        }
                        cannibalPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowVegetarian, false);
                        cannibalPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCarnivore, false);
                        cannibalPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowInsectMeat, false);
                        if (ModsConfig.BiotechActive)
                        {
                            cannibalPolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
                        }
                        DefaultFoodPolicies.Add(cannibalPolicy);

                        Policies.FoodPolicies.FoodPolicy insectMeatPolicy = new Policies.FoodPolicies.FoodPolicy(0, "FoodRestrictionInsectMeat".Translate());
                        foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
                        {
                            if (!FoodUtility.MaybeAcceptableInsectMeatEatersDef(thingDef))
                            {
                                insectMeatPolicy.filter.SetAllow(thingDef, false);
                            }
                        }
                        insectMeatPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowVegetarian, false);
                        insectMeatPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCarnivore, false);
                        insectMeatPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowCannibal, false);
                        if (ModsConfig.BiotechActive)
                        {
                            insectMeatPolicy.filter.SetAllow(ThingDefOf.HemogenPack, false);
                        }
                        DefaultFoodPolicies.Add(insectMeatPolicy);
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

        private static void InitializeDefaultReadingPolicies()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultReadingPolicies == null || DefaultReadingPolicies.Empty())
                {
                    DefaultReadingPolicies = new List<Policies.ReadingPolicies.ReadingPolicy>();

                    Policies.ReadingPolicies.ReadingPolicy allPolicy = PolicyUtility.NewReadingPolicy();
                    allPolicy.label = "AllReadingPolicy".Translate();
                    allPolicy.locked = false;

                    Policies.ReadingPolicies.ReadingPolicy textbookPolicy = PolicyUtility.NewReadingPolicy();
                    textbookPolicy.label = "TextbookPolicy".Translate();
                    textbookPolicy.defFilter.SetDisallowAll();
                    textbookPolicy.defFilter.SetAllow(ThingDefOf.TextBook, true);

                    Policies.ReadingPolicies.ReadingPolicy schematicPolicy = PolicyUtility.NewReadingPolicy();
                    schematicPolicy.label = "SchematicPolicy".Translate();
                    schematicPolicy.defFilter.SetDisallowAll();
                    schematicPolicy.defFilter.SetAllow(ThingDefOf.Schematic, true);

                    Policies.ReadingPolicies.ReadingPolicy novelPolicy = PolicyUtility.NewReadingPolicy();
                    novelPolicy.label = "NovelPolicy".Translate();
                    novelPolicy.defFilter.SetDisallowAll();
                    novelPolicy.defFilter.SetAllow(ThingDefOf.Novel, true);

                    if (ModsConfig.AnomalyActive)
                    {
                        Policies.ReadingPolicies.ReadingPolicy tomePolicy = PolicyUtility.NewReadingPolicy();
                        tomePolicy.label = "TomePolicy".Translate();
                        tomePolicy.defFilter.SetDisallowAll();
                        tomePolicy.defFilter.SetAllow(ThingDefOf.Tome, true);
                    }

                    if (ModsConfig.OdysseyActive)
                    {
                        Policies.ReadingPolicies.ReadingPolicy mapPolicy = PolicyUtility.NewReadingPolicy();
                        mapPolicy.label = "MapPolicy".Translate();
                        mapPolicy.defFilter.SetDisallowAll();
                        mapPolicy.defFilter.SetAllow(ThingDefOf.Map, true);
                    }

                    Policies.ReadingPolicies.ReadingPolicy nonePolicy = PolicyUtility.NewReadingPolicy();
                    nonePolicy.label = "NoneReadingPolicy".Translate();
                    nonePolicy.defFilter.SetDisallowAll();
                }
            });
        }

        private static void InitializeDefaultWorkbenchBills()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultWorkbenchBills == null)
                {
                    DefaultWorkbenchBills = new List<WorkbenchBillStore>();
                }
                if (DefaultBillStoreMode == null)
                {
                    DefaultBillStoreMode = BillStoreModeDefOf.BestStockpile;
                }
            });
        }

        private static void InitializeDefaultBabyFeedingOptions()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultBabyFeedingOptions == null)
                {
                    DefaultBabyFeedingOptions = new BabyFeedingOptions();
                }
            });
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            search.OnGUI(new Rect(inRect.xMax - 250f - 20f, inRect.y - 15f - QuickSearchWidget.WidgetHeight, 250f, QuickSearchWidget.WidgetHeight));

            float width = settingsCategoryButtonSize.x * 4 + settingsCategoryButtonMargin * 3;
            Rect outRect = new Rect(inRect.x + (inRect.width - width) / 2, inRect.y, inRect.width - (inRect.width - width) / 2, inRect.height - 30f - 10f);
            Rect viewRect = new Rect(0f, 0f, width, y);
            y = 0f;
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

            IEnumerable<DefaultSettingsCategoryDef> categories = DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading;
            if (search.filter.Active)
            {
                categories = categories.Where(c => c.Matches(search.filter));
            }

            if (search.filter.Active)
            {
                using (new TextBlock(GameFont.Medium))
                {
                    Widgets.Label(new Rect(viewRect.x, y, viewRect.width, Text.LineHeight), ref y, "Defaults_SearchResults".Translate());
                    y += 10f;
                }
            }
            DoCategories(viewRect, ref y, categories);
            if (search.filter.Active)
            {
                IEnumerable<DefaultSettingDef> settings = DefDatabase<DefaultSettingDef>.AllDefsListForReading.Where(s =>
                    s.Matches(search.filter)
                );
                if (settings.Any())
                {
                    DoSettings(viewRect, ref y, settings);
                }
            }

            Widgets.EndScrollView();

            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 3, inRect.yMax - 30f, inRect.width / 3, 30f), "Defaults_ResetAllSettings".Translate()))
            {
                Find.WindowStack.Add(new Dialog_MessageBox("Defaults_ConfirmResetAllSettings".Translate(), "Confirm".Translate(), ResetAllSettings, "GoBack".Translate(), null, null, true, ResetAllSettings, null, WindowLayer.Dialog));
            }
        }

        private static void DoCategories(Rect rect, ref float y, IEnumerable<DefaultSettingsCategoryDef> categories)
        {
            if (search.filter.Active)
            {
                Widgets.Label(new Rect(rect.x, y, rect.width, Text.LineHeight), ref y, "Defaults_DefaultSettingsCategories".Translate());
                Widgets.DrawLineHorizontal(rect.x, y, rect.width);
                y += 10f;
            }

            float x = 0f;
            foreach (DefaultSettingsCategoryDef def in categories)
            {
                Rect buttonRect = new Rect(x, y, settingsCategoryButtonSize.x, settingsCategoryButtonSize.y);
                def.Worker.DoButton(buttonRect);
                x += buttonRect.width + settingsCategoryButtonMargin;
                if (x + settingsCategoryButtonSize.x > rect.xMax)
                {
                    x = 0f;
                    y += settingsCategoryButtonSize.y + settingsCategoryButtonMargin;
                }
            }
            if (x > 0)
            {
                y += settingsCategoryButtonSize.y + settingsCategoryButtonMargin;
            }
        }

        private static void DoSettings(Rect rect, ref float y, IEnumerable<DefaultSettingDef> settings)
        {
            Widgets.Label(new Rect(rect.x, y, rect.width, Text.LineHeight), ref y, "Defaults_DefaultSettings".Translate());
            Widgets.DrawLineHorizontal(rect.x, y, rect.width);
            y += 10f;

            Listing_Standard listing = new Listing_StandardHighlight() { maxOneColumn = true };
            listing.Begin(new Rect(rect.x, y, rect.width, rect.height - y));

            foreach (DefaultSettingDef def in settings)
            {
                Rect rowRect = listing.GetRect(30f);
                def.Worker.DoSetting(rowRect);
            }

            y += listing.CurHeight;
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
            Scribe_Collections.Look(ref DefaultRewardPreferences, "DefaultRewardPreferences");
            Scribe_Collections.Look(ref DefaultExpandedResourceCategories, "DefaultExpandedResourceCategories");
            Scribe_Values.Look(ref DefaultStoryteller, "DefaultStoryteller");
            Scribe_Values.Look(ref DefaultDifficulty, "DefaultDifficulty");
            Scribe_Deep.Look(ref DefaultDifficultyValues, "DefaultDifficultyValues");
            Scribe_Values.Look(ref DefaultAnomalyPlaystyle, "DefaultAnomalyPlaystyle");
            Scribe_Values.Look(ref DefaultPermadeath, "DefaultPermadeath", false);
            Scribe_Collections.Look(ref DefaultStockpileZones, "DefaultStockpileZones");
            Scribe_Deep.Look(ref DefaultShelfSettings, "DefaultShelfSettings");
            Scribe_Values.Look(ref DefaultPlanetCoverage, "DefaultPlanetCoverage", 0.3f);
            Scribe_Values.Look(ref DefaultOverallRainfall, "DefaultOverallRainfall", OverallRainfall.Normal);
            Scribe_Values.Look(ref DefaultOverallTemperature, "DefaultOverallTemperature", OverallTemperature.Normal);
            Scribe_Values.Look(ref DefaultOverallPopulation, "DefaultOverallPopulation", OverallPopulation.Normal);
            Scribe_Values.Look(ref DefaultPollution, "DefaultPollution", 0.05f);
            Scribe_Collections.Look(ref DefaultFactions, "DefaultFactions");
            Scribe_Values.Look(ref DefaultFactionsLock, "DefaultFactionsLock", true);
            Scribe_Values.Look(ref DefaultMapSize, "DefaultMapSize", 250);
            Scribe_Values.Look(ref DefaultStartingSeason, "DefaultStartingSeason", Season.Undefined);
            Scribe_Collections.Look(ref DefaultApparelPolicies, "DefaultApparelPolicies", LookMode.Deep);
            Scribe_Collections.Look(ref DefaultFoodPolicies, "DefaultFoodPolicies", LookMode.Deep);
            Scribe_Collections.Look(ref DefaultDrugPolicies, "DefaultDrugPolicies", LookMode.Deep);
            Scribe_Collections.Look(ref DefaultReadingPolicies, "DefaultReadingPolicies", LookMode.Deep);
            Scribe_Collections.Look(ref DefaultWorkbenchBills, "DefaultWorkbenchBills");
            Scribe_Values.Look(ref DefaultBillIngredientSearchRadius, "DefaultBillIngredientSearchRadius", 999f);
            Scribe_Values.Look(ref DefaultBillAllowedSkillRange, "DefaultBillAllowedSkillRange", new IntRange(0, 20));
            Scribe_Defs.Look(ref DefaultBillStoreMode, "DefaultBillStoreMode");
            Scribe_Deep.Look(ref DefaultBabyFeedingOptions, "DefaultBabyFeedingOptions");

            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.ExposeData();
            }

            Scribe_Collections.Look(ref PreviousFactionDefs, "PreviousFactionDefs");
            Scribe_Collections.Look(ref PreviousThingDefs, "PreviousThingDefs");
            Scribe_Collections.Look(ref PreviousSpecialThingFilterDefs, "PreviousSpecialThingFilterDefs");
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

                    if (filter == DefaultShelfSettings.filter)
                    {
                        if (DefaultShelfSettings != null && !(DefaultShelfSettings is ZoneType_Shelf))
                        {
                            DefaultShelfSettings = new ZoneType_Shelf(DefaultShelfSettings);
                        }
                    }
                });
            }
        }
    }
}
