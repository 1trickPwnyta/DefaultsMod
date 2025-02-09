using HarmonyLib;
using RimWorld;

namespace Defaults.WorkbenchBills
{
    [HarmonyPatch(typeof(Bill_Production))]
    [HarmonyPatch(nameof(Bill_Production.ShouldDoNow))]
    public static class Patch_Bill_Production
    {
        public static void Postfix(Bill_Production __instance, ref bool __result)
        {
            __result &= !__instance.IsSuspendedOrUnavailable();
        }
    }
}
