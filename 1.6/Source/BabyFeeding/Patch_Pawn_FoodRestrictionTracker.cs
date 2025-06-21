using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Defaults.BabyFeeding
{
    [HarmonyPatch(typeof(Pawn_FoodRestrictionTracker))]
    [HarmonyPatch("TrySetupAllowedBabyFoodTypes")]
    public static class Patch_Pawn_FoodRestrictionTracker
    {
        public static void Prefix(ref Dictionary<ThingDef, bool> ___allowedBabyFoodTypes)
        {
            if (___allowedBabyFoodTypes == null)
            {
                ___allowedBabyFoodTypes = new Dictionary<ThingDef, bool>();
                foreach (ThingDef def in ITab_Pawn_Feeding.BabyConsumableFoods)
                {
                    ___allowedBabyFoodTypes.Add(def, DefaultsSettings.DefaultBabyFeedingOptions.AllowedConsumables.Contains(def));
                }
            }
        }
    }
}
