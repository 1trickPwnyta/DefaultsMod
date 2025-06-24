using HarmonyLib;
using RimWorld;

namespace Defaults.Policies.ApparelPolicies
{
    [HarmonyPatch(typeof(OutfitDatabase))]
    [HarmonyPatch("GenerateStartingOutfits")]
    public static class Patch_OutfitDatabase
    {
        public static bool Prefix(OutfitDatabase __instance)
        {
            if (VanillaPolicyStore.loaded)
            {
                foreach (ApparelPolicy policy in DefaultsSettings.DefaultApparelPolicies)
                {
                    ApparelPolicy apparelPolicy = __instance.MakeNewOutfit();
                    apparelPolicy.label = policy.label;
                    apparelPolicy.CopyFrom(policy);
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
