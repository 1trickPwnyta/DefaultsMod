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

namespace Defaults
{
    public class DefaultsSettings : ModSettings
    {
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
        public static List<ApparelPolicy> DefaultApparelPolicies;
        public static List<FoodPolicy> DefaultFoodPolicies;
        public static List<DrugPolicy> DefaultDrugPolicies;
        public static List<ReadingPolicy> DefaultReadingPolicies;
        public static HashSet<Policy> UnlockedPolicies;
        public static List<WorkbenchBillStore> DefaultWorkbenchBills;
        public static float DefaultBillIngredientSearchRadius;
        public static IntRange DefaultBillAllowedSkillRange;
        public static BillStoreModeDef DefaultBillStoreMode;

        public static List<string> KnownDLCs;
        private static List<string> PreviousFactionDefs;
        private static List<string> PreviousThingDefs;
        private static List<string> PreviousSpecialThingFilterDefs;

        private static readonly Vector2 settingsCategoryButtonSize = new Vector2(150f, 120f);
        private static readonly float settingsCategoryButtonMargin = 30f;
        private static readonly List<DefaultSettingsCategoryDef> categories = DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading;

        private static float y;
        private static Vector2 scrollPosition;
        private static readonly QuickSearchWidget search = new QuickSearchWidget();

        static DefaultsSettings()
        {
            ResetAllSettings();
        }

        public static void ResetAllSettings()
        {
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
            UnlockedPolicies = null;
            DefaultWorkbenchBills = null;
            DefaultBillIngredientSearchRadius = 999f;
            DefaultBillAllowedSkillRange = new IntRange(0, 20);
            DefaultBillStoreMode = null;

            InitializeDefaultRewardPreferences();
            InitializeDefaultExpandedResourceCategories();
            InitializeDefaultStorytellerSettings();
            InitializeDefaultStockpileZones();
            InitializeDefaultFactions();
            InitializeDefaultPolicies();
            InitializeDefaultWorkbenchBills();

            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.ResetSettings();
            }
        }

        public static void CheckForNewContent()
        {
            DLCUtility.HandleNewDLCs();

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
                foreach (ApparelPolicy policy in DefaultApparelPolicies)
                {
                    if (!policy.IsLocked())
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

                foreach (FoodPolicy policy in DefaultFoodPolicies)
                {
                    if (!policy.IsLocked())
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

                foreach (ReadingPolicy policy in DefaultReadingPolicies)
                {
                    if (!policy.IsLocked())
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
            });

            HandleNewDefs<SpecialThingFilterDef>(ref PreviousSpecialThingFilterDefs, newSpecialThingFilterDefs =>
            {
                foreach (ApparelPolicy policy in DefaultApparelPolicies)
                {
                    if (!policy.IsLocked())
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

                foreach (FoodPolicy policy in DefaultFoodPolicies)
                {
                    if (!policy.IsLocked())
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

                foreach (ReadingPolicy policy in DefaultReadingPolicies)
                {
                    if (!policy.IsLocked())
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
                        foreach (DefaultSettingsCategoryDef def in categories)
                        {
                            def.Worker.HandleNewDefs(newDefs.Select(d => DefDatabase<T>.GetNamed(d)));
                        }
                        newDefHandler(newDefs);
                    });
                }
            }
            previousDefs = currentDefs;
        }

        public static ZoneType DefaultStockpileZone => DefaultStockpileZones.FirstOrDefault(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Resources));

        public static ZoneType DefaultDumpingStockpileZone => DefaultStockpileZones.FirstOrDefault(z => z.DesignatorType == typeof(Designator_ZoneAddStockpile_Dumping));

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

        private static void InitializeDefaultPolicies()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (UnlockedPolicies == null)
                {
                    UnlockedPolicies = new HashSet<Policy>();
                }
                if (DefaultApparelPolicies.NullOrEmpty())
                {
                    UnlockedPolicies.RemoveWhere(p => p is ApparelPolicy);
                    DefaultApparelPolicies = VanillaPolicyStore.VanillaApparelPolicies.ListFullCopy();
                    UnlockedPolicies.Add(DefaultApparelPolicies[0]);
                }
                if (DefaultFoodPolicies.NullOrEmpty())
                {
                    UnlockedPolicies.RemoveWhere(p => p is FoodPolicy);
                    DefaultFoodPolicies = VanillaPolicyStore.VanillaFoodPolicies.ListFullCopy();
                    UnlockedPolicies.Add(DefaultFoodPolicies[0]);
                }
                if (DefaultDrugPolicies.NullOrEmpty())
                {
                    DefaultDrugPolicies = VanillaPolicyStore.VanillaDrugPolicies.ListFullCopy();
                }
                if (DefaultReadingPolicies.NullOrEmpty())
                {
                    UnlockedPolicies.RemoveWhere(p => p is ReadingPolicy);
                    DefaultReadingPolicies = VanillaPolicyStore.VanillaReadingPolicies.ListFullCopy();
                    UnlockedPolicies.Add(DefaultReadingPolicies[0]);
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

            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 3, inRect.yMax - 30f - 8f, inRect.width / 3, 30f), "Defaults_ResetAllSettings".Translate()))
            {
                Find.WindowStack.Add(new Dialog_MessageBox("Defaults_ConfirmResetAllSettings".Translate(), "Confirm".Translate(), ResetAllSettings, "GoBack".Translate(), null, null, true, ResetAllSettings));
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
            Scribe_Collections.Look(ref UnlockedPolicies, "UnlockedPolicies", LookMode.Reference);
            Scribe_Collections.Look(ref DefaultWorkbenchBills, "DefaultWorkbenchBills");
            Scribe_Values.Look(ref DefaultBillIngredientSearchRadius, "DefaultBillIngredientSearchRadius", 999f);
            Scribe_Values.Look(ref DefaultBillAllowedSkillRange, "DefaultBillAllowedSkillRange", new IntRange(0, 20));
            Scribe_Defs.Look(ref DefaultBillStoreMode, "DefaultBillStoreMode");

            Scribe_Collections.Look(ref KnownDLCs, "KnownDLCs");
            Scribe_Collections.Look(ref PreviousFactionDefs, "PreviousFactionDefs");
            Scribe_Collections.Look(ref PreviousThingDefs, "PreviousThingDefs");
            Scribe_Collections.Look(ref PreviousSpecialThingFilterDefs, "PreviousSpecialThingFilterDefs");

            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.ExposeData();
            }
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
