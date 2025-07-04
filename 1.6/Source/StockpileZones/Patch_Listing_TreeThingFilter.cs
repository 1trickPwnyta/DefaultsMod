using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace Defaults.StockpileZones
{
    [HarmonyPatchCategory("Storage")]
    [HarmonyPatch(typeof(Listing_TreeThingFilter))]
    [HarmonyPatch("DoCategoryChildren")]
    public static class Patch_Listing_TreeThingFilter_DoCategoryChildren
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool foundManager = false;
            object label = null;

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == typeof(Find).Method("get_HiddenItemsManager"))
                {
                    foundManager = true;
                }
                if (foundManager && label == null && instruction.opcode == OpCodes.Brfalse_S)
                {
                    label = instruction.operand;
                }
            }

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == typeof(Find).Method("get_HiddenItemsManager"))
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Brfalse_S, label);
                    yield return new CodeInstruction(instruction.opcode, instruction.operand);
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
