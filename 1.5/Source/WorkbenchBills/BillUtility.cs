using RimWorld;
using System.Linq;
using Verse;

namespace Defaults.WorkbenchBills
{
    public static class BillUtility
    {
        public static bool IsSuspendedOrUnavailable(this Bill bill)
        {
            return bill.suspended || !bill.recipe.AvailableNow;
        }

        public static void DoBillRepeatModeMenu(this BillTemplate bill)
        {
            Find.WindowStack.Add(new FloatMenu(DefDatabase<BillRepeatModeDef>.AllDefsListForReading.Select(d => new FloatMenuOption(d.LabelCap, () =>
            {
                if (d != BillRepeatModeDefOf.TargetCount || bill.recipe.WorkerCounter.CanCountProducts(null))
                {
                    bill.repeatMode = d;
                }
                else
                {
                    Messages.Message("RecipeCannotHaveTargetCount".Translate(), MessageTypeDefOf.RejectInput, false);
                }
            })).ToList()));
        }
    }
}
