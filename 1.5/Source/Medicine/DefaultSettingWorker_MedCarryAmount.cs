using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults.Medicine
{
    public class DefaultSettingWorker_MedCarryAmount : DefaultSettingWorker
    {
        public DefaultSettingWorker_MedCarryAmount(DefaultSettingDef def) : base(def)
        {
        }

        public override void DoSetting(Rect rect)
        {
            rect.x += rect.width - 60f;
            rect.width = 60f;
            if (Widgets.ButtonText(rect, DefaultsSettings.DefaultMedicineAmountToCarry.ToString()))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                for (int i = InventoryStockGroupDefOf.Medicine.min; i <= InventoryStockGroupDefOf.Medicine.max; i++)
                {
                    int amount = i;
                    options.Add(new FloatMenuOption(i.ToString(), () => DefaultsSettings.DefaultMedicineAmountToCarry = amount));
                }
                Find.WindowStack.Add(new FloatMenu(options));
            }
        }
    }
}
