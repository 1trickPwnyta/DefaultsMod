using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults.Medicine
{
    public class DefaultSettingWorker_MedCarryAmount : DefaultSettingWorker<int?>
    {
        public DefaultSettingWorker_MedCarryAmount(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.MEDICINE_AMOUNT_TO_CARRY;

        protected override int? Default => 0;

        protected override void DoWidget(Rect rect)
        {
            rect.x += rect.width - 60f;
            rect.width = 60f;
            if (Widgets.ButtonText(rect, setting.ToString()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                for (int i = InventoryStockGroupDefOf.Medicine.min; i <= InventoryStockGroupDefOf.Medicine.max; i++)
                {
                    int amount = i;
                    options.Add(new FloatMenuOption(i.ToString(), () => setting = amount));
                }
                Find.WindowStack.Add(new FloatMenu(options));
            }
        }

        protected override void ExposeSetting()
        {
            Scribe_Values.Look(ref setting, Key);
        }
    }
}
