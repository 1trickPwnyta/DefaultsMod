using UnityEngine;

namespace Defaults.PlantType
{
    public class DefaultSettingWorker_PlantType : DefaultSettingWorker
    {
        public DefaultSettingWorker_PlantType(DefaultSettingDef def) : base(def)
        {
        }

        public override void DoSetting(Rect rect)
        {
            rect.x += rect.width - 24f;
            rect.width = 24f;
            PlantTypeUtility.DrawPlantButton(rect);
        }
    }
}
