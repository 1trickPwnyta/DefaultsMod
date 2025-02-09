using HarmonyLib;
using RimWorld;
using System.Linq;

namespace Defaults.WorkbenchBills
{
    [HarmonyPatch(typeof(Building_WorkTable))]
    [HarmonyPatch(nameof(Building_WorkTable.SpawnSetup))]
    public class Patch_Building_WorkTable
    {
        public static void Postfix(Building_WorkTable __instance, bool respawningAfterLoad)
        {
            if (!respawningAfterLoad)
            {
                foreach (BillTemplate bill in DefaultsSettings.DefaultWorkbenchBills.Where(s => s.workbenchGroup.Contains(__instance.def)).SelectMany(s => s.bills))
                {
                    if (bill.use && __instance.billStack.Count < 15)
                    {
                        __instance.billStack.AddBill(bill.ToBill());
                    }
                }
            }
        }
    }
}
