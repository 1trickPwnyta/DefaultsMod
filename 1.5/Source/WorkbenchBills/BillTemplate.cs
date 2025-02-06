using RimWorld;
using Verse;

namespace Defaults.WorkbenchBills
{
    public class BillTemplate : IExposable
    {
        public RecipeDef recipe;
        public ThingFilter ingredientFilter;
        public float ingredientSearchRadius = 999f;
        public IntRange allowedSkillRange = new IntRange(0, 20);
        public bool slavesOnly = false;
        public bool mechsOnly = false;
        public bool nonMechsOnly = false;
        public BillRepeatModeDef repeatMode = BillRepeatModeDefOf.RepeatCount;
        public int repeatCount = 1;
        public BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;
        public int targetCount = 10;
        public bool pauseWhenSatisfied = false;
        public int unpauseWhenYouHave = 5;
        public bool includeEquipped = false;
        public bool includeTainted = false;
        public FloatRange hpRange = FloatRange.ZeroToOne;
        public QualityRange qualityRange = QualityRange.All;
        public bool limitToAllowedStuff = false;

        public static BillTemplate clipboard;

        private BillTemplate() { }

        public BillTemplate(RecipeDef recipe)
        {
            this.recipe = recipe;
            ingredientFilter = new ThingFilter();
            if (recipe.fixedIngredientFilter != null)
            {
                ingredientFilter.CopyAllowancesFrom(recipe.fixedIngredientFilter);
            }
        }

        public BillTemplate Clone()
        {
            BillTemplate clone = new BillTemplate();
            clone.recipe = recipe;
            clone.ingredientFilter = new ThingFilter();
            clone.ingredientFilter.CopyAllowancesFrom(ingredientFilter);
            clone.ingredientSearchRadius = ingredientSearchRadius;
            clone.allowedSkillRange = allowedSkillRange;
            clone.slavesOnly = slavesOnly;
            clone.mechsOnly = mechsOnly;
            clone.nonMechsOnly = nonMechsOnly;
            clone.repeatMode = repeatMode;
            clone.repeatCount = repeatCount;
            clone.storeMode = storeMode;
            clone.targetCount = targetCount;
            clone.pauseWhenSatisfied = pauseWhenSatisfied;
            clone.unpauseWhenYouHave = unpauseWhenYouHave;
            clone.includeEquipped = includeEquipped;
            clone.includeTainted = includeTainted;
            clone.hpRange = hpRange;
            clone.qualityRange = qualityRange;
            clone.limitToAllowedStuff = limitToAllowedStuff;
            return clone;
        }

        public void ExposeData()
        {
            Scribe_Defs.Look(ref recipe, "recipe");
            Scribe_Deep.Look(ref ingredientFilter, "ingredientFilter");
            Scribe_Values.Look(ref ingredientSearchRadius, "ingredientSearchRadius");
            Scribe_Values.Look(ref allowedSkillRange, "allowedSkillRange");
            Scribe_Values.Look(ref slavesOnly, "slavesOnly");
            Scribe_Values.Look(ref mechsOnly, "mechsOnly");
            Scribe_Values.Look(ref nonMechsOnly, "nonMechsOnly");
            Scribe_Defs.Look(ref repeatMode, "repeatMode");
            Scribe_Values.Look(ref repeatCount, "repeatCount");
            Scribe_Defs.Look(ref storeMode, "storeMode");
            Scribe_Values.Look(ref targetCount, "targetCount");
            Scribe_Values.Look(ref pauseWhenSatisfied, "pauseWhenSatisfied");
            Scribe_Values.Look(ref unpauseWhenYouHave, "unpauseWhenYouHave");
            Scribe_Values.Look(ref includeEquipped, "includeEquipped");
            Scribe_Values.Look(ref includeTainted, "includeTainted");
            Scribe_Values.Look(ref hpRange, "hpRange");
            Scribe_Values.Look(ref qualityRange, "qualityRange");
            Scribe_Values.Look(ref limitToAllowedStuff, "limitToAllowedStuff");
        }
    }
}
