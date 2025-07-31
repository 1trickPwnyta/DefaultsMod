using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Defaults.WorkbenchBills
{
    [HarmonyPatchCategory("WorkbenchBills")]
    public static class PatchTargeted_BillStackLimit
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return typeof(BillStack).Method(nameof(BillStack.DoListing));
            yield return typeof(ITab_Bills).Method("FillTab");
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionsList = instructions.ToList();
            CodeInstruction billLimitInstruction = instructionsList.First(i => i.opcode == OpCodes.Ldc_I4_S && (sbyte)i.operand == 15);
            billLimitInstruction.opcode = OpCodes.Call;
            billLimitInstruction.operand = typeof(PatchUtility_BillStackLimit).Method(nameof(PatchUtility_BillStackLimit.GetBillLimit));
            return instructionsList;
        }
    }

    public static class PatchUtility_BillStackLimit
    {
        public static int GetBillLimit() => Settings.Get<GlobalBillOptions>(Settings.GLOBAL_BILL_OPTIONS).LimitBillsTo15 ? 15 : int.MaxValue;
    }
}
