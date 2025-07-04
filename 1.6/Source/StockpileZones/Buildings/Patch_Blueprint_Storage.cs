using HarmonyLib;
using RimWorld;

namespace Defaults.StockpileZones.Buildings
{
    [HarmonyPatchCategory("Storage")]
    [HarmonyPatch(typeof(Blueprint_Storage))]
    [HarmonyPatch(nameof(Blueprint_Storage.PostMake))]
    public static class Patch_Blueprint_Storage
    {
        public static void Postfix(Blueprint_Storage __instance)
        {
            BuildingUtility.SetDefaultBuildingStorageSettings(__instance.BuildDef, __instance);
        }
    }
}
