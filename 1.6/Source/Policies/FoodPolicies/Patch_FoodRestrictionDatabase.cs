using HarmonyLib;
using RimWorld;

namespace Defaults.Policies.FoodPolicies
{
    [HarmonyPatch(typeof(FoodRestrictionDatabase))]
    [HarmonyPatch("GenerateStartingFoodRestrictions")]
    public static class Patch_FoodRestrictionDatabase
    {
        public static bool Prefix(FoodRestrictionDatabase __instance)
        {
            if (VanillaPolicyStore.loaded)
            {
                foreach (FoodPolicy policy in DefaultsSettings.DefaultFoodPolicies)
                {
                    FoodPolicy foodPolicy = __instance.MakeNewFoodRestriction();
                    foodPolicy.label = policy.label;
                    foodPolicy.CopyFrom(policy);
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
