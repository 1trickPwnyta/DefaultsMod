using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Defaults.TargetTemperature
{
    [HarmonyPatch(typeof(CompTempControl))]
    [HarmonyPatch(nameof(CompTempControl.PostSpawnSetup))]
    public static class Patch_CompTempControl_PostSpawnSetup
    {
        public static void Postfix(CompTempControl __instance)
        {
            __instance.TargetTemperature = PatchUtility_CompTempControl.GetDefaultTargetTemperature(__instance);
        }
    }

    // Patched manually in mod constructor
    public static class Patch_CompTempControl_CompGetGizmosExtra_b__12_2
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 21f)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, typeof(PatchUtility_CompTempControl).Method(nameof(PatchUtility_CompTempControl.GetDefaultTargetTemperature)));
                    continue;
                }

                yield return instruction;
            }
        }
    }

    public static class PatchUtility_CompTempControl
    {
        public static float GetDefaultTargetTemperature(CompTempControl comp)
        {
            if (comp.parent is Building_Heater)
            {
                return DefaultsSettings.DefaultTargetTemperatureHeater;
            }
            if (comp.parent is Building_Cooler)
            {
                return DefaultsSettings.DefaultTargetTemperatureCooler;
            }
            return 21f;
        }
    }
}
