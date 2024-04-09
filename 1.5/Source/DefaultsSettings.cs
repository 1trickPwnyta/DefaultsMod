using Defaults.Medicine;
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
        public static List<Schedule.Schedule> DefaultSchedules = new[] { new Schedule.Schedule() }.ToList();
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

        public static Schedule.Schedule GetNextDefaultSchedule()
        {
            if (NextScheduleIndex >= DefaultSchedules.Count)
            {
                NextScheduleIndex %= DefaultSchedules.Count;
            }
            return DefaultSchedules[NextScheduleIndex++];
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            if (listingStandard.ButtonTextLabeled("Defaults_Schedules".Translate(), "Defaults_SetDefaults".Translate()))
            {
                Find.WindowStack.Add(new Dialog_ScheduleSettings());
            }
            if (listingStandard.ButtonTextLabeled("Defaults_Medicine".Translate(), "Defaults_SetDefaults".Translate()))
            {
                Find.WindowStack.Add(new Dialog_MedicineSettings());
            }

            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref DefaultSchedules, "DefaultSchedules");
            if (DefaultSchedules == null)
            {
                DefaultSchedules = new[] { new Schedule.Schedule() }.ToList();
            }
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
        }
    }
}
