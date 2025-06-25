using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.WorkbenchBills
{
    [HarmonyPatch(typeof(ThingWithComps))]
    [HarmonyPatch(nameof(ThingWithComps.PostMake))]
    public static class Patch_ThingWithComps
    {
        public static void Postfix(ThingWithComps __instance)
        {
            List<WorkbenchBillStore> workbenchBills = Settings.Get<List<WorkbenchBillStore>>(Settings.WORKBENCH_BILLS);
            if (__instance is Building_WorkTable table && workbenchBills != null)
            {
                foreach (BillTemplate bill in workbenchBills.Where(s => s.workbenchGroup.Contains(table.def)).SelectMany(s => s.bills))
                {
                    if (bill.use && table.billStack.Count < 15)
                    {
                        table.billStack.AddBill(bill.ToBill());
                    }
                }
            }
        }
    }
}
