using UnityEngine;

namespace Defaults.Medicine
{
    public class DefaultSettingWorker_MedCarry : DefaultSettingWorker
    {
        public DefaultSettingWorker_MedCarry(DefaultSettingDef def) : base(def)
        {
        }

        public override void DoSetting(Rect rect)
        {
            rect.x += rect.width - 32f;
            rect.width = 32f;
            MedicineUtility.DrawMedicineButton(rect);
        }
    }
}
