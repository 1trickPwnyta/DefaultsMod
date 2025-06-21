using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.Medicine
{
    public class DefaultSettingWorker_MedCarry : DefaultSettingWorker<ThingDef>
    {
        public DefaultSettingWorker_MedCarry(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.MEDICINE_TO_CARRY;

        protected override ThingDef Default => ThingDefOf.MedicineIndustrial;

        protected override void DoWidget(Rect rect)
        {
            rect.x += rect.width - 32f;
            rect.width = 32f;
            MedicineUtility.DrawMedicineButton(rect);
        }

        protected override void ExposeSetting()
        {
            Scribe_Defs.Look(ref setting, Key);
        }
    }
}
