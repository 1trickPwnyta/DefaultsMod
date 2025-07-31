using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.WorkbenchBills
{
    public class Dialog_GlobalBillSettings : Window
    {
        public Dialog_GlobalBillSettings()
        {
            doCloseX = true;
            doCloseButton = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
        }

        public override Vector2 InitialSize => new Vector2(350f, 450f);

        public override void DoWindowContents(Rect inRect)
        {
            GlobalBillOptions options = Settings.Get<GlobalBillOptions>(Settings.GLOBAL_BILL_OPTIONS);
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            string ingredientRadiusLabel = "IngredientSearchRadius".Translate().Truncate(inRect.width * 0.6f);
            string ingredientRadiusValue = options.DefaultBillIngredientSearchRadius == 999f ? "Unlimited".TranslateSimple().Truncate(inRect.width * 0.3f) : options.DefaultBillIngredientSearchRadius.ToString("F0");
            listing.Label(ingredientRadiusLabel + ": " + ingredientRadiusValue);
            options.DefaultBillIngredientSearchRadius = listing.Slider((options.DefaultBillIngredientSearchRadius > 100f) ? 100f : options.DefaultBillIngredientSearchRadius, 3f, 100f);
            if (options.DefaultBillIngredientSearchRadius >= 100f)
            {
                options.DefaultBillIngredientSearchRadius = 999f;
            }

            listing.Label("AllowedSkillRange".Translate("") + ": " + (options.DefaultBillAllowedSkillRange.max == 20 ? "Unlimited".TranslateSimple() : ""));
            listing.IntRange(ref options.DefaultBillAllowedSkillRange, 0, 20);
            listing.Gap();

            if (listing.ButtonText(options.DefaultBillStoreMode.LabelCap))
            {
                Find.WindowStack.Add(new FloatMenu(DefDatabase<BillStoreModeDef>.AllDefsListForReading.Where(d => d != BillStoreModeDefOf.SpecificStockpile).Select(d => new FloatMenuOption(d.LabelCap, () =>
                {
                    options.DefaultBillStoreMode = d;
                })).ToList()));
            }
            listing.Gap();

            listing.CheckboxLabeled("Defaults_LimitBillsTo15".Translate(), ref options.LimitBillsTo15);

            listing.End();
        }
    }
}
