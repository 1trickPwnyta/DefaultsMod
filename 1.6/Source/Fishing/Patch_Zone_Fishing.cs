using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Defaults.Fishing
{
    [HarmonyPatchCategory("Fishing")]
    [HarmonyPatch(typeof(Zone_Fishing))]
    [HarmonyPatch(nameof(Zone_Fishing.GetGizmos))]
    [HarmonyPatchMod("Ludeon.RimWorld.Odyssey")]
    public static class Patch_Zone_Fishing
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> gizmos, Zone_Fishing __instance)
        {
            foreach (Gizmo gizmo in gizmos)
            {
                yield return gizmo;
            }

            if (!Settings.GetValue<bool>(Settings.HIDE_LOADDEFAULT))
            {
                yield return new Command_Action
                {
                    defaultLabel = "Defaults_ResetToDefault".Translate(),
                    defaultDesc = "Defaults_ResetFishingZoneDesc".Translate(),
                    icon = TexCommand.DesirePower,
                    action = () => FishingUtility.SetDefaultFishingZoneSettings(__instance)
                };
            }
        }
    }
}
