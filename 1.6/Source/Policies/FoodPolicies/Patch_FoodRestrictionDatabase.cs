using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.Policies.FoodPolicies
{
    [HarmonyPatch(typeof(FoodRestrictionDatabase))]
    [HarmonyPatch("GenerateStartingFoodRestrictions")]
    public static class Patch_FoodRestrictionDatabase
    {
        public static bool Prefix(FoodRestrictionDatabase __instance)
        {
            if (Current.Game != null)
            {
                foreach (FoodPolicy policy in DefaultsSettings.DefaultFoodPolicies)
                {
                    RimWorld.FoodPolicy foodPolicy = __instance.MakeNewFoodRestriction();
                    foodPolicy.label = policy.label;
                    foodPolicy.filter.CopyAllowancesFrom(policy.filter);
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
