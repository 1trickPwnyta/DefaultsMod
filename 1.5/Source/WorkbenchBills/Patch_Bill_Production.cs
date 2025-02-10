using HarmonyLib;
using RimWorld;

namespace Defaults.WorkbenchBills
{
    // Patched manually in mod constructor
    public static class Patch_Bill_Production_ctor
    {
        public static void Postfix(Bill_Production __instance)
        {
            __instance.ingredientSearchRadius = DefaultsSettings.DefaultBillIngredientSearchRadius;
            __instance.allowedSkillRange = DefaultsSettings.DefaultBillAllowedSkillRange;
            __instance.SetStoreMode(DefaultsSettings.DefaultBillStoreMode);
        }
    }

    [HarmonyPatch(typeof(Bill_Production))]
    [HarmonyPatch(nameof(Bill_Production.ShouldDoNow))]
    public static class Patch_Bill_Production_ShouldDoNow
    {
        public static void Postfix(Bill_Production __instance, ref bool __result)
        {
            __result &= !__instance.IsSuspendedOrUnavailable();
        }
    }
}
