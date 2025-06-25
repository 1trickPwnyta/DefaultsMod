using Verse;
using HarmonyLib;
using UnityEngine;
using RimWorld;
using System;
using Defaults.WorkbenchBills;

namespace Defaults
{
    [StaticConstructorOnStartup]
    public static class DefaultsModInitializer
    {
        static DefaultsModInitializer()
        {
            Harmony harmony = new Harmony(DefaultsMod.PACKAGE_ID);
            harmony.PatchAll();
            harmony.Patch(typeof(Pawn_TimetableTracker).GetConstructor(new[] { typeof(Pawn) }), null, typeof(Schedule.Patch_Pawn_TimetableTracker_ctor).GetMethod("Postfix"));
            harmony.Patch(typeof(RimWorld.PlaySettings).GetConstructor(new Type[] { }), null, typeof(Medicine.Patch_PlaySettings_ctor).GetMethod("Postfix"));
            harmony.Patch(typeof(RimWorld.PlaySettings).GetConstructor(new Type[] { }), null, typeof(PlaySettings.Patch_PlaySettings_ctor).GetMethod("Postfix"));
            harmony.Patch(typeof(ResourceReadout).GetConstructor(new Type[] { }), null, typeof(ResourceCategories.Patch_ResourceReadout_ctor).GetMethod("Postfix"));
            harmony.Patch(typeof(CompTempControl).Method("<CompGetGizmosExtra>b__14_2"), null, null, typeof(TargetTemperature.Patch_CompTempControl_CompGetGizmosExtra_b__14_2).GetMethod("Transpiler"));
            harmony.Patch(typeof(Bill_Production).GetConstructor(new[] { typeof(RecipeDef), typeof(Precept_ThingStyle) }), null, typeof(Patch_Bill_Production_ctor).GetMethod("Postfix"));
            harmony.Patch(typeof(MechanitorControlGroup).GetConstructor(new[] { typeof(Pawn_MechanitorTracker) }), null, typeof(MechWorkModes.Patch_MechanitorControlGroup).Method(nameof(MechWorkModes.Patch_MechanitorControlGroup.Postfix)));

            DefaultsMod.Settings = DefaultsMod.Mod.GetSettings<DefaultsSettings>();
            DefaultsSettings.CheckForNewContent();
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

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            DefaultsSettings.DoSettingsWindowContents(inRect);
        }
    }
}
