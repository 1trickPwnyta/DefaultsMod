using RimWorld;

namespace Defaults.AutoRebuild
{
    // Patched manually in mod constructor
    public static class Patch_PlaySettings_ctor
    {
        public static void Postfix(PlaySettings __instance)
        {
            __instance.autoRebuild = DefaultsSettings.DefaultAutoRebuild;
        }
    }
}
