using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.Policies.ApparelPolicies
{
    [HarmonyPatch(typeof(OutfitDatabase))]
    [HarmonyPatch("GenerateStartingOutfits")]
    public static class Patch_OutfitDatabase
    {
        public static bool Prefix(OutfitDatabase __instance)
        {
            if (Current.Game != null)
            {
                foreach (ApparelPolicy policy in DefaultsSettings.DefaultApparelPolicies)
                {
                    RimWorld.ApparelPolicy apparelPolicy = __instance.MakeNewOutfit();
                    apparelPolicy.label = policy.label;
                    apparelPolicy.filter.CopyAllowancesFrom(policy.filter);
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
