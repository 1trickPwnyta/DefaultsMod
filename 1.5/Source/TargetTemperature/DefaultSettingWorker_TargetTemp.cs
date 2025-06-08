using UnityEngine;

namespace Defaults.TargetTemperature
{
    public abstract class DefaultSettingWorker_TargetTemp : DefaultSettingWorker
    {
        protected DefaultSettingWorker_TargetTemp(DefaultSettingDef def) : base(def)
        {
        }

        public abstract float Target { get; set; }

        public override void DoSetting(Rect rect)
        {
            rect.x += rect.width / 2;
            rect.width /= 2;
            float target = Target;
            UIUtility.TemperatureEntry(rect, ref target, 1, -50f, 50f);
            Target = target;
        }
    }
}
