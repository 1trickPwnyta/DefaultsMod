using HarmonyLib;

namespace Defaults.Misc.PlaySettings
{
    [HarmonyPatchCategory("Misc")]
    [HarmonyPatch(typeof(RimWorld.PlaySettings))]
    [HarmonyPatch(MethodType.Constructor)]
    public static class Patch_PlaySettings_ctor
    {
        public static void Postfix(RimWorld.PlaySettings __instance)
        {
            __instance.autoRebuild = Settings.GetValue<bool>(Settings.AUTO_REBUILD);
            __instance.autoHomeArea = Settings.GetValue<bool>(Settings.AUTO_HOME_AREA);
            __instance.showZones = Settings.GetValue<bool>(Settings.ZONE_VISIBILITY);
            __instance.useWorkPriorities = Settings.GetValue<bool>(Settings.MANUAL_PRIORITIES);
        }
    }
}
