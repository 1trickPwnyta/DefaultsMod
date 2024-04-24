using Defaults.Medicine;
using Defaults.Rewards;
using Defaults.Schedule;
using RimWorld;
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
        public static Dictionary<string, RewardPreference> DefaultRewardPreferences;
        public static HostilityResponseMode DefaultHostilityResponse = HostilityResponseMode.Flee;
        public static string DefaultPlantType;
        public static bool DefaultAutoRebuild = false;

        static DefaultsSettings()
        {
            InitializeDefaultSchedules();
            InitializeDefaultRewardPreferences();
            InitializeDefaultPlantType();
        }

        public static Schedule.Schedule GetNextDefaultSchedule()
        {
            if (NextScheduleIndex >= DefaultSchedules.Count)
            {
                NextScheduleIndex %= DefaultSchedules.Count;
            }
            Schedule.Schedule nextSchedule = DefaultSchedules[NextScheduleIndex++];
            LongEventHandler.ExecuteWhenFinished(DefaultsMod.Settings.Write);
            return nextSchedule;
        }

        private static void InitializeDefaultSchedules()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (DefaultSchedules == null)
                {
                    DefaultSchedules = new[] { new Schedule.Schedule() }.ToList();
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
            AutoRebuild.AutoRebuildUtility.DrawAutoRebuildButton(autoRebuildRect);

            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref DefaultSchedules, "DefaultSchedules");
            Scribe_Values.Look(ref NextScheduleIndex, "NextScheduleIndex");
            Scribe_Values.Look(ref DefaultCareForColonist, "DefaultCareForColonist");
            Scribe_Values.Look(ref DefaultCareForPrisoner, "DefaultCareForPrisoner");
            Scribe_Values.Look(ref DefaultCareForSlave, "DefaultCareForSlave");
            Scribe_Values.Look(ref DefaultCareForGhouls, "DefaultCareForGhouls");
            Scribe_Values.Look(ref DefaultCareForTamedAnimal, "DefaultCareForTamedAnimal");
            Scribe_Values.Look(ref DefaultCareForFriendlyFaction, "DefaultCareForFriendlyFaction");
            Scribe_Values.Look(ref DefaultCareForNeutralFaction, "DefaultCareForNeutralFaction");
            Scribe_Values.Look(ref DefaultCareForHostileFaction, "DefaultCareForHostileFaction");
            Scribe_Values.Look(ref DefaultCareForNoFaction, "DefaultCareForNoFaction");
            Scribe_Values.Look(ref DefaultCareForWildlife, "DefaultCareForWildlife");
            Scribe_Values.Look(ref DefaultCareForEntities, "DefaultCareForEntities");
            Scribe_Collections.Look(ref DefaultRewardPreferences, "DefaultRewardPreferences");
            Scribe_Values.Look(ref DefaultHostilityResponse, "DefaultHostilityResponse");
            Scribe_Values.Look(ref DefaultPlantType, "DefaultPlantType");
            Scribe_Values.Look(ref DefaultAutoRebuild, "DefaultAutoRebuild");
        }
    }
}
