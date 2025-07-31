using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.WorkbenchBills
{
    [HarmonyPatchCategory("WorkbenchBills")]
    [HarmonyPatch(typeof(Bill_Production))]
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new[] { typeof(RecipeDef), typeof(Precept_ThingStyle) })]
    public static class Patch_Bill_Production_ctor
    {
        public static bool enabled = true;

        public static void Postfix(Bill_Production __instance)
        {
            if (enabled)
            {
                GlobalBillOptions options = Settings.Get<GlobalBillOptions>(Settings.GLOBAL_BILL_OPTIONS);

                __instance.ingredientSearchRadius = options.DefaultBillIngredientSearchRadius;

                __instance.allowedSkillRange.min = options.DefaultBillAllowedSkillRange.min;
                // To support mods that increase max skill level, count 20 as unlimited and therefore do not change the max
                if (options.DefaultBillAllowedSkillRange.max < 20)
                {
                    __instance.allowedSkillRange.max = options.DefaultBillAllowedSkillRange.max;
                }

                __instance.SetStoreMode(options.DefaultBillStoreMode);
            }
        }
    }

    [HarmonyPatchCategory("WorkbenchBills")]
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
