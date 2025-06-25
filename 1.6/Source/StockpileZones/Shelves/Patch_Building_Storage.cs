using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.StockpileZones.Shelves
{
    [HarmonyPatch(typeof(Building_Storage))]
    [HarmonyPatch(nameof(Building_Storage.PostMake))]
    public static class Patch_Building_Storage
    {
        public static void Postfix(Building_Storage __instance)
        {
            PatchUtility_Building_Storage.SetDefaultShelfSettings(__instance.def, __instance);
        }
    }

    public static class PatchUtility_Building_Storage
    {
        public static void SetDefaultShelfSettings(ThingDef def, IStoreSettingsParent parent)
        {
            if (def.IsShelf())
            {
                ZoneType shelfSettings = Settings.Get<ZoneType>(Settings.SHELF_SETTINGS);
                StorageSettings settings = parent.GetStoreSettings();
                settings.Priority = shelfSettings.Priority;
                settings.filter.CopyAllowancesFrom(shelfSettings.filter);
            }
        }
    }
}
