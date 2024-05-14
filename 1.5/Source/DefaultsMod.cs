using Verse;
using HarmonyLib;
using UnityEngine;
using RimWorld;
using System;

namespace Defaults
{
    public class DefaultsMod : Mod
    {
        public const string PACKAGE_ID = "defaults.1trickPwnyta";
        public const string PACKAGE_NAME = "1trickPwnyta's Defaults";

        public static DefaultsSettings Settings;

        public DefaultsMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<DefaultsSettings>();

            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();
            harmony.Patch(typeof(Pawn_TimetableTracker).GetConstructor(new[] { typeof(Pawn) }), null, typeof(Schedule.Patch_Pawn_TimetableTracker_ctor).GetMethod("Postfix"));
            harmony.Patch(typeof(PlaySettings).GetConstructor(new Type[] { }), null, typeof(Medicine.Patch_PlaySettings_ctor).GetMethod("Postfix"));
            harmony.Patch(typeof(PlaySettings).GetConstructor(new Type[] { }), null, typeof(AutoRebuild.Patch_PlaySettings_ctor).GetMethod("Postfix"));
            harmony.Patch(typeof(ResourceReadout).GetConstructor(new Type[] { }), null, typeof(ResourceCategories.Patch_ResourceReadout_ctor).GetMethod("Postfix"));

            Log.Message($"[{PACKAGE_NAME}] Loaded.");
        }

        public override string SettingsCategory() => PACKAGE_NAME;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect rect = new Rect(inRect.x + 150f, inRect.y, inRect.width - 300f, inRect.height);
            base.DoSettingsWindowContents(rect);
            DefaultsSettings.DoSettingsWindowContents(rect);
        }
    }
}
