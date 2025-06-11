namespace Defaults.PlaySettings
{
    // Patched manually in mod constructor
    public static class Patch_PlaySettings_ctor
    {
        public static void Postfix(RimWorld.PlaySettings __instance)
        {
            __instance.autoRebuild = Settings.GetValue<bool>(Settings.AUTO_REBUILD);
            __instance.autoHomeArea = Settings.GetValue<bool>(Settings.AUTO_HOME_AREA);
            __instance.useWorkPriorities = Settings.GetValue<bool>(Settings.MANUAL_PRIORITIES);
        }
    }
}
