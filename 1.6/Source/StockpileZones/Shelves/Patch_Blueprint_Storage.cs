using HarmonyLib;
using RimWorld;

namespace Defaults.StockpileZones.Shelves
{
    [HarmonyPatch(typeof(Blueprint_Storage))]
    [HarmonyPatch(nameof(Blueprint_Storage.PostMake))]
    public static class Patch_Blueprint_Storage
    {
        public static void Postfix(Blueprint_Storage __instance)
        {
            PatchUtility_Building_Storage.SetDefaultShelfSettings(__instance.BuildDef, __instance);
        }
    }
}
