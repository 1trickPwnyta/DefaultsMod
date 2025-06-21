using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.Policies.DrugPolicies
{
    [HarmonyPatch(typeof(DrugPolicyDatabase))]
    [HarmonyPatch("GenerateStartingDrugPolicies")]
    public static class Patch_DrugPolicyDatabase
    {
        public static bool Prefix(DrugPolicyDatabase __instance)
        {
            if (Current.Game != null)
            {
                foreach (DrugPolicy policy in DefaultsSettings.DefaultDrugPolicies)
                {
                    DrugPolicy drugPolicy = __instance.MakeNewDrugPolicy();
                    drugPolicy.label = policy.label;
                    drugPolicy.CopyFrom(policy);
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
