using RimWorld;
using System.Linq;
using Verse;

namespace Defaults.StockpileZones.Buildings
{
    // Patched manually in mod constructor
    public static class Patch_Building_PostMake
    {
        public static void Postfix(Building __instance)
        {
            if (!(__instance is IStoreSettingsParent parent))
            {
                parent = __instance.AllComps.OfType<IStoreSettingsParent>().FirstOrDefault();
            }
            if (parent != null)
            {
                BuildingUtility.SetDefaultBuildingStorageSettings(__instance.def, parent);
            }
        }
    }
}
