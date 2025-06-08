using UnityEngine;
using Verse;

namespace Defaults.MechWorkModes
{
    public abstract class DefaultSettingWorker_MechWorkMode : DefaultSettingWorker
    {
        protected DefaultSettingWorker_MechWorkMode(DefaultSettingDef def) : base(def)
        {
        }

        public abstract MechWorkModeDef Default { get; }

        public abstract void SetDefault(MechWorkModeDef def);

        public override void DoSetting(Rect rect)
        {
            rect.x += rect.width - 24f;
            rect.width = 24f;
            MechWorkModeUtility.DrawWorkModeButton(rect, Default, SetDefault);
        }
    }
}
