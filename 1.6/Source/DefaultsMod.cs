using Verse;
using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using System.Collections.Generic;
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

            harmony.Patch(typeof(Pawn_TimetableTracker).GetConstructor(new[] { typeof(Pawn) }), postfix: typeof(Schedule.Patch_Pawn_TimetableTracker_ctor).Method(nameof(Schedule.Patch_Pawn_TimetableTracker_ctor.Postfix)));
            //harmony.Patch(typeof(PlaySettings).GetConstructor(new Type[] { }), postfix: typeof(Medicine.Patch_PlaySettings_ctor).Method(nameof(Medicine.Patch_PlaySettings_ctor.Postfix)));
            harmony.Patch(typeof(PlaySettings).GetConstructor(new Type[] { }), postfix: typeof(Misc.PlaySettings.Patch_PlaySettings_ctor).Method(nameof(Misc.PlaySettings.Patch_PlaySettings_ctor.Postfix)));
            harmony.Patch(typeof(ResourceReadout).GetConstructor(new Type[] { }), postfix: typeof(ResourceCategories.Patch_ResourceReadout_ctor).Method(nameof(ResourceCategories.Patch_ResourceReadout_ctor.Postfix)));
            harmony.Patch(typeof(CompTempControl).Method("<CompGetGizmosExtra>b__14_2"), transpiler: typeof(Misc.TargetTemperature.Patch_CompTempControl_CompGetGizmosExtra_b__14_2).Method(nameof(Misc.TargetTemperature.Patch_CompTempControl_CompGetGizmosExtra_b__14_2.Transpiler)));
            harmony.Patch(typeof(Bill_Production).GetConstructor(new[] { typeof(RecipeDef), typeof(Precept_ThingStyle) }), postfix: typeof(WorkbenchBills.Patch_Bill_Production_ctor).Method(nameof(WorkbenchBills.Patch_Bill_Production_ctor.Postfix)));
            harmony.Patch(typeof(MechanitorControlGroup).GetConstructor(new[] { typeof(Pawn_MechanitorTracker) }), postfix: typeof(Misc.MechWorkModes.Patch_MechanitorControlGroup).Method(nameof(Misc.MechWorkModes.Patch_MechanitorControlGroup.Postfix)));

            HashSet<Type> patchedTypes = new HashSet<Type>();
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefsListForReading.Where(d => d.building?.defaultStorageSettings != null))
            {
                Type postMakeType = def.thingClass;
                while (postMakeType.GetMethod(nameof(Building.PostMake)).DeclaringType != postMakeType)
                {
                    postMakeType = postMakeType.BaseType;
                }
                if (!patchedTypes.Contains(postMakeType))
                {
                    harmony.Patch(postMakeType.Method(nameof(Building.PostMake)), postfix: typeof(StockpileZones.Buildings.Patch_Building_PostMake).Method(nameof(StockpileZones.Buildings.Patch_Building_PostMake.Postfix)));
                    patchedTypes.Add(postMakeType);
                }
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
