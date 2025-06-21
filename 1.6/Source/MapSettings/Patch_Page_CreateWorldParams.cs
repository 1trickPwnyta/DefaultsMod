using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.MapSettings
{
    [HarmonyPatch(typeof(Page_CreateWorldParams))]
    [HarmonyPatch(nameof(Page_CreateWorldParams.PostOpen))]
    public static class Patch_Page_CreateWorldParams
    {
        public static void Postfix()
        {
            Find.GameInitData.mapSize = DefaultsSettings.DefaultMapSize;
            Find.GameInitData.startingSeason = DefaultsSettings.DefaultStartingSeason;
        }
    }
}
