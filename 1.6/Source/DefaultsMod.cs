using Verse;
using HarmonyLib;
using System.Linq;
using Defaults.Defs;

namespace Defaults
{
    [StaticConstructorOnStartup]
    public static class DefaultsModInitializer
    {
        static DefaultsModInitializer()
        {
            DefaultsMod.Settings = DefaultsMod.Mod.GetSettings<DefaultsSettings>();
            DefaultsSettings.CheckForNewContent();

            Harmony harmony = new Harmony(DefaultsMod.PACKAGE_ID);
            harmony.PatchAllUncategorized();
            foreach (DefaultSettingsCategoryDef def in DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading.Where(d => d.Enabled && d.canDisable))
            {
                harmony.PatchCategory(def.defName);
            }
        }
    }

    public class DefaultsMod : Mod
    {
        public const string PACKAGE_ID = "defaults.1trickPwnyta";
        public const string PACKAGE_NAME = "1trickPwnyta's Defaults";

        public static DefaultsMod Mod;
        public static DefaultsSettings Settings;

        public DefaultsMod(ModContentPack content) : base(content)
        {
            Mod = this;
            Log.Message($"[{PACKAGE_NAME}] Loaded.");
        }

        public override string SettingsCategory() => PACKAGE_NAME;
    }
}
