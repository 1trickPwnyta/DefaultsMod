using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.StockpileZones
{
    public static class PatchUtility_DesignationCategoryDef
    {
        public static IEnumerable<Designator> AddDesignators(IEnumerable<Designator> __result)
        {
            Designator builtInStockpileDesignator = __result.FirstOrDefault(d => d is Designator_ZoneAddStockpile_Resources);
            Designator builtInDumpingStockpileDesignator = __result.FirstOrDefault(d => d is Designator_ZoneAddStockpile_Dumping);

            foreach (Designator designator in __result)
            {
                if (designator != builtInStockpileDesignator && designator != builtInDumpingStockpileDesignator)
                {
                    yield return designator;
                }

                if (designator is Designator_Deconstruct)
                {
                    if (DefaultsSettings.DefaultStockpileZones != null)
                    {
                        foreach (ZoneType type in DefaultsSettings.DefaultStockpileZones)
                        {
                            if (type.DesignatorType == typeof(Designator_ZoneAddStockpile_Resources))
                            {
                                yield return builtInStockpileDesignator;
                            }
                            if (type.DesignatorType == typeof(Designator_ZoneAddStockpile_Dumping))
                            {
                                yield return builtInDumpingStockpileDesignator;
                            }
                            if (type.DesignatorType == typeof(Designator_ZoneAddStockpile_Custom))
                            {
                                yield return new Designator_ZoneAddStockpile_Custom(type) { isOrder = true };
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(DesignationCategoryDef))]
    [HarmonyPatch("get_AllResolvedDesignators")]
    public static class Patch_DesignationCategoryDef_get_AllResolvedDesignators
    {
        public static void Postfix(DesignationCategoryDef __instance, ref List<Designator> __result)
        {
            if (__instance == DesignationCategoryDefOf.Zone)
            {
                __result = PatchUtility_DesignationCategoryDef.AddDesignators(__result).ToList();
            }
        }
    }

    [HarmonyPatch(typeof(DesignationCategoryDef))]
    [HarmonyPatch("get_ResolvedAllowedDesignators")]
    public static class Patch_DesignationCategoryDef_get_ResolvedAllowedDesignators
    {
        public static void Postfix(DesignationCategoryDef __instance, ref IEnumerable<Designator> __result)
        {
            if (__instance == DesignationCategoryDefOf.Zone)
            {
                __result = PatchUtility_DesignationCategoryDef.AddDesignators(__result);
            }
        }
    }

    [HarmonyPatch(typeof(DesignationCategoryDef))]
    [HarmonyPatch("get_AllResolvedAndIdeoDesignators")]
    public static class Patch_DesignationCategoryDef_get_AllResolvedAndIdeoDesignators
    {
        public static void Postfix(DesignationCategoryDef __instance, ref IEnumerable<Designator> __result)
        {
            if (__instance == DesignationCategoryDefOf.Zone)
            {
                __result = PatchUtility_DesignationCategoryDef.AddDesignators(__result);
            }
        }
    }
}
