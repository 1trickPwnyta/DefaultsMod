using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.Medicine
{
    [HarmonyPatch(typeof(Zone_Growing), "get_PlantDefToGrow")]
    public static class Patch_Zone_Growing_get_PlantDefToGrow
    {
        public static void Prefix(Zone_Growing __instance, ref ThingDef ___plantDefToGrow)
        {
            if (___plantDefToGrow == null)
            {
                ___plantDefToGrow = PollutionUtility.SettableEntirelyPolluted(__instance)
                    ? ThingDefOf.Plant_Toxipotato
                    : Settings.Get<ThingDef>(Settings.PLANT_TYPE);
            }
        }
    }
}
