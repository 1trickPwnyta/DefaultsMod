using Defaults.Defs;
using RimWorld;
using Verse;

namespace Defaults.WorkbenchBills
{
    public class GlobalBillOptions : IExposable
    {
        public float DefaultBillIngredientSearchRadius = 999f;
        public IntRange DefaultBillAllowedSkillRange = new IntRange(0, 20);
        public BillStoreModeDef DefaultBillStoreMode = BillStoreModeDefOf.BestStockpile;
        public bool LimitBillsTo15 = true;

        public void ExposeData()
        {
            Scribe_Values.Look(ref DefaultBillIngredientSearchRadius, "DefaultBillIngredientSearchRadius", 999f);
            Scribe_Values.Look(ref DefaultBillAllowedSkillRange, "DefaultBillAllowedSkillRange", new IntRange(0, 20));
            Scribe_Defs_Silent.Look(ref DefaultBillStoreMode, "DefaultBillStoreMode");
            Scribe_Values.Look(ref LimitBillsTo15, "LimitBillsTo15", true);
            if (Scribe.mode == LoadSaveMode.PostLoadInit && DefaultBillStoreMode == null)
            {
                DefaultBillStoreMode = BillStoreModeDefOf.BestStockpile;
            }
        }
    }
}
